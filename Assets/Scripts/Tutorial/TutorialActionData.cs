using System;
using UnityEngine;

public enum ActionType
{
    Dialog,
    WaitTime,
    Touch,
}

[Serializable]
public class TutorialActionData
{
    [SerializeField, TextArea] private string _summary;
    [SerializeField] private int _id;
    [SerializeField] private int _groupId;
    [SerializeField] private int _step;
    [SerializeField] private ActionType _actionType;
    [SerializeField] private string _stringValue;
    [SerializeField] private float _floatValue;
    [SerializeField] private int _intValue;

    public int id => _id;
    public int groupId => _groupId;
    public int step => _step;
    public ActionType actionType => _actionType;
    public string stringValue => _stringValue;
    public float floatValue => _floatValue;
    public int intValue => _intValue;
}
