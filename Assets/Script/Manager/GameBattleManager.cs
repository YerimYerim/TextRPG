using System;
using Script.Manager;
using UnityEngine;

public class GameBattleManager : Singleton<GameBattleManager>
{
    public (bool isHit, int damage,string message) DoBattle(ActorBase attacker, ActorBase defencer, string resultSuccess, string resultFail)
    {
        // player 가 enemy 를 공격
        bool isHit = attacker.IsHit(defencer);

        if (isHit)
        {
            return DoDamageIgnoreDodge(attacker, defencer, resultSuccess, resultFail);
        }
        else
        {
            return (false, 0, resultFail);
        }
    }

    public (bool isHit, int damage, string message) DoDamageIgnoreDodge(ActorBase player, ActorBase enemy, string resultSuccess, string resultFail)
    {
        var damage = Math.Max(1, player.DoDamage(enemy) - enemy.ReduceDamage(player));
        var hp = GameDataManager.Instance._configTableData.Find(_ => _.config_id == "status_hp");
        enemy.playerStat.AddStat((int) hp.GetValueConfigData(), -damage);
        string formatString = string.Format(resultSuccess, damage.ToString());
        return (true, damage, formatString);
    }
}
