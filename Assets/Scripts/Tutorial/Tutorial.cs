using System;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

[Serializable]
public class Tutorial
{
    public float triggerTime => Data.triggerTime;

    public bool IsCompleted { get; private set; }
    public bool IsRunning { get; private set; }

    public TutorialData Data { get; private set; }
    private List<TutorialAction> _currentActions;
    private int _currentStepIndex;

    public Tutorial(TutorialData data, List<TutorialActionData> actions)
    {
        Data = data;

        _currentActions = actions
            .OrderBy(d => d.step)
            .Select(d => CreateTutorialAction(d))
            .ToList();
    }

    public void StartTutorial()
    {
        IsRunning = true;
        _currentStepIndex = 0;

        StartStep(0);
    }

    public void Update()
    {
        foreach (var action in _currentActions)
        {
            if (!action.IsCompleted)
                action.OnProcess();
        }

        foreach (var action in _currentActions)
        {
            if (action.errorString == "") continue;
            EndCurrentStep();
            IsRunning = false;
            return;
        }

        if (_currentActions.TrueForAll(a => a.IsCompleted))
        {
            EndCurrentStep();
            AdvanceStep();
        }
    }

    private void StartStep(int index)
    {
        //_currentActions = _steps[index].actions;
        foreach (var action in _currentActions)
            action.Start();
    }

    private void EndCurrentStep()
    {
        foreach (var action in _currentActions)
            action.End();
    }

    private void AdvanceStep()
    {
        _currentStepIndex++;
        //if (_currentStepIndex >= _steps.Count)
        //{
        //    IsRunning = false;
        //    IsCompleted = true;
        //    return;
        //}

        StartStep(_currentStepIndex);
    }

    private TutorialAction CreateTutorialAction(TutorialActionData data)
    {
        return data.actionType switch
        {
            ActionType.Dialog => new TutorialDialog(data),
            ActionType.WaitTime => new TutorialWaitTime(data),
            ActionType.Touch => new TutorialTouch(data),
            _ => throw new ArgumentException($"Unknown ActionType: {data.actionType}")
        };
    }
}
