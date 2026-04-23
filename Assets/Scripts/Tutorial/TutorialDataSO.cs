using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Resources/Data/Tutorial/TutorialDataSO", menuName = "Tutorial/TutorialData")]
public class TutorialDataSO : ScriptableObject
{
    [SerializeField] private List<TutorialData> _tutorials = new();

    public IReadOnlyList<TutorialData> Tutorials => _tutorials;
}
