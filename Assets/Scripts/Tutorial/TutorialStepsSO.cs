using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialStepsSO", menuName = "Tutorial/TutorialSteps")]
public class TutorialStepsSO : ScriptableObject
{
    [SerializeField] private List<TutorialStepData> _steps = new();

    public IReadOnlyList<TutorialStepData> Steps => _steps;

    public List<TutorialStepData> GetGroup(int groupId)
    {
        return _steps.Where(s => s.GroupId == groupId).ToList();
    }
}
