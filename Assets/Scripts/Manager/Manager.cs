using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }

    public static UIManager UI { get; private set; }
    public static GameManager Game { get; private set; }


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
        Game = new GameManager();
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;

        Game.Progress(deltaTime);
    }
}
