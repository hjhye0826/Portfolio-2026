using System;
using UnityEngine;

public enum ActionType
{
    Dialog,
    HideDialog,
    WaitTime,
    Touch,
    Interact,
    UnlockMovement,
    WaitDoorOpen,
}

[Serializable]
public class TutorialActionData
{
    [SerializeField, TextArea] private string _summary;
    [SerializeField] private int _id;
    [SerializeField] private ActionType _actionType;
    [SerializeField] private string _stringValue;
    [SerializeField] private float _floatValue;
    [SerializeField] private int _intValue;
    [SerializeField] private ObjectKey _objectKey;

    public int Id => _id;
    public ActionType ActionType => _actionType;
    public string StringValue => _stringValue;
    public float FloatValue => _floatValue;
    public int IntValue => _intValue;
    public ObjectKey ObjectKey => _objectKey;
}
