using System;


/// <summary>
/// 정적 매니저 카탈로그.
/// 각 매니저 인스턴스를 보관하고, GameMain의 요청에 따라 Init, Progress를 수행한다.
/// 호출부에서는 Manager.UI, Manager.Game 등으로 접근한다.
/// </summary>
public static class Manager
{
    public static UIManager UI { get; private set; }
    public static GameManager Game { get; private set; }
    public static TutorialManager Tutorial { get; private set; }


    public static void Initialize()
    {
        UI = new UIManager();
        UI.Init();

        Game = new GameManager();
        Game.Init();

        Tutorial = new TutorialManager();
        Tutorial.Init();
    }

    public static void Progress(float deltaTime)
    {
        Game.Progress(deltaTime);
        Tutorial.Progress();
    }
}
