using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<Tutorial> _tutorials = new();

    private float _currentPlayTime;

    private void Update()
    {
        _currentPlayTime += Time.deltaTime;

        foreach (var tutorial in _tutorials)
        {
            if (tutorial.IsCompleted) continue;

            if (!tutorial.IsRunning && _currentPlayTime >= tutorial.triggerTime)
                tutorial.Start();

            if (tutorial.IsRunning)
                tutorial.Update();
        }
    }
}
