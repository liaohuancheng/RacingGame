using Assets.Scripts.Photon.Controller;
using UnityEngine;
using UnityEngine.UI;

public class RegisterDgt : MonoBehaviour
{
    private RegisterController registerController;
    private static RegisterDgt _instance = null;
    public InputField userNameInput;
    public InputField passwordInput;
    public static RegisterDgt Instance
    {
        get
        {
            if (_instance == null)
            {    //查找场景中是否已经存在单例
                _instance = GameObject.FindObjectOfType<RegisterDgt>();
                if (_instance == null)
                {    //创建游戏对象然后绑定单例脚本
                    GameObject go = new GameObject("Singleton");
                    _instance = go.AddComponent<RegisterDgt>();
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
        registerController = FindObjectOfType<RegisterController>();
    }

    public void OnClickRegisterBtn()
    {
        registerController.Register(userNameInput.text, passwordInput.text);
    }

    public void OnClickCancelBtn()
    {

    }

    public void RegisterSuccess()
    {
        Debug.Log("注册成功");
    }

    public void RegisterFail()
    {
        Debug.Log("注册失败");
    }
}
