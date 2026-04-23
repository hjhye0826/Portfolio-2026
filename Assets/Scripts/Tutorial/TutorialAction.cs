using System;
using UnityEngine;

[Serializable]
public abstract class TutorialAction
{
    public int TutorialDataId => Data.id;
    
    public bool IsCompleted { get; private set; }
    public string ErrorString { get; protected set; } = "";
    public TutorialActionData Data { get; private set; }

    protected TutorialAction(TutorialActionData data)
    {
        Data = data;
        Debug.Log($"Start Tutorial Action : {Data.id}");
    }

    public virtual void StartAction() {}

    public virtual void OnProcess() { }

    public virtual void Complete()
    {
        IsCompleted = true;
    }

    public virtual void End() { }
}
