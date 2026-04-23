using System;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Tutorial : UIView
{
    [SerializeField] private Text _dialogText;
    [SerializeField] private Button _confirmButton;

    private Action _onConfirm;

    public void Setup(string text, Action onConfirm)
    {
        _dialogText.text = text;
        _onConfirm = onConfirm;
    }

    protected override void OnShow()
    {
        _confirmButton.onClick.RemoveAllListeners();
        _confirmButton.onClick.AddListener(OnConfirmClicked);
    }

    protected override void OnHide()
    {
        _confirmButton.onClick.RemoveAllListeners();
    }

    private void OnConfirmClicked()
    {
        _onConfirm?.Invoke();
    }
}
