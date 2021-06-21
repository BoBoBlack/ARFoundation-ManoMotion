using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    private void Awake()
    {
        instance = this;
    }
    public Image Image_Des;
    public Text Text_Des;
     
    public void ShowDes(string desStr)
    {
        Image_Des.gameObject.SetActive(true);
        Text_Des.text = desStr;
    }
    public void HideDes()
    {
        Text_Des.text = null;
        Image_Des.gameObject.SetActive(false);
    }
}
