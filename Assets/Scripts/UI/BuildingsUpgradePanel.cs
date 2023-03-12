using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingsUpgradePanel : MonoBehaviour
{
    [SerializeField] private Transform panelT;
    [SerializeField] private GameObject buildingsUpgradePanelObj;

    [Space]
    
    [SerializeField] private TMP_Text buildingNameLabel;
    
    [SerializeField] private TMP_Text buildingMoneyEarningLabel;
    [SerializeField] private TMP_Text moneyEarnCooldown;

    
    [SerializeField] private TMP_Text buildingUpgradePriceLabel;

    public Transform PanelT => panelT;
    
    private void Start()
    {
        buildingsUpgradePanelObj.SetActive(false);
    }

    public void UpdateLabels(BuildingItem buildingItem)
    {
        var currentBuildingLevel = buildingItem.BuildingLevel; 
        
        buildingNameLabel.text = $"{buildingItem.BuildingName}";
        
        buildingMoneyEarningLabel.text = $"{buildingItem.MoneyEarnCountPerLevel[currentBuildingLevel]}";

        moneyEarnCooldown.text = $"{buildingItem.MoneyEarnCooldownPerLevel[currentBuildingLevel]} сек." ;
        
        if (!(buildingItem.BuildingLevel >= buildingItem.MaxBuildingBuildingLevel))
            buildingUpgradePriceLabel.text = $"{buildingItem.UpgradePricePerLevel[currentBuildingLevel]}";
        else
            buildingUpgradePriceLabel.text = "Макс ур.";
    }

    public void SetUpgradePanelActive(bool state)
    {
        buildingsUpgradePanelObj.SetActive(state);
    }
    
}
