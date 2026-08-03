using UnityEngine;

public class TutorialNearObjectCondition : ITutorialCondition
{
    private readonly ObjectKey _targetKey;
    private readonly float _range;

    public TutorialNearObjectCondition(ObjectKey targetKey, float range)
    {
        _targetKey = targetKey;
        _range = range;
    }

    public bool Evaluate()
    {
        if (Manager.Game == null || Manager.Game.Player == null) return false;

        var playerPos = Manager.Game.Player.position;
        var nearest = ObjectRegistry.GetNearest(_targetKey, playerPos);
        if (nearest == null) return false;

        return Vector3.Distance(playerPos, nearest.transform.position) <= _range;
    }
}
