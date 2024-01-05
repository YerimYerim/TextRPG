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
        var damageFactor = (int)GameDataManager.Instance.GetValueConfigData(configTableData);
        var status = playerStat.GetStat((int)damageFactor);
        return status - enemy.ReduceDamage(this);
    }

    /// <summary>
    /// 받는 데미지 총량
    /// </summary>
    /// <returns></returns>
    public int ReduceDamage(ActorBase enemy)
    {
        ConfigTableData enemyDefenseFactor = GameDataManager.Instance._configTableData.Find(_ => _.config_id == "status_defense_factor");
        var statDefID = (int)GameDataManager.Instance.GetValueConfigData(enemyDefenseFactor);
        var enemyDefenseValue  = enemy.playerStat.GetStat(statDefID);
        
        ConfigTableData statusReduceFactor  = GameDataManager.Instance._configTableData.Find(_ => _.config_id == "status_reduce_defense_factor");
        var statReduceID = (int)GameDataManager.Instance.GetValueConfigData(statusReduceFactor);
        var myReduceStatValue = playerStat.GetStat(statReduceID);

        return Math.Max(0, enemyDefenseValue - myReduceStatValue);
    }

    public bool IsHit(ActorBase enemy)
    {
        ConfigTableData enemyDefenseFactor = GameDataManager.Instance._configTableData.Find(_ => _.config_id == "status_dodge_factor");
        var statDefID = (int)GameDataManager.Instance.GetValueConfigData(enemyDefenseFactor);
        var enemyDodgeFactor  = enemy.playerStat.GetStat((int)statDefID);
        
        ConfigTableData statusReduceFactor  = GameDataManager.Instance._configTableData.Find(_ => _.config_id == "status_hit_factor");
        var statReduceID = (int)GameDataManager.Instance.GetValueConfigData(statusReduceFactor);
        var myHitFactor = playerStat.GetStat((int)statReduceID);
        var hitRandFactor = Random.Range(0, myHitFactor + enemyDodgeFactor);
        
        return hitRandFactor < enemyDodgeFactor;
    }
}
