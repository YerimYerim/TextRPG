using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class GamePlayerManager : Singleton<GamePlayerManager>
{
    public ActorBase myActor = new ActorBase();
    public int DeadCount = 0;
    public Dictionary<int, int> _killMonsterDic = new Dictionary<int, int>(); // monsterid , kill Count
    public Dictionary<int, int> _metMonsterDic = new Dictionary<int, int>(); // monsterid , kill Count


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
        foreach (var stat in statusList)
        {
            myActor.playerStat.AddStat(stat.status_id, stat?.default_status ?? 0 , false );
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
}
