using System;
using UnityEngine;

[Serializable]
public class TutorialData
{
    [SerializeField, TextArea] private string _summary;
    [SerializeField] private int _id;
    [SerializeField] private int _actionGroupId;
    [SerializeField] private float _triggerTime;

    public int id => _id;
    public int actionGroupId => _actionGroupId;
    public float triggerTime => _triggerTime;
}
