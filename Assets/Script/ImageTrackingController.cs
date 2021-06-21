using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ImageTrackingController : MonoBehaviour
{
    ARTrackedImageManager ImageTrackedManager;
    private Dictionary<string, GameObject> mPrefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> mCurSaveObjList = new Dictionary<string, GameObject>();

    private void Awake()
    {
        ImageTrackedManager = GetComponent<ARTrackedImageManager>();
        ChangeImageTrackingState();
    }
    void Start()
    {
        for (int i = 0; i < ImageTrackedManager.referenceLibrary.count; i++)
        {
            string bojName = ImageTrackedManager.referenceLibrary[i].name;
            GameObject obj = Resources.Load("ArObj/" + bojName) as GameObject;
            mPrefabs.Add(bojName, obj);
            mCurSaveObjList.Add(bojName, null);
        }
        ChangeImageTrackingState();
    }
    private void OnEnable()
    {
        ImageTrackedManager.trackedImagesChanged += OnTrackedImagesChanged;
    }
    void OnDisable()
    {
        ImageTrackedManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    /// <summary>
    /// 图片识别处理
    /// </summary>
    /// <param name="eventArgs"></param>
    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            //当图片是第一次识别时，实例化对应的模型
            OnImagesChanged(trackedImage);
        }
        for (int i = 0; i < eventArgs.updated.Count; i++)
        {
            //在ARCore中，图片丢失时，模型不会自动隐藏，所以这里对其进行手动隐藏处理
            //当前识别的图片丢失时，隐藏对应的物体
            if(eventArgs.updated[i].trackingState== UnityEngine.XR.ARSubsystems.TrackingState.Limited)
            {
                GameObject obj = mCurSaveObjList[eventArgs.updated[i].referenceImage.name];
                if (obj != null)
                    obj.SetActive(false);
            }
            //当图片再次识别时，显示刚刚隐藏的对应的模型
            else if(eventArgs.updated[i].trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                
                GameObject obj = mCurSaveObjList[eventArgs.updated[i].referenceImage.name];
                if (obj != null)
                {
                    obj.SetActive(true);
                    obj.transform.SetParent(eventArgs.updated[i].transform);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }     
            }
        }
    }
    private void OnImagesChanged(ARTrackedImage referenceImage)
    {
        GameObject obj = Instantiate(mPrefabs[referenceImage.referenceImage.name], referenceImage.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
        mCurSaveObjList[referenceImage.referenceImage.name]=obj;
    }
    #region 启用与禁用图像跟踪
    public void ChangeImageTrackingState()
    {
        ImageTrackedManager.enabled = !ImageTrackedManager.enabled;
        if (ImageTrackedManager.enabled)
            //禁用图像跟踪;
            SetAllImagesActive(true);
        else
            //启用图像跟踪;
            SetAllImagesActive(false);
    }

    void SetAllImagesActive(bool value)
    {
        foreach (var img in ImageTrackedManager.trackables)
            img.gameObject.SetActive(value);
    }
    #endregion
}
