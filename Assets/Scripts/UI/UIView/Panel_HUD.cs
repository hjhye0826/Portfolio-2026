using R3;
using System;
using TMPro;
using UnityEngine;

public class Panel_HUD : UIView
{
    [SerializeField] private TMP_Text TextPlayTime;
    [SerializeField] private TMP_Text TextScore;

    private void Start()
    {
        Manager.Game.PlayTime.ThrottleLast(TimeSpan.FromSeconds(0.1f), UnityTimeProvider.Update)
                                .Subscribe(d => TextPlayTime.text = $"Play Time : {d.ToTimeString()}")
                                .AddTo(this);
    }

}
