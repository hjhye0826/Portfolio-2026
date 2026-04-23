using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }

    public static UIManager UI { get; private set; }
    public static GameManager Game { get; private set; }
    public static TutorialManager Tutorial { get; private set; }


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        UI = new UIManager();
        UI.Init();

        Game = new GameManager();

        Tutorial = new TutorialManager();
        Tutorial.Init();
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;

        Game.Progress(deltaTime);
        Tutorial.Progress();
    }
}
