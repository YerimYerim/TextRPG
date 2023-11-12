using Script.UI;
using Script.UI.Story;
using Unity.VisualScripting;
using UnityEngine;

public class UIPopupBattle : UIBase
{
    [SerializeField] private UIBattleImagePanel _monsterImagePanel;
    [SerializeField] private DTScrollView _scrollView;
    [SerializeField] private UIStoryImagePanel _imagePanel;
    [SerializeField] private UIStoryButtonPanel _buttonsPanel;
    [SerializeField] private UIStroyTextPanel _textPanel;
    private MonsterTableData _monsterData;
    private ActorBase actorBase = new();
    private void Awake()
    {
        _scrollView.InitScrollView(OnUpdateScrollView, _imagePanel.GameObject(), _buttonsPanel.gameObject, _textPanel.gameObject );
        _scrollView.MakeList(0);
    }

    public void InitMonsterData(int monsterID)
    {
        _monsterData = GameDataManager.Instance._monsterTableData.Find(_ => _.monster_id == monsterID);
    
        actorBase = new ActorBase
        {
            playerStat = new Stat()
        };
        
        int damageKey = (int)GameDataManager.Instance.GetValueConfigData("status_damage_factor");
        int reduceDefKey = (int)GameDataManager.Instance.GetValueConfigData("status_reduce_defense_factor");
        int hpKey = (int)GameDataManager.Instance.GetValueConfigData("status_hp");
        int defKey = (int)GameDataManager.Instance.GetValueConfigData("status_defense_factor");
        int dodgeKey = (int)GameDataManager.Instance.GetValueConfigData("status_dodge_factor");
        int hitKey = (int)GameDataManager.Instance.GetValueConfigData("status_hit_factor");
        
        actorBase.playerStat.AddStat(damageKey, _monsterData.status_damage_factor ?? 0);
        actorBase.playerStat.AddStat(reduceDefKey, _monsterData.status_reduce_defense_factor ?? 0);
        actorBase.playerStat.AddStat(hpKey, _monsterData.status_hp ?? 0);
        actorBase.playerStat.AddStat(defKey, _monsterData.status_defense_factor ?? 0);
        actorBase.playerStat.AddStat(dodgeKey, _monsterData.status_dodge_factor ?? 0);
        actorBase.playerStat.AddStat(hitKey, _monsterData.status_hit_factor ?? 0);
        
        _monsterImagePanel.SetUI(_monsterData, _monsterData.status_hp ?? 0 );

    }
    GameObject OnUpdateScrollView(int index)
    {
        var item = _scrollView.GetItem( _textPanel.GameObject());
        var textPanel = item.GetComponent<UIStroyTextPanel>();
        var buttonsPanel = item.GetComponent<UIStoryButtonPanel>();
        return null;
    }

    void OnClickButton()
    {
        
    }
}