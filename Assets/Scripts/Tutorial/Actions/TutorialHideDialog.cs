public class TutorialHideDialog : TutorialAction
{
    public TutorialHideDialog(TutorialActionData data) : base(data) { }

    public override void StartAction()
    {
        Manager.UI.Show<Popup_Tutorial>().HideDialog();

        Complete();
    }
}
