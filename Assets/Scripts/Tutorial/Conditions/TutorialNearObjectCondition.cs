using UnityEngine;

public class TutorialNearObjectCondition : ITutorialCondition
{
    private readonly string _targetName;
    private readonly float _range;
    private Transform _target;

    public TutorialNearObjectCondition(string targetName, float range)
    {
        _targetName = targetName;
        _range = range;
    }

    public bool Evaluate()
    {
        _target ??= GameObject.Find(_targetName)?.transform;
        if (_target == null)
        {
            return false;
        }

        return Vector3.Distance(Manager.Game.Player.position, _target.position) <= _range;
    }
}
