using System;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

[Serializable]
public class Tutorial
{
    public float triggerTime => Data.triggerTime;
    public string ErrorString { get; protected set; } = "";

    public bool IsCompleted { get; private set; }
    public bool IsRunning { get; private set; }

    public TutorialData Data { get; private set; }

    private List<TutorialActionData> _actions;
    private List<TutorialAction> _currentActions;

    private int _lastStep;
    private int _currentStep;

    public Tutorial(TutorialData data, List<TutorialActionData> actions)
    {
        Data = data;

        _actions = actions;
        _lastStep = actions.Last().step;
    }

    public void StartTutorial()
    {
        Debug.Log($"Start Tutorial : {Data.id}");
        Manager.Game.Pause();
        Manager.UI.Show<Popup_Tutorial>();

        IsRunning = true;
        _currentStep = 0;

        StartStep(0);
    }

    public void CompeleteTutorial()
    {
        IsCompleted = true;
        
        EndTutorial();
    }

    public void EndTutorial()
    {
        IsRunning = false;

        Manager.Game.Resume();
        Manager.UI.Hide<Popup_Tutorial>();
    }

    public void Progress()
    {
        var completedCount = 0;
        foreach (var action in _currentActions)
        {
            if(action.IsCompleted)
            {
                completedCount++;
                continue;
            }
            
            action.OnProcess();

            if (action.ErrorString != "")
            {
                ErrorString = action.ErrorString;
                EndTutorial();
                return;
            }
        }

        if(completedCount == _currentActions.Count)
        {
            AdvanceStep();
        }
    }

    private void StartStep(int step)
    {
        _currentActions = _actions
            .Where(d => d.step == step)
            .Select(d => CreateTutorialAction(d))
            .ToList();

        foreach (var action in _currentActions)
            action.StartAction();
    }


    private void AdvanceStep()
    {
        _currentStep++;
        if (_currentStep > _lastStep)
        {
            CompeleteTutorial();
            return;
        }

        StartStep(_currentStep);
    }

    private TutorialAction CreateTutorialAction(TutorialActionData data)
    {
        return data.actionType switch
        {
            ActionType.Dialog => new TutorialDialog(data),
            ActionType.Touch => new TutorialTouch(data),
            _ => throw new ArgumentException($"Unknown ActionType: {data.actionType}")
        };
    }
}
