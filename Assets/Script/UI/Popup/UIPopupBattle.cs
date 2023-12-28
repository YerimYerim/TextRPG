using System.Collections.Generic;
using Script.DataClass;
using Script.Manager;
using Script.UI;
using Script.UI.Story;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIPopupBattle : UIBase
{
    enum TYPE
    {
        Warning,
        Seletion,
        SelectionSpecial,
        Result
    }
    [SerializeField] private UIBattleImagePanel _monsterImagePanel;
    [SerializeField] private DTScrollView _scrollView;
    [SerializeField] private UIStoryImagePanel _imagePanel;
    [SerializeField] private UIStoryButtonPanel _buttonsPanel;
    [SerializeField] private UIStroyTextPanel _textPanel;

    
    private MonsterTableData _monsterData;
    private ActorBase _actorBase = new();
    private int _curPhaze = 0;
    private List<LTDescr> _leantweenList = new();
    private List<PlayerActionGroup> _playerAction = new();
    private List<MonsterActionGroup> _actionGroup = new();
    private List<BattleAction> _battleActions = new();
    private int _curActionIndex; 
    private float _tweenTime  = 0.1f;
    private int _endScenarioID;
    private UIBase _parents;
    private void Awake()
    {
        _scrollView.InitScrollView(OnUpdateScrollView, _imagePanel.GameObject(), _buttonsPanel.gameObject, _textPanel.gameObject );
        _scrollView.MakeList(0);
    }

    public void InitMonsterData(int monsterID , int endScenarioData, UIBase parents)
    {
        _monsterData = GameDataManager.Instance._monsterTableData.Find(_ => _.monster_id == monsterID);
        _endScenarioID = endScenarioData;
        _parents = parents;
        _actorBase = new ActorBase
        {
            playerStat = new Stat()
        };
        
        int damageKey = (int)GameDataManager.Instance.GetValueConfigData("status_damage_factor");
        int reduceDefKey = (int)GameDataManager.Instance.GetValueConfigData("status_reduce_defense_factor");
        int hpKey = (int)GameDataManager.Instance.GetValueConfigData("status_hp");
        int defKey = (int)GameDataManager.Instance.GetValueConfigData("status_defense_factor");
        int dodgeKey = (int)GameDataManager.Instance.GetValueConfigData("status_dodge_factor");
        int hitKey = (int)GameDataManager.Instance.GetValueConfigData("status_hit_factor");
        
        _actorBase.playerStat.AddStat(damageKey, _monsterData.status_damage_factor ?? 0);
        _actorBase.playerStat.AddStat(reduceDefKey, _monsterData.status_reduce_defense_factor ?? 0);
        _actorBase.playerStat.AddStat(hpKey, _monsterData.status_hp ?? 0);
        _actorBase.playerStat.AddStat(defKey, _monsterData.status_defense_factor ?? 0);
        _actorBase.playerStat.AddStat(dodgeKey, _monsterData.status_dodge_factor ?? 0);
        _actorBase.playerStat.AddStat(hitKey, _monsterData.status_hit_factor ?? 0);
        
        _monsterImagePanel.SetUI(_monsterData, _monsterData.status_hp ?? 0 );
        _curPhaze = 0;
        _actionGroup.Clear();
        _actionGroup.AddRange(GameDataManager.Instance._monsterActionTableData.FindAll(_ => _.action_group_id == _monsterData.action_group[_curPhaze]));
        
        _playerAction.Clear();
        _playerAction.AddRange(GameDataManager.Instance._playerActionTableData);
        
        GamePlayerManager.Instance.AddMetMonsterCount(_monsterData?.monster_id ?? 0, 1);
        AddBattleActions();
        SetUI();
    }

    GameObject OnUpdateScrollView(int index)
    {
        switch (_battleActions[index].Type)
        {
            case TYPE.Warning:
            {
                var item = _scrollView.GetItem( _textPanel.GameObject());
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                textPanel.SetText(_battleActions[index].MonsterActionTable.warning_text );
                _leantweenList.Add(LeanTween.alphaCanvas( textPanel._canvas, 1, _tweenTime).setDelay(index).setEase(LeanTweenType.animationCurve).setLoopOnce());
                return textPanel.gameObject;
            }            
            case TYPE.Result:
            {
                var item = _scrollView.GetItem( _textPanel.GameObject());
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                textPanel.SetText(_battleActions[index].Message);
                _leantweenList.Add(LeanTween.alphaCanvas( textPanel._canvas, 1, _tweenTime).setDelay(index).setEase(LeanTweenType.animationCurve).setLoopOnce());
                return textPanel.gameObject;
            }
            case TYPE.Seletion:
            {
                var buttonItem = _scrollView.GetItem(_buttonsPanel.GameObject());
                var buttonsPanel = buttonItem.GetComponent<UIStoryButtonPanel>();
                buttonsPanel.SetButton(_battleActions[index].PlayerActionGroup.button_text, ()=>OnClickButton(index));
                _leantweenList.Add(LeanTween.alphaCanvas( buttonsPanel._canvas, 1, _tweenTime).setDelay(index).setEase(LeanTweenType.animationCurve).setLoopOnce());
                return buttonsPanel.gameObject;
            }
            case TYPE.SelectionSpecial:
            {
                var buttonItem = _scrollView.GetItem(_buttonsPanel.GameObject());
                var buttonsPanel = buttonItem.GetComponent<UIStoryButtonPanel>();
                buttonsPanel.SetButton(_battleActions[index].MonsterActionTable.additional_selection, ()=>OnClickButton(index));
                _leantweenList.Add(LeanTween.alphaCanvas( buttonsPanel._canvas, 1, _tweenTime).setDelay(index).setEase(LeanTweenType.animationCurve).setLoopOnce());
                return buttonsPanel.gameObject;
            }
            default:
                return null;
        }
    }

    void SetUI()
    {
        int hpKey = (int)GameDataManager.Instance.GetValueConfigData("status_hp");
        _monsterImagePanel.SetUI(_monsterData, _actorBase?.playerStat?.GetStat(hpKey) ?? 0);
        _scrollView.AddList(_battleActions.Count);
        _scrollView.MoveScrollEndVertical();
    }

    void AddBattleActions()
    {
        _curActionIndex = Random.Range(0, _actionGroup.Count - 1);
        
        _battleActions.Add(new BattleAction()
        {
            Type =  TYPE.Warning,
            MonsterActionTable = _actionGroup[_curActionIndex],
        });
        
        foreach (var actions in _playerAction)
        {
            if(actions.default_action == true)
            {
                _battleActions.Add(new BattleAction()
                {
                    Type = TYPE.Seletion,
                    PlayerActionGroup = actions,
                });
            }
        }
        
        if( _actionGroup[_curActionIndex]?.additional_selection?.Equals(string.Empty) == false)
        {
            var occurCondition = _actionGroup[_curActionIndex].additional_selection_condition.to_OccurCondition_enum();
            bool isCanOccur = true;
            switch (occurCondition)
            {
                case OccurCondition.OCCUR_CONDITION_OWN_ITEM:
                    isCanOccur = GameItemManager.Instance.GetItem( _actionGroup[_curActionIndex].additional_selection_value[0]) >  _actionGroup[_curActionIndex].additional_selection_value[1];
                    break;
                case OccurCondition.OCCUR_CONDITION_NONE:
                    break;
                case OccurCondition.OCCUR_CONDITION_NOT_ENOUGH_OWN_ITEM:
                    isCanOccur = GameItemManager.Instance.GetItem( _actionGroup[_curActionIndex].additional_selection_value[0]) <  _actionGroup[_curActionIndex].additional_selection_value[1];
                    break;
                case OccurCondition.OCCUR_CONDITION_STATUS_HIGH:
                    isCanOccur = GamePlayerManager.Instance.myActor.playerStat.GetStat( _actionGroup[_curActionIndex].additional_selection_value[0]) >  _actionGroup[_curActionIndex].additional_selection_value[1];
                    break;
                case OccurCondition.OCCUR_CONDITION_STATUS_LOW:
                    isCanOccur = GamePlayerManager.Instance.myActor.playerStat.GetStat( _actionGroup[_curActionIndex].additional_selection_value[0]) <  _actionGroup[_curActionIndex].additional_selection_value[1];
                    break;
                case OccurCondition.OCCUR_CONDITION_PAGE_VIEWED:
                    break;
            }
            
            if(isCanOccur)
            {
                _battleActions.Add(new BattleAction()
                {
                    Type = TYPE.SelectionSpecial,
                    MonsterActionTable = _actionGroup[_curActionIndex],
                });
            }
        }
        
    }

    void AddResultActions(bool isHit, string message)
    {
        if (message.Equals(string.Empty) == false)
        {
            _battleActions.Add(new BattleAction()
            {
                Type = TYPE.Result,
                MonsterActionTable = _actionGroup[_curActionIndex],
                Message = message
            });
        }
    }

    private void CheckPhaze()
    {
        int hpKey = (int)GameDataManager.Instance.GetValueConfigData("status_hp");
        var hp = _actorBase.playerStat.GetStat(hpKey);
        
        if ( _curPhaze < _monsterData.phaze_hp_condition.Count && hp <= _monsterData.phaze_hp_condition[_curPhaze])
        {
            ++_curPhaze;
            _actionGroup.Clear();
            _actionGroup.AddRange(GameDataManager.Instance._monsterActionTableData.FindAll(_ => _.action_group_id == _monsterData.action_group[_curPhaze]));
        }
    }

    private bool CheckMonsterWin()
    {
        int hpKey = (int)GameDataManager.Instance.GetValueConfigData("status_hp");
        var playerhp = GamePlayerManager.Instance.myActor.playerStat.GetStat(hpKey);
        
        if (playerhp <= 0)
        {
            GamePageManager.Instance.EnqueueCurPageData(_endScenarioID);
            GamePlayerManager.Instance.DeadCount += 1;
            _parents.Show();
            
            Hide();
            return true;
        }
        return false;
    }   
    private bool CheckPlayerWin()
    {
        int hpKey = (int)GameDataManager.Instance.GetValueConfigData("status_hp");
        var monsterHp = _actorBase.playerStat.GetStat(hpKey);
        if (monsterHp <= 0)
        {
            GamePageManager.Instance.EnqueueCurPageData(_endScenarioID);
            GamePlayerManager.Instance.AddKillMonsterCount(_monsterData.monster_id ?? 0, 1);
            _parents.Show();
            Hide();
            return true;
        }

        return false;
    }

    public override void Hide()
    {
        for (int i = 0; i < _leantweenList.Count; ++i)
        {
            LeanTween.cancel(_leantweenList[i].id);
        }
        base.Hide();
    }

    void OnClickButton(int index)
    {
        var buttonItems = _scrollView.GetItemsByComponent<UIStoryButtonPanel>();
        foreach (var button in buttonItems)
        {
            button.SetActive(false);
        }

        var playerAction = _battleActions[index].PlayerActionGroup;
        var monsterActionGroup = _actionGroup[_curActionIndex];
        _battleActions.Clear();
        (bool isHit, int damage, string message) result;
                
        
        if(playerAction != null)
        {
            switch (playerAction.action_type)
            {
                case "normal_attack":
                    if (monsterActionGroup.action_type.Equals("dodge"))
                    {
                        result = GameBattleManager.Instance.DoBattle(GamePlayerManager.Instance.myActor, _actorBase, playerAction.result_text, monsterActionGroup.result_text );
                        AddResultActions(result.isHit, result.message);
                        if (CheckPlayerWin())
                        {
                            return;
                        };
                    }
                    else
                    {
                        result = GameBattleManager.Instance.DoDamageIgnoreDodge(GamePlayerManager.Instance.myActor, _actorBase, playerAction.result_text, string.Empty );
                        AddResultActions(result.isHit, result.message);
                        if (CheckPlayerWin())
                        {
                            return;
                        };
                        result = GameBattleManager.Instance.DoDamageIgnoreDodge(_actorBase, GamePlayerManager.Instance.myActor, monsterActionGroup.result_text, string.Empty);
                        AddResultActions(result.isHit, result.message);
                    }
                    
                    if (CheckMonsterWin())
                    {
                        return;
                    };
                    break;
                case "special_attack":
                    break;
                case "dodge":
                    result = GameBattleManager.Instance.DoBattle(_actorBase, GamePlayerManager.Instance.myActor, monsterActionGroup.result_text, playerAction.result_text);
                    AddResultActions(result.isHit, result.message);
                    if (CheckMonsterWin())
                    {
                        return;
                    };
                    break;
                case "action_cancel":
                    break;
            }
        }

        CheckPhaze();
        AddBattleActions();
        SetUI();
    }
    
    private class BattleAction
    {
        public TYPE Type;
        public MonsterActionGroup MonsterActionTable;
        public PlayerActionGroup PlayerActionGroup;
        public string Message;
    }
}