using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialTouch : TutorialAction
{
    public TutorialTouch(TutorialActionData data) : base(data) { }

    public override void OnProcess()
    {
        var mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            Complete();
            return;
        }

        var touch = Touchscreen.current;
        if (touch != null && touch.primaryTouch.press.wasPressedThisFrame)
            Complete();
    }
}
