using System;
using TMPro;
using UnityEngine;

public class PlayerMoneyLabel : MonoBehaviour
{

    private PlayerMoneyService playerMoneyService;
    [SerializeField] private TMP_Text moneyCountLabel;
    
    [Space]
    
    [SerializeField] private string moneyPreText;

    private void Start()
    {
        playerMoneyService = PlayerMoneyService.instance;
        
        playerMoneyService.OnMoneyCountChange += UpdateMoneyLabel;
    }

    private void UpdateMoneyLabel(int moneyCount)
    {
        moneyCountLabel.text = $"{moneyPreText}{moneyCount}";
    }
    
}
