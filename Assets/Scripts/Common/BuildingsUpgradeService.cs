using System;
using UnityEngine;

public class BuildingsUpgradeService : MonoBehaviour
{
    public static BuildingsUpgradeService instance;
    private PlayerMoneyService playerMoneyService;
    [SerializeField] private BuildingsUpgradePanel upgradePanel;
    
    [Space]
        
    private BuildingItem selectedBuildingItem;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerMoneyService = PlayerMoneyService.instance;
    }

    public void OpenUpgradePanel(BuildingItem buildingItem)
    {
        if(buildingItem.Equals(selectedBuildingItem))
            return;

        selectedBuildingItem = buildingItem;
        
        upgradePanel.SetUpgradePanelActive(true);
        upgradePanel.PanelT.position = buildingItem.BuildingT.position;
        
        upgradePanel.UpdateLabels(selectedBuildingItem);
    }

    public void CloseUpgradePanel()
    {
        selectedBuildingItem = null;
        upgradePanel.SetUpgradePanelActive(false);
    }

    public void UpgradeSelectedBuilding()
    {
        if(selectedBuildingItem.BuildingLevel >= selectedBuildingItem.MaxBuildingBuildingLevel)
            return;
        
        var buildingUpgradePrice = selectedBuildingItem.UpgradePricePerLevel[selectedBuildingItem.BuildingLevel];
        var isEnoughMoneyForUpgrade = playerMoneyService.MoneyCount >= buildingUpgradePrice;
        
        if(!isEnoughMoneyForUpgrade)
            return;
        
        selectedBuildingItem.UpgradeLevel();
        upgradePanel.UpdateLabels(selectedBuildingItem);
        
        playerMoneyService.MoneyCount -= buildingUpgradePrice;
    }
    
}
