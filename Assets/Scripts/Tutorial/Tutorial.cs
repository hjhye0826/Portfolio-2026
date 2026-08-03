using System;
using System.Collections.Generic;
using System.Linq;

public class Tutorial
{
    public bool IsCompleted { get; private set; }
    public bool IsRunning { get; private set; }
    public string ErrorString { get; protected set; }

    public TutorialData Data { get; private set; }

    private ITutorialCondition _condition;
    private List<TutorialStepData> _steps;
    private List<TutorialAction> _currentActions;

    private int _currentStep;

    public Tutorial(TutorialData data, List<TutorialStepData> steps)
    {
        Data = data;

        _condition = data.ConditionType switch
        {
            ConditionType.Time       => new TutorialTimeCondition(data.TriggerTime),
            ConditionType.NearObject => new TutorialNearObjectCondition(data.TargetKey, data.Range),
            _                        => throw new ArgumentException($"Unknown ConditionType: {data.ConditionType}")
        };

        _steps = steps;
    }

    public bool Evaluate() => _condition.Evaluate();

    public void StartTutorial()
    {
        Manager.Game.Pause();
        if (Data.BlockMovement)
            Manager.Game.LockMovement();
        Manager.UI.Show<Popup_Tutorial>();

        IsRunning = true;
        _currentStep = 0;

        StartStep(0);
    }

    public void CompleteTutorial()
    {
        IsCompleted = true;
        
        EndTutorial();
    }

    public void EndTutorial()
    {
        IsRunning = false;

        Manager.Game.Resume();
        Manager.Game.UnlockMovement();
        Manager.UI.Hide<Popup_Tutorial>();
    }

    public void Progress()
    {
        var completedCount = 0;
        foreach (var action in _currentActions)
        {
            if (action.IsCompleted)
            {
                completedCount++;
                continue;
            }

            action.OnProcess();

            if (action.ErrorString != null)
            {
                ErrorString = action.ErrorString;
                EndTutorial();
                return;
            }
        }

        var mode = _steps[_currentStep].CompletionMode;
        var isStepComplete = mode == StepCompletionMode.Any
                                ? completedCount >= 1
                                : completedCount == _currentActions.Count;

        if (isStepComplete)
        {
            EndCurrentActions();
            AdvanceStep();
        }
    }

    private void EndCurrentActions()
    {
        foreach (var action in _currentActions)
        {
            if (false == action.IsCompleted)
            {
                action.End();
            }
        }
    }

    private void StartStep(int step)
    {
        _currentActions = _steps[step].Actions
                                    .Select(d => CreateTutorialAction(d))
                                    .ToList();

        foreach (var action in _currentActions)
        {
            action.StartAction();
        }
    }

    private void AdvanceStep()
    {
        _currentStep++;

        if (_currentStep > _steps.Count - 1)
        {
            CompleteTutorial();
            return;
        }

        StartStep(_currentStep);
    }

    private TutorialAction CreateTutorialAction(TutorialActionData data)
    {
        return data.ActionType switch
        {
            ActionType.Dialog     => new TutorialDialog(data),
            ActionType.HideDialog => new TutorialHideDialog(data),
            ActionType.WaitTime   => new TutorialWaitTime(data),
            ActionType.Touch      => new TutorialTouch(data),
            ActionType.Interact   => new TutorialInteract(data),
            _ => throw new ArgumentException($"Unknown ActionType: {data.ActionType}")
        };
    }
}
