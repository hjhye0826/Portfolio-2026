using UnityEngine;

public class TutorialWaitTime : TutorialAction
{
    private readonly float _duration;
    private float _elapsed;

    public TutorialWaitTime(TutorialActionData data) : base(data)
    {
        _duration = data.FloatValue;
    }

    public override void OnProcess()
    {
        _elapsed += Time.deltaTime;
        
        if (_elapsed >= _duration)
        {
            Complete();
        }
    }
}
