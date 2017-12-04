using System;
using UnityEngine;

public class UIInputBase : MonoBehaviour
{
    public event EventHandler<InputActionEventArgs> Interacted;
    public float Value { get; protected set; }
    public InputType InputType { get; protected set; }

    protected void OnInteracted()
    {
        if(Interacted != null)
        {
            var args = new InputActionEventArgs();
            args.InputAction = new InputAction()
            {
                InputControl = this,
                InputValue = Value
            };

            Interacted(this, args);
        }
    }
}
