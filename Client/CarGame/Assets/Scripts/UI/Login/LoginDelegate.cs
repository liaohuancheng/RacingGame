using Assets.Scripts.Photon.Controller;
using Assets.Scripts.UI.Login;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginDelegate : MonoBehaviour
{
    //public LoginView loginView;
    public InputField UserNameInput;
    public InputField PasswordInput;
    public Button LoginBtn;
    public Button RegisterBtn;
    public Dropdown ServerNameDropDown;
    private LoginModel loginModel;
    private LoginController loginController;
    private static LoginDelegate _instance = null;
    public static LoginDelegate Instance
    {
        get
        {
            if (_instance == null)
            {    //查找场景中是否已经存在单例
                _instance = GameObject.FindObjectOfType<LoginDelegate>();
                if (_instance == null)
                {    //创建游戏对象然后绑定单例脚本
                    GameObject go = new GameObject("Singleton");
                    _instance = go.AddComponent<LoginDelegate>();
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
        loginModel = new LoginModel();
        loginController = new LoginController();
    }

    public void OnCLickLoginBtn()
    {
        loginController.Login(UserNameInput.text, PasswordInput.text);
    }

    public void OnClickRegisterBtn()
    {

    }

    public void InitServerList(List<CarCommon.Model.ServerProperty> serverLst)
    {
        ServerNameDropDown.ClearOptions();
        loginModel.ServerLst = serverLst;
        List<Dropdown.OptionData> datas = new List<Dropdown.OptionData>();
        foreach (var serverInfo in serverLst)
        {
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = serverInfo.Name;
            datas.Add(data);
        }
        ServerNameDropDown.AddOptions(datas);

    }
    //public void initWithView(ViewBase view)
    //{
    //    loginView = view as LoginView;
    //}

    //public void onViewRemoved()
    //{
    //}
}
