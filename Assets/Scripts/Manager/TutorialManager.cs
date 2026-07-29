using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialManager
{
    public bool IsPlay = false;

    private List<Tutorial> _tutorials = new();
    private Tutorial _currentTutorial = null;

    public void Init()
    {
        var tutorialDataSO = Resources.Load<TutorialDataSO>("Data/Tutorial/TutorialDataSO");
        var tutorialStepsSO = Resources.Load<TutorialStepsSO>("Data/Tutorial/TutorialStepsSO");

        _tutorials = tutorialDataSO.Tutorials
            .Select(d => new Tutorial(d, tutorialStepsSO.GetGroup(d.ActionGroupId)))
            .ToList();
    }

    public void Progress()
    {
        if (_currentTutorial?.IsRunning == true)
        {
            _currentTutorial.Progress();
            return;
        }

        ApplyTutorial();

        if (_currentTutorial?.ErrorString != null)
        {
            Debug.LogError($"[Tutorial] {_currentTutorial.Data.Id}: {_currentTutorial.ErrorString}");
            _currentTutorial = null;
        }

        if (_currentTutorial?.IsCompleted == true)
        {
            _currentTutorial = null;
        }
    }

    private void ApplyTutorial()
    {
        foreach (var tutorial in _tutorials)
        {
            if (tutorial.IsCompleted) continue;
            if (tutorial.ErrorString != null) continue;

            if (false == tutorial.IsRunning && tutorial.Evaluate())
            {
                _currentTutorial = tutorial;
                tutorial.StartTutorial();
                break;
            }
        }

    }


    //public bool IsBtnTouchable(Transform trans)
    //{
    //    var clickAction = _orderActions.Find(d => d is TutorialBtnTouch && d.IsEnd == false);
    //    if (null == clickAction)
    //        return false;

    //    return ((TutorialBtnTouch)clickAction).CanClicked(trans);
    //}
    //public void BtnTouch(Transform ui)
    //{
    //    var action = _orderActions.Find(d => d is TutorialBtnTouch && d.IsEnd == false);
    //    if (null == action)
    //        return;

    //    ((TutorialBtnTouch)action).BtnTouch(ui);
    //}
}
