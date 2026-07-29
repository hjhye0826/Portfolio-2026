public class TutorialDialog : TutorialAction
{
    private readonly string _dialogText;

    public TutorialDialog(TutorialActionData data) : base(data)
    {
        _dialogText = data.StringValue;
    }

    public override void StartAction()
    {
        Manager.UI.Show<Popup_Tutorial>().ShowDialog(_dialogText);

        Complete();
    }
}
