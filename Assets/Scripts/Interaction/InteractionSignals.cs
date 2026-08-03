using R3;

/// <summary>
/// 상호작용 관련 전역 신호 허브.
/// 플레이어가 대상을 상호작용(F)한 순간 Interacted가 방출된다.
/// 튜토리얼/퀘스트 등 소비자는 이 스트림을 구독하고 target.Key로 대상을 판별한다.
/// </summary>
public static class InteractionSignals
{
    public static readonly Subject<InteractableBase> Interacted = new();
}
