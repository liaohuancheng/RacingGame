using Assets.Scripts;
using Assets.Scripts.CarContorl;
using CarCommon.Model;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    //单例
    private static GameMgr _instance = null;
    public static GameMgr Instance
    {
        get
        {
            if (_instance == null)
            {    //查找场景中是否已经存在单例
                _instance = GameObject.FindObjectOfType<GameMgr>();
                if (_instance == null)
                {    //创建游戏对象然后绑定单例脚本
                    GameObject go = new GameObject("Singleton");
                    _instance = go.AddComponent<GameMgr>();
                }
            }
            return _instance;
        }

    }
    //坦克预设
    public GameObject[] carPrefabs;
    //战场中的所有坦克
    public Dictionary<int, CarContainer> CarDic = new Dictionary<int, CarContainer>();

    // Use this for initialization
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public CarContainer GetCarById(int Id)
    {
        return CarDic[Id];
    }

    //清理场景
    public void ClearBattle()
    {
        CarDic.Clear();
        GameObject[] Cars = GameObject.FindGameObjectsWithTag("Car");
        for (int i = 0; i < Cars.Length; i++)
            Destroy(Cars[i]);
    }

    //开始战斗
    public void StartBattle(int carCount,List<User> UserLst)
    {
        //每一辆坦克
        for (int i = 0; i < carCount; i++)
        {
            int id = UserLst[i].ID;
            GenerateCar(id, i);
        }
    }


    //产生坦克
    public void GenerateCar(int id, int count)
    {
        //获取出生点
        Transform sp = GameObject.Find("SpawnPoints").transform;
        Transform swopTrans;
        swopTrans = sp.GetChild(count);
        if (swopTrans == null)
        {
            Debug.LogError("GenerateCar出生点错误！");
            return;
        }
        //预设
        if (carPrefabs.Length < 2)
        {
            Debug.LogError("汽车预设数量不够");
            return;
        }
        //产生坦克
        GameObject carObj = Instantiate(carPrefabs[count]);
        carObj.name = id.ToString();
        carObj.transform.position = swopTrans.position;
        carObj.transform.rotation = swopTrans.rotation;
        //列表处理
        Car car = carObj.GetComponent<Car>(); ;
        CarContainer carContainer = new CarContainer()
        {
            Carobject = carObj,
            car = car
        };
        //玩家处理
        if (id == UserInfo.Instance.Id)
        {
            carContainer.car.ctrlType = Car.CtrlType.player;
            GameObject camera = GameObject.Find("Camera");
            ThridCamera thridCamera = camera.GetComponent<ThridCamera>();
            GameObject target = carContainer.Carobject;
            thridCamera.SetTarget(target.transform);
        }
        else
        {
            carContainer.car.ctrlType = Car.CtrlType.net;
            carContainer.car.InitNetCtrl();  //初始化网络同步
        }
        CarDic.Add(id, carContainer);
    }


    //public void RecvUpdateUnitInfo(ProtocolBase protocol)
    //{
    //    //解析协议
    //    int start = 0;
    //    ProtocolBytes proto = (ProtocolBytes)protocol;
    //    string protoName = proto.GetString(start, ref start);
    //    string id = proto.GetString(start, ref start);
    //    Vector3 nPos;
    //    Vector3 nRot;
    //    nPos.x = proto.GetFloat(start, ref start);
    //    nPos.y = proto.GetFloat(start, ref start);
    //    nPos.z = proto.GetFloat(start, ref start);
    //    nRot.x = proto.GetFloat(start, ref start);
    //    nRot.y = proto.GetFloat(start, ref start);
    //    nRot.z = proto.GetFloat(start, ref start);
    //    float turretY = proto.GetFloat(start, ref start);
    //    float gunX = proto.GetFloat(start, ref start);
    //    //处理
    //    Debug.Log("RecvUpdateUnitInfo " + id);
    //    if (!list.ContainsKey(id))
    //    {
    //        Debug.Log("RecvUpdateUnitInfo bt == null ");
    //        return;
    //    }
    //    BattleTank bt = list[id];
    //    if (id == GameMgr.instance.id)
    //        return;

    //    bt.tank.NetForecastInfo(nPos, nRot);
    //    bt.tank.NetTurretTarget(turretY, gunX); //稍后实现
    //}

    //public void RecvResult(ProtocolBase protocol)
    //{
    //    //解析协议
    //    int start = 0;
    //    ProtocolBytes proto = (ProtocolBytes)protocol;
    //    string protoName = proto.GetString(start, ref start);
    //    int winTeam = proto.GetInt(start, ref start);
    //    //弹出胜负面板
    //    string id = GameMgr.instance.id;
    //    BattleTank bt = list[id];
    //    if (bt.camp == winTeam)
    //    {
    //        PanelMgr.instance.OpenPanel<WinPanel>("", 1);
    //    }
    //    else
    //    {
    //        PanelMgr.instance.OpenPanel<WinPanel>("", 0);
    //    }
    //    //取消监听
    //    NetMgr.srvConn.msgDist.DelListener("UpdateUnitInfo", RecvUpdateUnitInfo);
    //    NetMgr.srvConn.msgDist.DelListener("Shooting", RecvShooting);
    //    NetMgr.srvConn.msgDist.DelListener("Hit", RecvHit);
    //    NetMgr.srvConn.msgDist.DelListener("Result", RecvResult);
    //}
}
