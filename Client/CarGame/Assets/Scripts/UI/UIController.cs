using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private static UIController _instance = null;
    public static UIController Instance
    {
        get
        {
            if (_instance == null)
            {    //查找场景中是否已经存在单例
                _instance = GameObject.FindObjectOfType<UIController>();
                if (_instance == null)
                {    //创建游戏对象然后绑定单例脚本
                    GameObject go = new GameObject("Singleton");
                    _instance = go.AddComponent<UIController>();
                }
            }
            return _instance;
        }

    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public void HideUI(string UIName)
    {
        GameObject UI = transform.Find(UIName).gameObject;
        UI.SetActive(false);
    }

    public void ShowUI(string UIName)
    {
        GameObject UI = transform.Find(UIName).gameObject;
        UI.SetActive(true);
    }

    
}
