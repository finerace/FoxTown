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
    [SerializeField] private TMP_Text buildingLevelLabel;
    
    [SerializeField] private TMP_Text buildingMoneyEarningLabel;
    [SerializeField] private TMP_Text buildingEarnCooldownPriceLabel;

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
        buildingLevelLabel.text = $"Уровень {buildingItem.BuildingLevel}";
        
        buildingMoneyEarningLabel.text = $"{buildingItem.MoneyEarnCountPerLevel[currentBuildingLevel]}";
        buildingEarnCooldownPriceLabel.text = $"{buildingItem.MoneyEarnCooldownPerLevel[currentBuildingLevel]}s";

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
