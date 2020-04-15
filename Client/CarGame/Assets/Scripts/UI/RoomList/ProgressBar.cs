using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private static ProgressBar _instance;

    private GameObject bg;
    public Image progressBar;
    private bool isAsyn = false;
    private AsyncOperation ao = null;
    public static ProgressBar Instance
    {
        get
        {
            if (_instance == null)
            {    //查找场景中是否已经存在单例
                _instance = GameObject.FindObjectOfType<ProgressBar>();
                if (_instance == null)
                {    //创建游戏对象然后绑定单例脚本
                    GameObject go = new GameObject("Singleton");
                    _instance = go.AddComponent<ProgressBar>();
                }
            }
            return _instance;
        }

    }

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        gameObject.SetActive(false);
        progressBar = transform.Find("image").GetComponent<Image>();
    }

    void Update()
    {
        if (isAsyn)
        {
            progressBar.fillAmount = ao.progress;
        }
    }

    public void Show(AsyncOperation ao)
    {
        gameObject.SetActive(true);
        isAsyn = true;
        this.ao = ao;
    }
}
