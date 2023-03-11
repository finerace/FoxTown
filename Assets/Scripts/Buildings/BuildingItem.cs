using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingItem : MonoBehaviour
{

    private PlayerMoneyService playerMoneyService;
    private BuildingsUpgradeService buildingsUpgradeService;
    private Transform buildingT;
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject notificationObj;
    
    [Space] 
    
    [SerializeField] private string buildingName;
    [SerializeField] private int buildingLevel;
    [SerializeField] private int maxBuildingLevel;

    [Space] 
    
    private float moneyEarnTimer;
    [SerializeField] private bool isMoneyEarnReady;
    
    [SerializeField] private float[] moneyEarnCooldownPerLevel;
    [SerializeField] private int[] moneyEarnCountPerLevel;

    [Space] 
    
    [SerializeField] private Sprite[] buildingSpritePerLevel;
    
    [Space] 
    
    [SerializeField] private int[] upgradePricePerLevel;

    public string BuildingName => buildingName;

    public int BuildingLevel => buildingLevel;

    public int MaxBuildingBuildingLevel => maxBuildingLevel;

    public float MoneyEarnTimer => moneyEarnTimer;

    public bool IsMoneyEarnReady => isMoneyEarnReady;

    public float[] MoneyEarnCooldownPerLevel => moneyEarnCooldownPerLevel;

    public int[] MoneyEarnCountPerLevel => moneyEarnCountPerLevel;

    public int[] UpgradePricePerLevel => upgradePricePerLevel;

    public Transform BuildingT => buildingT;
    
    private event Action<int> onBuildingUpgrade;
    public event Action<int> OnBuildingUpgrade
    {
        add => onBuildingUpgrade += value ?? throw new NullReferenceException();
        
        remove => onBuildingUpgrade -= value ?? throw new NullReferenceException();
    }
    
    private event Action onMoneyEarn;
    public event Action OnMoneyEarn
    {
        add => onMoneyEarn += value ?? throw new NullReferenceException();
        
        remove => onMoneyEarn -= value ?? throw new NullReferenceException();
    }

    private void Awake()
    {
        buildingT = transform;
    }

    private void Start()
    {
        SetFields();
        void SetFields()
        {
            playerMoneyService = PlayerMoneyService.instance;
            buildingsUpgradeService = BuildingsUpgradeService.instance;
        }
        
        playerMoneyService.OnMoneyCountChange += UpgradeNotificationCheck;

        spriteRenderer.sprite = buildingSpritePerLevel[buildingLevel];
    }

    private void Update()
    {
        if (!isMoneyEarnReady)
        {
            moneyEarnTimer += Time.deltaTime;

            if (moneyEarnTimer >= moneyEarnCooldownPerLevel[buildingLevel])
            {
                isMoneyEarnReady = true;
                onMoneyEarn?.Invoke();
            } 
        }
    }

    private void OnMouseDown()
    {
        buildingsUpgradeService.OpenUpgradePanel(this);
    }

    public void CollectMoney()
    {
        if (!isMoneyEarnReady)
            return;

        playerMoneyService.MoneyCount += moneyEarnCountPerLevel[buildingLevel];

        isMoneyEarnReady = false;
        moneyEarnTimer = 0;

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
