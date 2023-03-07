using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingItem : MonoBehaviour
{

    private PlayerMoneyService playerMoneyService;
    private BuildingsUpgradeService buildingsUpgradeService;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject notificationObj;
    
    [Space] 
    
    [SerializeField] private string buildingName;
    [SerializeField] private int buildingLevel;
    [SerializeField] private int maxBuildingLevel;

    [Space] 
    
    [SerializeField] private float[] moneyEarnCooldownPerLevel;
    [SerializeField] private int[] moneyEarnCountPerLevel;

    [Space] 
    
    [SerializeField] private Sprite[] buildingSpritePerLevel;
    
    [Space] 
    
    [SerializeField] private int[] upgradePricePerLevel;

    public string BuildingName => buildingName;

    public int BuildingLevel => buildingLevel;

    public int MaxBuildingBuildingLevel => maxBuildingLevel;
    
    public float[] MoneyEarnCooldownPerLevel => moneyEarnCooldownPerLevel;

    public int[] MoneyEarnCountPerLevel => moneyEarnCountPerLevel;

    public int[] UpgradePricePerLevel => upgradePricePerLevel;

    private event Action<int> onBuildingUpgrade;
    public event Action<int> OnBuildingUpgrade
    {
        add => onBuildingUpgrade += value ?? throw new NullReferenceException();
        
        remove => onBuildingUpgrade -= value ?? throw new NullReferenceException();
    }

    private void Start()
    {
        SetFields();
        void SetFields()
        {
            playerMoneyService = PlayerMoneyService.instance;
            buildingsUpgradeService = BuildingsUpgradeService.instance;
        }

        StartCoroutine(MoneyEarnCycle());

        playerMoneyService.OnMoneyCountChange += UpgradeNotificationCheck;

        spriteRenderer.sprite = buildingSpritePerLevel[buildingLevel];
    }

    private IEnumerator MoneyEarnCycle()
    {
        YieldInstruction idleWaitTime = new WaitForSeconds(2);

        while (true)
        {
            if (moneyEarnCooldownPerLevel[buildingLevel] <= 0)
            {
                yield return idleWaitTime;
                continue;
            }
            
            yield return new WaitForSeconds(moneyEarnCooldownPerLevel[buildingLevel]);

            playerMoneyService.MoneyCount += moneyEarnCountPerLevel[buildingLevel];
        }
    }

    private void OnMouseDown()
    {
        buildingsUpgradeService.OpenUpgradePanel(this);
    }

    public void UpgradeLevel()
    {
        if(buildingLevel >= maxBuildingLevel)
            throw new Exception("ДОСТИГНУТ МАКСИМАЛЬНЫЙ УРОВЕНЬ, УЛУЧШЕНИЕ НЕВОЗМОЖНО!");
        
        buildingLevel++;
        
        spriteRenderer.sprite = buildingSpritePerLevel[buildingLevel];
        onBuildingUpgrade?.Invoke(buildingLevel);
    }

    private void UpgradeNotificationCheck(int moneyCount)
    {
        if(buildingLevel >= maxBuildingLevel)
        {
            notificationObj.SetActive(false);
            return;
        }

        var isBuildingCanUpgrade = moneyCount >= upgradePricePerLevel[buildingLevel];
        notificationObj.SetActive(isBuildingCanUpgrade);  
    }
}
