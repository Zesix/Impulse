using System;
using System.Collections.Generic;
using UnityEngine;

namespace Impulse.FiniteStateMachine
{
	/// <summary>
	/// Finite State Machine of <typeparamref name="TClassType"/> with enum states <typeparamref name="TEnumState"/> and enum transition <typeparamref name="TEnumTransition"/>.
	/// </summary>
	/// <typeparam name="TClassType"></typeparam>
	/// <typeparam name="TEnumState"></typeparam>
	/// <typeparam name="TEnumTransition"></typeparam>
	[Serializable]
	public class StateMachine<TClassType, TEnumState, TEnumTransition> where TClassType : MonoBehaviour
	{
		protected TClassType Parent;

		private Dictionary<TEnumState, State<TClassType, TEnumState, TEnumTransition>> _statesDictionary;

		protected State<TClassType, TEnumState, TEnumTransition> MCurrentState;

		/// <summary>
		/// Keeps track of transitions.
		/// </summary>
		private Stack<State<TClassType, TEnumState, TEnumTransition>> _stateStack;

		private bool _mDidSetInitialState;

		private bool _mTrackStates;
		public bool TrackStates
		{
			get { return _mTrackStates; }

			set	
			{ 
				_mTrackStates = value; 

				if(_mTrackStates && _stateStack == null)
				{
					_stateStack = new Stack<State<TClassType, TEnumState, TEnumTransition>>();
				}
			}
		}

		private bool _mDebug;
		public bool Debug
		{
			get { return _mDebug; }

			set	{ _mDebug = value; }
		}

		public StateMachine(TClassType parent, IEnumerable<State<TClassType, TEnumState, TEnumTransition>> states, 
			TEnumState initialState, bool debug = false, bool trackStates = false)
		{
			Parent = parent;
			_statesDictionary = new Dictionary<TEnumState, State<TClassType, TEnumState, TEnumTransition>>();
			_mDebug = debug;
			_mTrackStates = trackStates;

			if(_mTrackStates)
			{
				_stateStack = new Stack<State<TClassType, TEnumState, TEnumTransition>>();
			}

			MCurrentState = null;

			_mDidSetInitialState = false;

			foreach(var state in states)
			{
				if(state.GetType().IsSubclassOf(typeof(State<TClassType, TEnumState, TEnumTransition>)))
				{
					AddState(state);
					state.Initialize(this, parent);
					state.BuildTransitions();
				}
				else
				{
					UnityEngine.Debug.LogError(string.Format("State {0} is not subclass of {1}", state.GetType(), 
						typeof(State<TClassType, TEnumState, TEnumTransition>)));
				}
			}
			SetInitialState(initialState);
		}

		public State<TClassType, TEnumState, TEnumTransition> CurrentState => MCurrentState;

		public string CurrentStateName => MCurrentState.GetType().Name;

		/// <summary>
		/// Call this method on MonoBehaviour.OnDestroy
		/// </summary>
		public void Destroy()
		{
			ClearCurrentState();

			if(_mTrackStates)
				ClearHistory();

			Parent = null;
			_statesDictionary = null;
		}

		public void Update()
		{
			if (MCurrentState != null)
				MCurrentState.Update();
		}

		public void FixedUpdate()
		{
			if (MCurrentState != null)
				MCurrentState.FixedUpdate();
		}

		public void OnTriggerEnter(Collider collider)
		{
			if(MCurrentState != null)
				MCurrentState.OnTriggerEnter(collider);
		}

		public void OnTriggerStay(Collider collider)
		{
			if(MCurrentState != null)
				MCurrentState.OnTriggerStay(collider);
		}

		public void OnTriggerExit(Collider collider)
		{
			if(MCurrentState != null)
				MCurrentState.OnTriggerExit(collider);
		}

		public void OnCollisionEnter(Collision collision)
		{
			if(MCurrentState != null)
				MCurrentState.OnCollisionEnter(collision);
		}

		public void OnCollisionStay(Collision collision)
		{
			if(MCurrentState != null)
				MCurrentState.OnCollisionStay(collision);
		}

		public void OnCollisionExit(Collision collision)
		{
			if(MCurrentState != null)
				MCurrentState.OnCollisionExit(collision);
		}

		public void SetInitialState(TEnumState enumState)
		{
			if (!_mDidSetInitialState)
			{
				ChangeState(enumState);
				_mDidSetInitialState = true;
			}
			else
			{
				UnityEngine.Debug.LogError("ERROR! StateMachine<" + Parent.GetType().Name + ">.SetInitialState(" + 
									   enumState.GetType().Name + "." + enumState + "): initial state already set.");
			}
		}

		public void SetInitialState(State<TClassType, TEnumState, TEnumTransition> state)
		{
			if (!_mDidSetInitialState)
			{
				ChangeState(state);
				_mDidSetInitialState = true;
			}
			else
			{
				UnityEngine.Debug.LogError("ERROR! StateMachine<" + Parent.GetType().Name + ">.SetInitialState(" + 
				                           state.GetType().Name + "): initial state already set.");
			}
		}

		public bool MakeTransition(TEnumTransition enumTransition)
		{
			if (MCurrentState == null)
			{
				UnityEngine.Debug.LogError("ERROR! StateMachine<" + Parent.GetType().Name + ">.MakeTransition(" + 
			   		enumTransition.GetType().Name + "." + enumTransition + 
				                           "): current state is null. Did you forget to set the initial state?");
				return false;
			}

			var transitionState = MCurrentState.GetTransitionState(enumTransition);
			if (MCurrentState.StateId.Equals(transitionState))
			{
				UnityEngine.Debug.LogError("ERROR! StateMachine<" + Parent.GetType().Name + ">.MakeTransition(" + 
								   		enumTransition.GetType().Name + "." + enumTransition + 
									   	"): transition leads to current state OR the transition is probably invalid.");
				return false;
			}

			ChangeState(transitionState);
			return true;
		}

		public bool AddState(State<TClassType, TEnumState, TEnumTransition> newState, bool overwriteIfExists = true)
		{
			if (newState == null)
			{
				UnityEngine.Debug.LogError("ERROR! StateMachine<" + Parent.GetType().Name + ">.AddState(" + 
										newState.StateId.GetType().Name + "." + newState.StateId + ", null, " + 
								   		overwriteIfExists + "): called with null state.");
				return false;
			}

			if (overwriteIfExists)
			{
				if(_mDebug)
				{
					bool didOverwrite = _statesDictionary.ContainsKey(newState.StateId);
					UnityEngine.Debug.Log("StateMachine<" + Parent.GetType().Name + ">.AddState(" + 
									  	newState.StateId.GetType().Name + "." + newState.StateId + ", " + 
									  	newState.GetType().Name + ", " + overwriteIfExists + "): state " + 
									  	(didOverwrite ? "overwritten." : "added."));
				}
				_statesDictionary[newState.StateId] = newState;
			}
			else
			{
				try
				{
					if(_mDebug)
					{
						UnityEngine.Debug.Log("StateMachine<" + Parent.GetType().Name + ">.AddState(" + 
									  	newState.StateId.GetType().Name + "." + newState.StateId + ", " + 
									  	newState.GetType().Name + ", " + overwriteIfExists + "): state added.");
					}
					_statesDictionary.Add(newState.StateId, newState);
				}
				catch (ArgumentException)
				{
					UnityEngine.Debug.LogError("ERROR! StateMachine<" + Parent.GetType().Name + ">.AddState(" + 
									   	newState.StateId.GetType().Name + "." + newState.StateId + ", " + 
									   	newState.GetType().Name + ", " + overwriteIfExists + 
									   	"): trying to add state that already exists.");
					return false;
				}
			}

			return true;
		}

		public bool RemoveState(TEnumState enumState, bool forceIfCurrent = false)
		{
			if (_statesDictionary.ContainsKey(enumState))
			{
				if (_statesDictionary[enumState] == MCurrentState)
				{
					if (forceIfCurrent)
					{
						UnityEngine.Debug.LogWarning("WARNING! StateMachine<" + Parent.GetType().Name + 
									    ">.RemoveState(" + enumState.GetType().Name + "." + enumState + ", " + 
										forceIfCurrent + "): removing current state.");

						if(_mTrackStates)
						{
							if (_stateStack.Contains(_statesDictionary[enumState]))
							{
								UnityEngine.Debug.LogWarning("WARNING! StateMachine<" + Parent.GetType().Name + 
										">.RemoveState(" + enumState.GetType().Name + "." + enumState + ", " + 
										forceIfCurrent + "): removed state is in state stack.");
							}
						}

						MCurrentState = null;
						return _statesDictionary.Remove(enumState);
					}
					else
					{
						UnityEngine.Debug.LogError("ERROR! StateMachine<" + Parent.GetType().Name + ">.RemoveState(" + 
										enumState.GetType().Name + "." + enumState + ", " + forceIfCurrent + 
									   	"): trying to remove current state.");
						return false;
					}
				}
				else
				{
					if(_mTrackStates)
					{
						if (_stateStack.Contains(_statesDictionary[enumState]))
						{
							UnityEngine.Debug.LogWarning("WARNING! StateMachine<" + Parent.GetType().Name + 
										">.RemoveState(" + enumState.GetType().Name + "." + enumState + ", " + 
									  	forceIfCurrent + "): removed state is in state stack.");
						}
					}

					return _statesDictionary.Remove(enumState);
				}
			}

			UnityEngine.Debug.LogError("ERROR! StateMachine<" + Parent.GetType().Name + ">.RemoveState(" + 
								   		enumState.GetType().Name + "." + enumState + ", " + forceIfCurrent + 
			                           	"): trying to remove a state that does not exist.");
			return false;
		}

		private void ChangeState(TEnumState enumState)
		{
			if (!_statesDictionary.ContainsKey(enumState))
			{
				UnityEngine.Debug.LogError("ERROR! StateMachine<" + Parent.GetType().Name + ">.ChangeState(" + 
									   	enumState.GetType().Name + "." + enumState + "): state not found in FSM.");
				return;
			}

			if (_statesDictionary[enumState] == MCurrentState)
			{
				UnityEngine.Debug.LogError("ERROR! StateMachine<" + Parent.GetType().Name + ">.ChangeState(" + 
									   	enumState.GetType().Name + "." + enumState + 
									   	"): new state is the same as the current state.");
				return;
			}

			InternalChangeState(_statesDictionary[enumState], true);
		}

		private void ChangeState(State<TClassType, TEnumState, TEnumTransition> newState)
		{
			if (newState == null)
			{
				UnityEngine.Debug.LogError("ERROR! StateMachine<" + Parent.GetType().Name + 
									   	">.ChangeState(null): called with null state.");
				return;
			}

			if (newState == MCurrentState)
			{
				UnityEngine.Debug.LogError("ERROR! StateMachine<" + Parent.GetType().Name + ">.ChangeState(" + 
									   	newState.GetType().Name + "): new state is the same as the current state.");
				return;
			}

			InternalChangeState(newState, true);
		}

		private void InternalChangeState(State<TClassType, TEnumState, TEnumTransition> newState, bool isNewState)
		{
			if(_mDebug)
			{
				UnityEngine.Debug.Log("StateMachine<" + Parent.GetType().Name + ">.InternalChangeState(" + 
				                      	newState.GetType().Name + ").");
			}

			if (MCurrentState != null)
			{
				if(_mDebug)
				{
					UnityEngine.Debug.Log("StateMachine<" + Parent.GetType().Name + ">.InternalChangeState(" + 
									  	newState.GetType().Name + "): leaving current state " + 
									  	MCurrentState.GetType().Name + ".");
				}

				MCurrentState.Exit();
			}

			MCurrentState = newState;

			if(_mDebug)
			{
				UnityEngine.Debug.Log("StateMachine<" + Parent.GetType().Name + ">.InternalChangeState(" + 
				                      	newState.GetType().Name + "): entering new state " + 
				                      	MCurrentState.GetType().Name + ".");
			}

			MCurrentState.Enter();

			if (isNewState)
			{
				if(_mTrackStates)
				{
					_stateStack.Push(MCurrentState);
				}
			}
		}

		public void ClearCurrentState()
		{
			if (MCurrentState != null)
				MCurrentState.Exit();

			MCurrentState = null;
		}

		public void RevertToPreviousState()
		{
			if(_mTrackStates)
			{
				// Pop the first state on the stack (which is the current state).
				if (_stateStack.Count > 0)
				{
					_stateStack.Pop();
				}

				// If we still have a state in the stack, revert to it.
				if (_stateStack.Count > 0)
				{
					// Get the previous state.
					State<TClassType, TEnumState, TEnumTransition> previousState = _stateStack.Peek();

					if(_mDebug)
					{
						UnityEngine.Debug.Log("StateMachine<" + Parent.GetType().Name + ">.RevertToPreviousState(): " + 
										  	previousState.GetType().Name);
					}

					InternalChangeState(previousState, false);
				}
				else
				{
					UnityEngine.Debug.LogWarning("WARNING! StateMachine<" + Parent.GetType().Name + 
										 	">.RevertToPreviousState(): state stack is empty. Current state: " + 
										 	MCurrentState.GetType().Name + ".");
				}
			}
			else
			{
				UnityEngine.Debug.LogWarning("WARNING! StateMachine<" + Parent.GetType().Name + 
						">.RevertToPreviousState(): stack is not enabled. Please enable before calling this method.");
			}
		}

		public bool HasHistory()
		{
			return _stateStack != null && _stateStack.Count > 0;
		}

		public void ClearHistory()
		{
			if(_mTrackStates)
			{
				_stateStack.Clear();
			}
			else
			{
				UnityEngine.Debug.LogWarning("WARNING! StateMachine<" + Parent.GetType().Name + 
										 	">.ClearHistory(): stack is not enabled. No changes.");
			}
		}

		public bool IsInState(TEnumState enumState)
		{
			if (MCurrentState == null)
				return false;

			return MCurrentState == _statesDictionary[enumState];
		}

		public bool IsInState(System.Type stateType)
		{
			if (MCurrentState == null)
				return false;

			return MCurrentState.GetType() == stateType;
		}
	}
}
