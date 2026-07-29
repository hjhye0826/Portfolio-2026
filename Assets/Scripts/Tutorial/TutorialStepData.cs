using System;
using System.Collections.Generic;
using UnityEngine;

public enum StepCompletionMode
{
    All,
    Any,
}

[Serializable]
public class TutorialStepData
{
    [SerializeField] private int _groupId;
    [SerializeField] private StepCompletionMode _completionMode;
    [SerializeField] private List<TutorialActionData> _actions = new();

    public int GroupId => _groupId;
    public StepCompletionMode CompletionMode => _completionMode;
    public IReadOnlyList<TutorialActionData> Actions => _actions;
}
