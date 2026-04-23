using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialManager
{
    private List<Tutorial> _tutorials = new();
    private Tutorial CurrentTutorial = null;

    public void Init()
    {
        var tutorialDataSO = Resources.Load<TutorialDataSO>("Data/Tutorial/TutorialDataSO");
        var tutorialActionsSO = Resources.Load<TutorialActionsSO>("Data/Tutorial/TutorialActionsSO");

        _tutorials = tutorialDataSO.Tutorials
            .Select(d => new Tutorial(d, tutorialActionsSO.GetGroup(d.actionGroupId)))
            .ToList();
    }

    public void Progress()
    {
        if (CurrentTutorial != null && CurrentTutorial.IsCompleted) 
        {
            CurrentTutorial = null;
        }

        foreach (var tutorial in _tutorials)
        {
            if (tutorial.IsCompleted) continue;
            if (tutorial.ErrorString != "") continue;

            if (!tutorial.IsRunning && Manager.Game.PlayTime.Value >= tutorial.triggerTime)
            {
                CurrentTutorial = tutorial;
                tutorial.StartTutorial();
                break;
            }
        }

        if (CurrentTutorial == null) return;

        CurrentTutorial.Progress();
    }
}
