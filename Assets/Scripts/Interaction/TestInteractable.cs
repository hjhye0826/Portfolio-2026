using UnityEngine;

/// <summary>
/// 상호작용 파이프라인 검증용 최소 구현.
/// F로 상호작용하면 로그를 남긴다. (반복 검증을 위해 완료 처리는 하지 않음)
/// </summary>
public class TestInteractable : InteractableBase
{
    public override void OnInteract(GameObject interactor)
    {
        Debug.Log($"[TestInteractable] Interacted by '{interactor.name}'. (key={(Key != null ? Key.name : "none")})");
    }
}
