using System;
using UnityEngine;

[Serializable]
public class TutorialTouch : TutorialAction
{
    [SerializeField] private GameObject _targetObject;

    private Camera _camera;

    public TutorialTouch(TutorialActionData data)
    {
    }

    public override void Start()
    {
        _camera = Camera.main;
        if (_targetObject == null)
            errorString = $"{nameof(TutorialTouch)}: Target object is not assigned.";
    }

    public override void OnProcess()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit)) return;

        if (hit.collider.gameObject == _targetObject)
            Complete();
    }
}
