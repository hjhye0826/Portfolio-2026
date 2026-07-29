using TMPro;
using UnityEngine;

public class Popup_Tutorial : UIView
{
    [SerializeField] private GameObject _groupDialog;
    [SerializeField] private TMP_Text _dialogText;

    public void ShowDialog(string text)
    {
        _dialogText.text = text;
        _groupDialog.SetActive(true);
    }

    public void HideDialog() => _groupDialog.SetActive(false);
}
