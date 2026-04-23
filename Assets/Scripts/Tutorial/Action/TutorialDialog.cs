using System;
using UnityEngine;

[Serializable]
public class TutorialDialog : TutorialAction
{
    [SerializeField] private string _dialogText;

    public TutorialDialog(TutorialActionData data)
    {
    }

    public override void Start()
    {
        if (Manager.UI == null)
        {
            errorString = $"{nameof(TutorialDialog)}: UIManager not found.";
            return;
        }

        var dialog = Manager.UI.Show<Popup_Tutorial>();
        dialog.Setup(_dialogText, Complete);
    }

    public override void End()
    {
        Manager.UI?.Hide<Popup_Tutorial>();
        base.End();
    }
}
