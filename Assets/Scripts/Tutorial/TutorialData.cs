using System;
using UnityEngine;

public enum ConditionType
{
    Time,
    NearObject,
}

[Serializable]
public class TutorialData
{
    [SerializeField, TextArea] private string _summary;
    [SerializeField] private int _id;
    [SerializeField] private int _actionGroupId;
    [SerializeField] private ConditionType _conditionType;
    [SerializeField] private float _triggerTime;
    [SerializeField] private float _range;
    [SerializeField] private ObjectKey _targetKey;
    [SerializeField] private bool _blockMovement = false;

    public int Id => _id;
    public int ActionGroupId => _actionGroupId;
    public ConditionType ConditionType => _conditionType;
    public float TriggerTime => _triggerTime;
    public float Range => _range;
    public ObjectKey TargetKey => _targetKey;
    public bool BlockMovement => _blockMovement;
}
