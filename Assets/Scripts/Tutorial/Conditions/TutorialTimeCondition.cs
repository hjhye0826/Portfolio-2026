public class TutorialTimeCondition : ITutorialCondition
{
    private readonly float _triggerTime;

    public TutorialTimeCondition(float triggerTime)
    {
        _triggerTime = triggerTime;
    }

    public bool Evaluate() => Manager.Game.PlayTime.Value >= _triggerTime;
}
