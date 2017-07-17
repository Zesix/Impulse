using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarIncrementer : MonoBehaviour
{
    // Control parameters
    public Scrollbar Target;
    public ItemScrollerController MyItemScrollerComponent;

    public void Increment()
    {
        if (Target == null ) throw new Exception("Setup ScrollbarIncrementer first!");

        var Step = 1.0f/ MyItemScrollerComponent.CurrentNumberElements() ;

        Target.value = Mathf.Clamp(Target.value + Step, 0, 1);
    }

    public void Decrement()
    {
        if (Target == null ) throw new Exception("Setup ScrollbarIncrementer first!");

        var Step = 1.0f / MyItemScrollerComponent.CurrentNumberElements();

        Target.value = Mathf.Clamp(Target.value - Step, 0, 1);
    }
}