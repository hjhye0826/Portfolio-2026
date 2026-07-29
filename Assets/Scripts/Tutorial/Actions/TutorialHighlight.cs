using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TutorialHighlight : TutorialAction
{
    protected Canvas _overCanvas { get; set; }

    private RectTransform rectTrans;
    private bool _addCanvas = false;
    private int _originOrder = 0;

    public TutorialHighlight(TutorialActionData data) : base(data)
    {
    }

    public override void StartAction()
    {
        _overCanvas = rectTrans.GetComponent<Canvas>();
        if (_overCanvas == null)
        {
            _addCanvas = true;
            _overCanvas = rectTrans.gameObject.AddComponent<Canvas>();
        }
        else
        {
            _originOrder = _overCanvas.sortingOrder;
        }

        _overCanvas.overrideSorting = true;
        _overCanvas.sortingOrder = 89;  // 다른 UI 위로 끌어올려 강조
    }

    public override void Complete()
    {
        if (_addCanvas)
        {
            GameObject.DestroyImmediate(_overCanvas);
        }
        else
        {
            _overCanvas.sortingOrder = _originOrder;
        }

        base.Complete();
    }
    
    
    //public string PanelName => _tbTutorialStep.Panel_Name;
    //public string UIName { get; private set; }
    
    //public Transform transform;

    //public void BtnTouch(Transform ui)
    //{
    //    var panel = ui.GetComponentInParent<UIView>();
    //    if (null == panel)
    //        return;

    //    if (!panel.name.Equals(PanelName))
    //        return;

    //    if (ui.name.Equals(UIName))
    //    {
    //        End();
    //    }
    //}
}
