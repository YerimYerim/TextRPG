using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class GamePlayerManager : Singleton<GamePlayerManager>
{
    public ActorBase myActor = new();
    public int DeadCount = 0;
    public Dictionary<int, int> _killMonsterDic = new(); // monsterid , kill Count

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
}
