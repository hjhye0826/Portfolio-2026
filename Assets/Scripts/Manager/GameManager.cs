using R3;

public class GameManager
{
    public ReactiveProperty<float> PlayTime { get; private set; } = new ReactiveProperty<float>();
    public ReactiveProperty<int> Score { get; private set; } = new ReactiveProperty<int>();

    public bool IsPaused { get; private set; }

    public void Pause() => IsPaused = true;
    public void Resume() => IsPaused = false;

    public void Progress(float deltaTime)
    {
        if (IsPaused) return;

        PlayTime.Value += deltaTime;
    }
}
