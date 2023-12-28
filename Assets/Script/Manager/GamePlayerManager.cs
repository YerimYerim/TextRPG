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
}
