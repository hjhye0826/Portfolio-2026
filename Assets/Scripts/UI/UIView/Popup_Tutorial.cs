using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Tutorial : UIView
{
    [SerializeField] TMP_Text DialogText;

    public void SetDialogText(string text)
    {
        DialogText.text = text;
    }
}
