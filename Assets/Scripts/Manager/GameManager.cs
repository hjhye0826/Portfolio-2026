public class GameManager
{
    public float PlayTime { get; private set; }
    public bool IsPaused { get; private set; }

    public void Pause() => IsPaused = true;
    public void Resume() => IsPaused = false;

    public void Progress(float deltaTime)
    {
        if (IsPaused) return;

        PlayTime += deltaTime;
    }
}
