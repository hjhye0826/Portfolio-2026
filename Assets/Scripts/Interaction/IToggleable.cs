/// <summary>
/// 레버 등으로 원격 토글되는 대상(문, 슬라이드 덮개 등)의 공통 인터페이스.
/// Lever는 대상 타입을 몰라도 이 인터페이스로 여닫을 수 있다.
/// </summary>
public interface IToggleable
{
    bool IsOpen { get; }
    void Toggle();
    void SetOpen(bool open);
}
