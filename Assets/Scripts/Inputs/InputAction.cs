using System;

[Serializable]
public struct InputAction
{
    public UIInputBase InputControl;
    public float InputValue;
    public InputValueComparisonOperator Comparison;
    public float NearEpsilon;

    public bool IsSatisfiedBy(InputAction inputAction)
    {
        return
            InputControl == inputAction.InputControl &&
            IsComparisonValueSatified(inputAction.InputValue);

    }

    private bool IsComparisonValueSatified(float value)
    {
        if(InputControl.InputType == InputType.Button) { return true; }

        switch (Comparison) {
            case InputValueComparisonOperator.Equals:
                return InputValue == value;
            case InputValueComparisonOperator.GreaterThan:
                return value > InputValue;
            case InputValueComparisonOperator.LessThan:
                return value < InputValue;
            case InputValueComparisonOperator.Near:
                return (InputValue - NearEpsilon) <= value && value <= (InputValue + NearEpsilon);
            default:
                return false;
        }
    }
}

public enum InputType
{
    Switch,
    Button,
    Slider
}

public enum InputValueComparisonOperator
{
    Equals,
    GreaterThan,
    LessThan,
    Near
}