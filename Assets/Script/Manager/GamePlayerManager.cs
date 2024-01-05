using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class GamePlayerManager : Singleton<GamePlayerManager>
{
    public ActorBase myActor = new ActorBase();
    public int DeadCount = 0;
    public Dictionary<int, int> _killMonsterDic = new(); // monsterid , kill Count
    public Dictionary<int, int> _metMonsterDic = new(); // monsterid , kill Count


    protected override void Awake()
    {
        base.Awake();
        myActor ??= new ActorBase();
        DeadCount = 0;
        _killMonsterDic ??= new Dictionary<int, int>(); // monsterid , kill Count
        _metMonsterDic ??= new Dictionary<int, int>(); // monsterid , kill Count
    }

    public void InitMyActor()
    {
        var statusList = GameDataManager.Instance._statusData;
        myActor.playerStat = new Stat();
        foreach (var stat in statusList)
        {
            myActor.playerStat.AddStat(stat?.status_id ?? 0, stat?.default_status ?? 0 , false );
        }
    }
    public void AddKillMonsterCount(int id, int addCount)
    {
        if (_killMonsterDic.TryGetValue(id, out var curCount))
        {
            _killMonsterDic[id] += addCount;
        }
        else
        {
            _killMonsterDic.TryAdd(id, addCount);
        }
    }    
    
    public void AddMetMonsterCount(int id, int addCount)
    {
        if (_metMonsterDic.TryGetValue(id, out var curCount))
        {
            _metMonsterDic[id] += addCount;
        }
        else
        {
            _metMonsterDic.TryAdd(id, addCount);
        }
    }
    
    public bool CheckPlayerDead()
    {
        int hpKey = (int)GameDataManager.Instance.GetValueConfigData("status_hp");
        var curHp = myActor.playerStat.GetStat(hpKey);
        return curHp <= 0;
    }
    
    public override void SaveData(string[] fileName)
    {
        base.SaveData(fileName);
        
        var killMonsterDic = GameDataSaveManager.ToJson(_killMonsterDic);
        var metMonsterDic = GameDataSaveManager.ToJson(_metMonsterDic);
        var playerStatDic = GameDataSaveManager.ToJson(myActor?.playerStat?.GetStatusDataDictionary());
        
        GameDataSaveManager.Save(fileName[0], killMonsterDic);
        GameDataSaveManager.Save(fileName[1], metMonsterDic);
        GameDataSaveManager.Save(fileName[2], DeadCount.ToString());
        GameDataSaveManager.Save(fileName[3], playerStatDic);
    }
    
    public override void LoadData(string[] fileName)
    {
        base.LoadData(fileName);
        
        string killMonsterDicJson = GameDataSaveManager.Load(fileName[0]);
        string metMonsterDicJson = GameDataSaveManager.Load(fileName[1]);
        string deadCount = GameDataSaveManager.Load(fileName[2]);
        string playerStat = GameDataSaveManager.Load(fileName[3]);

        _killMonsterDic = GameDataSaveManager.FromJson<int, int>(killMonsterDicJson);
        _metMonsterDic = GameDataSaveManager.FromJson<int, int>(metMonsterDicJson);
        DeadCount = GameDataSaveManager.FromIntJson(deadCount);

        if (playerStat != null)
        {
            myActor.playerStat.SetStatusDataDictionary(GameDataSaveManager.FromJson<int, int>(playerStat));
        }
        else
        {
            InitMyActor();
        }
    }
}
