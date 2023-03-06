using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingsUpgradePanel : MonoBehaviour
{
    [SerializeField] private GameObject buildingsUpgradePanelObj;

    [Space]
    
    [SerializeField] private TMP_Text buildingNameLabel;
    [SerializeField] private TMP_Text buildingLevelLabel;
    
    [SerializeField] private TMP_Text buildingMoneyEarningLabel;
    [SerializeField] private TMP_Text buildingUpgradePriceLabel;

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


        if (!(buildingItem.BuildingLevel >= buildingItem.MaxBuildingBuildingLevel))
            buildingUpgradePriceLabel.text = $"{buildingItem.UpgradePricePerLevel[currentBuildingLevel]}";
        else
            buildingUpgradePriceLabel.text = "Всё куплено!";
    }

    public void SetUpgradePanelActive(bool state)
    {
        buildingsUpgradePanelObj.SetActive(state);
    }
    
}
