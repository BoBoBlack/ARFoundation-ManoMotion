using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTapFX : MonoBehaviour
{
    public static ScreenTapFX instance;
    private void Awake()
    {
        instance = this;
        
    }
    /// <summary>
    /// 屏幕特效原始资源
    /// </summary>
    public GameObject fxSample;
    private void Start()
    {
        if (fxSample == null)
        {
            Debug.LogErrorFormat("没有找到屏幕特效");
            this.enabled = false;
        }
        else
        {
            fxSample.SetActive(false);
        }
    }
    public void PlayFX(Vector3 tapPos)
    {
        if (fxSample == null) return;
        GameObject fx = CreateFX();
        fx.name = Time.time.ToString(); ;

        Transform fxRectTrans = fx.GetComponent<Transform>();
        fxRectTrans.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        fxRectTrans.position = tapPos;
        fxRectTrans.LookAt(Camera.main.transform.position);
        fx.SetActive(true);
    }


    private GameObject CreateFX()
    {
        GameObject newFX = null;

        newFX = Instantiate(fxSample);

        return newFX;
    }
}
