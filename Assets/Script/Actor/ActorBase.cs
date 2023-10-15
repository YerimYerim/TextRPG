using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ActorBase
{
    public Stat playerStat = new Stat();
    
    /// <summary>
    /// 주는 데미지
    /// </summary>
    /// <returns></returns>
    public int DoDamage(ActorBase enemy)
    {
        ConfigTableData configTableData = GameDataManager.Instance._configTableData.Find(_ => _.config_id == "status_damage_factor");
        var status = playerStat.GetStat((int)configTableData.GetValueConfigData());
        return status - enemy.ReduceDamage(this);
    }

    /// <summary>
    /// 받는 데미지 총량
    /// </summary>
    /// <returns></returns>
    public int ReduceDamage(ActorBase enemy)
    {
        ConfigTableData enemyDefenseFactor = GameDataManager.Instance._configTableData.Find(_ => _.config_id == "status_defense_factor");
        var enemyDefenseValue  = enemy.playerStat.GetStat((int)enemyDefenseFactor.GetValueConfigData());
        
        ConfigTableData statusReduceFactor  = GameDataManager.Instance._configTableData.Find(_ => _.config_id == "status_reduce_defense_factor");
        var myReduceStatValue = playerStat.GetStat((int)statusReduceFactor.GetValueConfigData());

        return Math.Max(0, enemyDefenseValue - myReduceStatValue);
    }

    public bool IsHit(ActorBase enemy)
    {
        ConfigTableData enemyDefenseFactor = GameDataManager.Instance._configTableData.Find(_ => _.config_id == "status_dodge_factor");
        var enemyDodgeFactor  = enemy.playerStat.GetStat((int)enemyDefenseFactor.GetValueConfigData());
        
        ConfigTableData statusReduceFactor  = GameDataManager.Instance._configTableData.Find(_ => _.config_id == "status_hit_factor");
        var myHitFactor = playerStat.GetStat((int)statusReduceFactor.GetValueConfigData());
        var hitRandFactor = Random.Range(0, myHitFactor + enemyDodgeFactor);
        
        return hitRandFactor < enemyDodgeFactor;
    }
}
