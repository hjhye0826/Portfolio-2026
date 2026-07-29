using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExButton : Button
{
    private bool _ignoreTutoralClick = false;

    //public override void OnPointerClick(PointerEventData eventData)
    //{
    //    // ... 공통 클릭 처리 ...

    //    if (Manager.Tutorial != null && Manager.Tutorial.IsPlay)
    //    {
    //        // 튜토리얼 대상 버튼만 통과, 그 외 버튼은 클릭 차단
    //        if (Manager.Tutorial.IsBtnTouchable(transform))
    //        {
    //            Manager.Tutorial.BtnTouch(transform);
    //        }
    //        else if (_ignoreTutoralClick == false)
    //            return;
    //    }

    //    base.OnPointerClick(eventData);
    //}
}
