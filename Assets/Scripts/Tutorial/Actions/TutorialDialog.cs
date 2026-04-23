using System;
using UnityEngine;

[Serializable]
public class TutorialDialog : TutorialAction
{
    [SerializeField] private string _dialogText;

    public TutorialDialog(TutorialActionData data) : base(data)
    {
        _dialogText = data.stringValue;
    }

    public override void StartAction()
    {
        var ui = Manager.UI.Show<Popup_Tutorial>();
        ui.SetDialogText(_dialogText);

        Complete();
    }
}
