using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private List<Tutorial> _tutorials = new();

    private void Awake()
    {
        var tutorialDataSO = Resources.Load<TutorialDataSO>("Data/Tutorial/TutorialDataSO");
        var tutorialActionsSO = Resources.Load<TutorialActionsSO>("Data/Tutorial/TutorialActionsSO");

        _tutorials = tutorialDataSO.Tutorials
            .Select(d => new Tutorial(d, tutorialActionsSO.GetGroup(d.actionGroupId)))
            .ToList();
    }

    private void Update()
    {
        foreach (var tutorial in _tutorials)
        {
            if (tutorial.IsCompleted) continue;

            if (!tutorial.IsRunning && Manager.Game.PlayTime >= tutorial.triggerTime)
                tutorial.StartTutorial();

            if (tutorial.IsRunning)
                tutorial.Update();
        }
    }
}
