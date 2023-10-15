using System;
using Script.Manager;
using UnityEngine;

public class GameBattleManager : Singleton<GameBattleManager>
{
    public void DoBattle(ActorBase doingDamage, ActorBase receivingDamage)
    {
        var damage = Math.Max(1, doingDamage.DoDamage(receivingDamage) - receivingDamage.ReduceDamage(doingDamage));
        bool isHit = doingDamage.IsHit(receivingDamage);

        if (isHit)
        {
            var hp = GameDataManager.Instance._configTableData.Find(_ => _.config_id == "status_hp");
            receivingDamage.playerStat.AddStat((int)hp.GetValueConfigData(), -damage);
        }
        else
        {
            Debug.Log("빗나감");
        }
    }
}
