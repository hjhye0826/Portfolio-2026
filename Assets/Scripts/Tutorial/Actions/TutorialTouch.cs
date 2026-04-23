using UnityEngine;
using UnityEngine.UI;

public class TutorialTouch : TutorialAction
{
    public TutorialTouch(TutorialActionData data) : base(data)
    {

    }


    public override void OnProcess()
    {
        base.OnProcess();

        if(Input.GetMouseButtonDown(0))
        {
            Complete();
        }
    }
}
