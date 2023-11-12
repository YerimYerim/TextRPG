using Script.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleImagePanel : MonoBehaviour
{
    [SerializeField] private Image _monsterImage;
    [SerializeField] private Image _hpBar;
    [SerializeField] private TextMeshProUGUI _txtHp;
    [SerializeField] private TextMeshProUGUI _monsterName;

    public void SetUI(MonsterTableData monsterTableData, int curHp)
    {
        _monsterImage.sprite = GameResourceManager.Instance.GetImage(monsterTableData.monter_img);
        if (monsterTableData.status_hp != null)
        {
            _hpBar.fillAmount = curHp / (float) monsterTableData.status_hp;
        }
        _monsterName.text = $"Lv.{monsterTableData.monster_level.ToString()} {monsterTableData.monster_name}";
        _txtHp.text = $"{curHp.ToString()} / {monsterTableData.status_hp.ToString()}";
    }
}
