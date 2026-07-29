
public abstract class TutorialAction
{
    public bool IsCompleted { get; private set; }
    public string ErrorString { get; protected set; }

    public TutorialActionData Data { get; private set; }

    protected TutorialAction(TutorialActionData data)
    {
        Data = data;
    }

    public virtual void StartAction() {}

    public virtual void OnProcess() { }

    public virtual void Complete()
    {
        IsCompleted = true;
    }

    public virtual void End() { }
}
