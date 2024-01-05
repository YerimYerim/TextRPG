using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI attackStat;
    [SerializeField] private TextMeshProUGUI hpStat;
    [SerializeField] private Image hpBar;


    void UpdateStat()
    {
        int hpKey = (int)GameDataManager.Instance.GetValueConfigData("status_hp");
        //hpBar.fillAmount = 
    }
}
