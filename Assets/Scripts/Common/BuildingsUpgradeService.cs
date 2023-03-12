using System;
using UnityEngine;

public class BuildingsUpgradeService : MonoBehaviour
{
    public static BuildingsUpgradeService instance;
    private PlayerMoneyService playerMoneyService;
    [SerializeField] private BuildingsUpgradePanel upgradePanel;
    
    [Space]
        
    private BuildingItem selectedBuildingItem;
    
    private event Action onDeselectBuilding;
    public event Action OnDeselectBuilding
    {
        add => onDeselectBuilding += value ?? throw new NullReferenceException();
        
        remove => onDeselectBuilding -= value ?? throw new NullReferenceException();
    }

    private AudioPoolService audioPoolService;
    
    [SerializeField] private AudioCastData openPanelSound;
    [SerializeField] private AudioCastData closePanelSound;
    
    [Space]
    
    [SerializeField] private AudioCastData moneyEarnSound;
    
    [Space]
    
    [SerializeField] private AudioCastData upgradeSound;
    
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioPoolService = AudioPoolService.audioPoolServiceInstance;
        playerMoneyService = PlayerMoneyService.instance;
    }

    public void OpenUpgradePanel(BuildingItem buildingItem)
    {
        if(buildingItem.Equals(selectedBuildingItem))
            return;

        selectedBuildingItem = buildingItem;
        
        upgradePanel.SetUpgradePanelActive(true);
        
        if(buildingItem.isLeftMenu)
            upgradePanel.PanelT.position = buildingItem.BuildingT.position - (-Vector3.left * 8);
        else
            upgradePanel.PanelT.position = buildingItem.BuildingT.position;

        upgradePanel.PanelT.position += new Vector3(buildingItem.x, buildingItem.y, 0);
        
        upgradePanel.UpdateLabels(selectedBuildingItem);
        
        onDeselectBuilding?.Invoke();

        audioPoolService.CastAudio(openPanelSound);
    }

    public void CloseUpgradePanel()
    {
        selectedBuildingItem = null;
        upgradePanel.SetUpgradePanelActive(false);
        
        onDeselectBuilding?.Invoke();

        audioPoolService.CastAudio(closePanelSound);
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

        audioPoolService.CastAudio(upgradeSound);
        
        CloseUpgradePanel();
    }

    public void CastCollectMoneySound()
    {
        audioPoolService.CastAudio(moneyEarnSound);
    }

}
