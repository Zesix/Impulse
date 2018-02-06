using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Impulse.FiniteStateMachine
{
	/// <inheritdoc />
	/// <summary>
	/// Abstract class defining a state for the Finite State Machine of <typeparamref name="TClassType" /> with enum
	/// states <typeparamref name="TEnumState" /> and enum transition <typeparamref name="TEnumTransition" />.
	/// </summary>
	/// <typeparam name="TClassType"></typeparam>
	/// <typeparam name="TEnumState"></typeparam>
	/// <typeparam name="TEnumTransition"></typeparam>
	public abstract class State<TClassType, TEnumState, TEnumTransition> where TClassType : MonoBehaviour
	{		
		public abstract void Enter();
		public abstract void Update();
		public abstract void FixedUpdate();
		public abstract void Exit();
		public abstract void BuildTransitions();
		public virtual void OnTriggerEnter(Collider collider) {}
		public virtual void OnTriggerStay(Collider collider) {}
		public virtual void OnTriggerExit(Collider collider) {}
		public virtual void OnCollisionEnter(Collision collision) {}
		public virtual void OnCollisionStay(Collision collision) {}
		public virtual void OnCollisionExit(Collision collision) {}

		protected TClassType Parent { get; private set; }

		public StateMachine<TClassType, TEnumState, TEnumTransition> FiniteStateMachine { get; private set; }

		[SerializeField]
		protected TEnumState stateId;
		public TEnumState StateId => stateId;

		protected Dictionary<TEnumTransition, TEnumState> TransitionDictionary;

		public void Initialize(StateMachine<TClassType, TEnumState, TEnumTransition> fsm, TClassType parent)
		{
			Parent = parent;
			FiniteStateMachine = fsm;

			TransitionDictionary = new Dictionary<TEnumTransition, TEnumState>();
		}

		public bool AddTransition(TEnumTransition enumTransition, TEnumState enumState, bool overwriteIfExists = true)
		{
			if (overwriteIfExists)
			{
				if(FiniteStateMachine.Debug)
				{
					var didOverwrite = TransitionDictionary.ContainsKey(enumTransition);
					Debug.Log(stateId + " - State.AddTransition(" + enumTransition.GetType().Name + "." + 
					          enumTransition + ", " + enumState.GetType().Name + "." + enumState + ", " + 
					          overwriteIfExists + "): transition " + (didOverwrite ? "overwritten." : "added."));
				}
				TransitionDictionary[enumTransition] = enumState;
			}
			else
			{
				try
				{
					if(FiniteStateMachine.Debug)
					{
						Debug.Log(stateId + " - State.AddTransition(" + enumTransition.GetType().Name + "." + 
						          enumTransition + ", " + enumState.GetType().Name + "." + enumState + ", " + 
						          overwriteIfExists + "): transition added.");
					}
					TransitionDictionary.Add(enumTransition, enumState);
				}
				catch (ArgumentException)
				{
					Debug.LogError("ERROR! " + stateId + " - State.AddTransition(" + enumTransition.GetType().Name + 
					               "." + enumTransition + ", " + enumState.GetType().Name + "." + enumState + ", " + 
					               overwriteIfExists + "): trying to add transition that already exists.");
					return false;
				}
			}

			return true;
		}

		public bool RemoveTransition(TEnumTransition enumTransition)
		{
			if (TransitionDictionary.ContainsKey(enumTransition))
			{
				return TransitionDictionary.Remove(enumTransition);
			}

			Debug.LogError("ERROR! " + stateId + " State.RemoveTransition(" + enumTransition.GetType().Name + "." 
			               + enumTransition + "): trying to remove a transition that does not exist.");
			return false;
		}

		public TEnumState GetTransitionState(TEnumTransition enumTransition)
		{
			return TransitionDictionary.ContainsKey(enumTransition) ? TransitionDictionary[enumTransition] : stateId;
		}

		protected bool MakeTransition(TEnumTransition transition)
		{
			return FiniteStateMachine.MakeTransition(transition);
		}

		public Coroutine StartCoroutine(IEnumerator routine)
		{
			return Parent.StartCoroutine(routine);
		}

		public Coroutine StartCoroutine(string methodName)
		{
			return Parent.StartCoroutine(methodName);
		}

		public Coroutine StartCoroutine(string methodName, object value)
		{
			return Parent.StartCoroutine(methodName, value);
		}

		public void StopCoroutine(string methodName)
		{
			Parent.StopCoroutine(methodName);
		}

		public void StopCoroutine(IEnumerator routine)
		{
			Parent.StopCoroutine(routine);
		}

		public void StopCoroutine(Coroutine routine)
		{
			Parent.StopCoroutine(routine);
		}

		public void StopAllCoroutines()
		{
			Parent.StopAllCoroutines();
		}
	}
}