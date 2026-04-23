using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    private enum InteractionState { None, Single, Multi }

    private const int OverlapBufferSize = 20;

    [SerializeField] private float _detectionRadius = 3f;
    //[SerializeField] private UIGroup_InteractionSelection _selectionUI;

    private LayerMask _interactableLayer;

    private readonly Collider[] _overlapBuffer = new Collider[OverlapBufferSize];
    private readonly List<InteractableBase> _candidates = new();
    private InteractableBase _currentTarget;
    private int _currentIndex;
    private InteractionState _state;

    private void Awake()
    {
        _interactableLayer = LayerMask.GetMask("Interactable");
    }

    private void Update()
    {
        RefreshCandidates();
        HandleWheel();
        HandleInteract();
    }

    private List<InteractableBase> OverlapSphere()
    {
        var count = Physics.OverlapSphereNonAlloc(transform.position, _detectionRadius, _overlapBuffer, _interactableLayer);
        var results = new List<InteractableBase>();
        for (var i = 0; i < count; i++)
        {
            //if (_overlapBuffer[i].TryGetComponent<InteractableBase>(out var interactable) && interactable.CanInteract)
            //    results.Add(interactable);
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

        //_selectionUI?.Refresh(_currentIndex);
    }

    private void HandleWheel()
    {
        if (_state != InteractionState.Multi) return;

        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
            SetIndex((_currentIndex - 1 + _candidates.Count) % _candidates.Count);
        else if (scroll < 0f)
            SetIndex((_currentIndex + 1) % _candidates.Count);
    }

    private void HandleInteract()
    {
        if (!Input.GetKeyDown(KeyCode.F)) return;
        //if (_currentTarget == null || !_currentTarget.CanInteract) return;

        //_currentTarget.OnInteract(gameObject);
    }

    private void UpdateState()
    {
        _state = _candidates.Count switch
        {
            0 => InteractionState.None,
            1 => InteractionState.Single,
            _ => InteractionState.Multi,
        };

        switch (_state)
        {
            case InteractionState.None:
            case InteractionState.Single:
                //_selectionUI?.Hide();
                break;
            case InteractionState.Multi:
                //_selectionUI?.Show(_candidates);
                break;
        }
    }
}
