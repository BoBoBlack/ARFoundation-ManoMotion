using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManoObjInteraction : MonoBehaviour
{
    public string des;
    public GameObject mControObj;

    Vector3 lastHandPos = Vector3.zero;
    ManoGestureTrigger lastState;
    private void Update()
    {
        //当触发点击手势时，触发点击事件
        ManoGestureTrigger curState = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_trigger;
        if (curState != lastState)
        {
            if (curState == ManoGestureTrigger.CLICK)
            {
                OnClick();
            }
            lastState = curState;
        }
        //当前手势为捏住状态时，持续触发拖拽事件，否则结束拖拽
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_continuous == ManoGestureContinuous.HOLD_GESTURE)
        {
            OnDraging();
        }
        else
        {
            EndDrag();
        }
    }
    private void OnEnable()
    {
        UIController.instance.ShowDes(des);
    }
    private void OnDisable()
    {
        lastHandPos = Vector3.zero;
    }
    #region CallBack
    void OnClick()
    {
        TrackingInfo tracking = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;
        Vector3 worPos = Camera.main.ViewportToWorldPoint(new Vector3(tracking.poi.x, tracking.poi.y, tracking.depth_estimation));
        ScreenTapFX.instance.PlayFX(worPos);

        if (UIController.instance.Image_Des.gameObject.activeInHierarchy)
            UIController.instance.HideDes();
        else
            UIController.instance.ShowDes(des);
    }
    public void OnDraging()
    {
        Vector3 vPos = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.poi;
        Vector3 sPos = Camera.main.ViewportToScreenPoint(vPos);
        if (lastHandPos == Vector3.zero)
        {
            lastHandPos = sPos;
            return;
        }
        float offsetX = sPos.x - lastHandPos.x;
        mControObj.transform.Rotate(-Vector3.up * offsetX * 0.5f, Space.Self);//绕Y轴进行旋转
        lastHandPos = sPos;
    }
    public void EndDrag()
    {
        lastHandPos = Vector3.zero;
    }
    
    #endregion

}
