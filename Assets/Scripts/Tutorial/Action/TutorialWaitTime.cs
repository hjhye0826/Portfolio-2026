using System;
using UnityEngine;

[Serializable]
public class TutorialWaitTime : TutorialAction
{
    [SerializeField] private float _waitDuration;

    private float _elapsed;

    public override void Start()
    {
        _elapsed = 0f;
    }

    public override void OnProcess()
    {
        _elapsed += Time.deltaTime;
        if (_elapsed >= _waitDuration)
            Complete();
    }
}
