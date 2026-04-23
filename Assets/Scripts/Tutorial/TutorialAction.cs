using System;
using UnityEngine;

[Serializable]
public abstract class TutorialAction
{
    [SerializeField] private string _tutorialDataId;

    public string tutorialDataId => _tutorialDataId;
    public bool IsCompleted { get; private set; }
    public string errorString { get; protected set; } = "";

    public virtual void Start() { }
    public virtual void OnProcess() { }

    public virtual void Complete()
    {
        IsCompleted = true;
    }

    public virtual void End() { }
}
