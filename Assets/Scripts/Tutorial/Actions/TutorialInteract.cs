using System;
using R3;

/// <summary>
/// 플레이어가 대상 오브젝트를 상호작용(F)하면 완료되는 튜토리얼 액션.
/// 액션 데이터에 ObjectKey가 지정되면 그 키를 가진 대상만 인정하고, 비어있으면 모든 상호작용을 인정한다.
/// </summary>
public class TutorialInteract : TutorialAction
{
    private readonly ObjectKey _targetKey;
    private IDisposable _subscription;

    public TutorialInteract(TutorialActionData data) : base(data)
    {
        _targetKey = data.ObjectKey;
    }

    public override void StartAction()
    {
        _subscription = InteractionSignals.Interacted.Subscribe(OnInteracted);
    }

    private void OnInteracted(InteractableBase target)
    {
        if (_targetKey != null && (target == null || target.Key != _targetKey))
            return;

        _subscription?.Dispose();
        _subscription = null;
        Complete();
    }

    public override void End()
    {
        _subscription?.Dispose();
        _subscription = null;
    }
}
