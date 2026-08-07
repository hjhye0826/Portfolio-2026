using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    private enum InteractionState { None, Single, Multi }

    private const int OverlapBufferSize = 20;

    [SerializeField] private float _detectionRadius = 3f;
    private UIGroup_InteractionSelection _selectionUI;

    private UIGroup_InteractionSelection SelectionUI
    {
        get
        {
            if (_selectionUI == null && Manager.UI != null)
                _selectionUI = Manager.UI.Show<Panel_HUD>()?.InteractionSelection;
            return _selectionUI;
        }
    }

    private LayerMask _interactableLayer;

    private readonly Collider[] _overlapBuffer = new Collider[OverlapBufferSize];
    private readonly List<InteractableBase> _candidates = new();
    private InteractableBase _currentTarget;
    private CarryableBlock _carriedBlock;
    private int _currentIndex;
    private InteractionState _state;

    public static bool IsCycleModeActive { get; private set; }

    // PlayerInput (Send Messages) 에서 채워주는 입력 상태
    private float _cycleInput;        // 한 프레임 동안 누적된 휠 값
    private bool _interactPressed;    // 단발성 F 입력

    private void Awake()
    {
        _interactableLayer = LayerMask.GetMask("Interactable");
        IsCycleModeActive = false;
    }

    // PlayerInput 메시지 콜백: Player 액션맵의 "CycleTarget" 액션 (마우스 휠)
    private void OnCycleTarget(InputValue value)
    {
        _cycleInput += value.Get<float>();
    }

    // PlayerInput 메시지 콜백: Player 액션맵의 "Interact" 액션 (F)
    private void OnInteract(InputValue value)
    {
        if (value.isPressed)
            _interactPressed = true;
    }

    private void Update()
    {
        RefreshCandidates();
        HandleWheel();
        HandleInteract();

        // 단발 입력은 한 프레임에만 소비
        _cycleInput = 0f;
        _interactPressed = false;
    }

    private List<InteractableBase> OverlapSphere()
    {
        var count = Physics.OverlapSphereNonAlloc(transform.position, _detectionRadius, _overlapBuffer, _interactableLayer);
        var results = new List<InteractableBase>();
        for (var i = 0; i < count; i++)
        {
            if (_overlapBuffer[i].TryGetComponent<InteractableBase>(out var interactable) && interactable.CanInteract)
            {
                // 이미 들고 있는 블럭은 상호작용 후보(및 UI)에서 제외한다.
                if (interactable is CarryableBlock carryable && carryable.IsCarried)
                    continue;
                results.Add(interactable);
            }
        }
        return results;
    }

    private void SortByDistance(List<InteractableBase> list)
    {
        list.Sort((a, b) =>
        {
            var distA = Vector3.Distance(transform.position, a.transform.position);
            var distB = Vector3.Distance(transform.position, b.transform.position);
            return distA.CompareTo(distB);
        });
    }

    private void RefreshCandidates()
    {
        var detected = OverlapSphere();
        SortByDistance(detected);

        if (CandidatesEqual(detected)) return;

        var previousTarget = _currentTarget;
        _candidates.Clear();
        _candidates.AddRange(detected);

        UpdateState();

        var restoredIndex = _candidates.IndexOf(previousTarget);
        SetIndex(restoredIndex >= 0 ? restoredIndex : 0);
    }

    private bool CandidatesEqual(List<InteractableBase> other)
    {
        if (_candidates.Count != other.Count) return false;
        for (var i = 0; i < _candidates.Count; i++)
            if (_candidates[i] != other[i]) return false;
        return true;
    }

    private void SetIndex(int index)
    {
        //if (_currentTarget is IFocusable prevFocusable)
        //    prevFocusable.OnDefocus();

        if (_candidates.Count == 0)
        {
            _currentTarget = null;
            _currentIndex = 0;
            return;
        }

        _currentIndex = Mathf.Clamp(index, 0, _candidates.Count - 1);
        _currentTarget = _candidates[_currentIndex];

        //if (_currentTarget is IFocusable newFocusable)
        //    newFocusable.OnFocus();

        SelectionUI?.Refresh(_currentIndex);
    }

    private void HandleWheel()
    {
        if (_state != InteractionState.Multi) return;

        if (_cycleInput > 0f)
            SetIndex((_currentIndex - 1 + _candidates.Count) % _candidates.Count);
        else if (_cycleInput < 0f)
            SetIndex((_currentIndex + 1) % _candidates.Count);
    }

    private void HandleInteract()
    {
        if (false == _interactPressed) return;

        // 블럭을 들고 있는 동안에는 F가 항상 내려놓기로 동작한다.
        // (들고 있는 블럭은 상호작용 후보에서 빠지므로 여기서 직접 처리한다.)
        if (_carriedBlock != null && _carriedBlock.IsCarried)
        {
            _carriedBlock.OnInteract(gameObject);
            InteractionSignals.Interacted.OnNext(_carriedBlock);
            _carriedBlock = null;
            return;
        }

        if (_currentTarget == null || !_currentTarget.CanInteract) return;

        _currentTarget.OnInteract(gameObject);
        InteractionSignals.Interacted.OnNext(_currentTarget);

        // 방금 집어든 블럭을 기억해 두었다가 다음 F 입력 때 내려놓는다.
        if (_currentTarget is CarryableBlock carryable && carryable.IsCarried)
            _carriedBlock = carryable;
    }

    private void UpdateState()
    {
        _state = _candidates.Count switch
        {
            0 => InteractionState.None,
            1 => InteractionState.Single,
            _ => InteractionState.Multi,
        };

        IsCycleModeActive = _state == InteractionState.Multi;

        switch (_state)
        {
            case InteractionState.None:
                SelectionUI?.Hide();
                break;
            case InteractionState.Single:
            case InteractionState.Multi:
                SelectionUI?.Show(_candidates);
                break;
        }
    }
}
