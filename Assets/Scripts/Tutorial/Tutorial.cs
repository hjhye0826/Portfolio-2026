using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tutorial
{
    [SerializeField] private float _triggerTime;
    [SerializeField] private List<TutorialStep> _steps = new();

    public float triggerTime => _triggerTime;
    public bool IsCompleted { get; private set; }
    public bool IsRunning { get; private set; }

    private List<TutorialAction> _currentActions;
    private int _currentStepIndex;

    public void Start()
    {
        if (_steps.Count == 0)
        {
            IsCompleted = true;
            return;
        }

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
        _currentActions = _steps[index].actions;
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
        if (_currentStepIndex >= _steps.Count)
        {
            IsRunning = false;
            IsCompleted = true;
            return;
        }
        StartStep(_currentStepIndex);
    }
}
