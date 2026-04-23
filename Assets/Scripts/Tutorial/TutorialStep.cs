using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TutorialStep
{
    [SerializeReference] public List<TutorialAction> actions = new();
}
