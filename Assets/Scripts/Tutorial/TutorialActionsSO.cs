using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Resources/Data/Tutorial/TutorialActionsSO", menuName = "Tutorial/TutorialActions")]
public class TutorialActionsSO : ScriptableObject
{
    [SerializeField] private List<TutorialActionData> _actions = new();

    public IReadOnlyList<TutorialActionData> Actions => _actions;

    public List<TutorialActionData> GetGroup(int groupId)
    {
        return _actions
            .Where(a => a.groupId == groupId)
            .OrderBy(a => a.step)
            .ToList();
    }
}
