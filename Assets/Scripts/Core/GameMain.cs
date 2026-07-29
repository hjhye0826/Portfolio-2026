using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

/// <summary>
/// 게임 전체의 진입점.
/// 씬에 단 하나의 GameObject로 존재하며, 일반 매니저들의 수명과 라이프사이클을 책임진다.
/// </summary>
public class GameMain : MonoSingleton<GameMain>
{
    protected override void Awake()
    {
        base.Awake();

        if (Instance != this) return;

        Manager.Initialize();
    }

    private void Update()
    {
        Manager.Progress(Time.deltaTime);
    }

}


