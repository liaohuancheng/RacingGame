using Assets.Scripts;
using Assets.Scripts.CarContorl;
using Assets.Scripts.Photon.Controller;
using CarCommon.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    //单例
    private static GameMgr _instance = null;
    private List<GameObject> arriveCheckPoitLst = new List<GameObject>();
    private float startTime;
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

    //汽车预设
    public GameObject[] carPrefabs;
    //战场中的所有汽车
    public Dictionary<int, CarContainer> CarDic = new Dictionary<int, CarContainer>();

    // Use this for initialization
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
        startTime = Time.time;
        InitScene();
    }

    private void InitScene()
    {
        arriveCheckPoitLst.Clear();
    }

    public void PassCheckPoint(GameObject checkPoint)
    {
        if (arriveCheckPoitLst.Contains(checkPoint))
        {
            return ;
        }
        arriveCheckPoitLst.Add(checkPoint);
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
        //每一辆汽车
        for (int i = 0; i < carCount; i++)
        {
            int id = UserLst[i].ID;
            GenerateCar(id, i);
        }
    }


    //产生汽车
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
        //产生汽车
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
            GameObject camerGameobject = GameObject.Find("CamerGameobject");
            ThridCamera thridCamera = camerGameobject.GetComponent<ThridCamera>();
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

    public void ArrivalTerminal()
    {
        if(arriveCheckPoitLst.Count == GameObject.FindGameObjectsWithTag("CheckPoint").Length)
        {
            BattleController.Instance.OnPlayerFinshGame(Time.time - startTime);
        }
        
    }
}
