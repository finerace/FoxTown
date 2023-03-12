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
    [SerializeField] private SpriteRenderer outlineSpriteRenderer;
    [SerializeField] private SpriteRenderer buildingBottomPlatform;
    
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
    [SerializeField] private Sprite[] buildingSpritePerLevelSelected;
    [SerializeField] private Sprite[] buildingBottomPlatformPerLevel;

    [Space] 
    
    [SerializeField] private int[] upgradePricePerLevel;

    [Space]
    
    [SerializeField] private GameObject moneyIndicators;
    [SerializeField] private GameObject buyPlace;

    [Space] 
    
    [SerializeField] private GameObject animatedCharacters;
    [SerializeField] private ParticleSystem upgradeParticle;

    [Space] 
    
    private AudioPoolService audioPoolService;
    [SerializeField] private AudioCastData onUpgradeSound;
    
    public string BuildingName => buildingName;

    public int BuildingLevel => buildingLevel;

    public int MaxBuildingBuildingLevel => maxBuildingLevel;

    public float MoneyEarnTimer => moneyEarnTimer;

    public bool IsMoneyEarnReady => isMoneyEarnReady;

    public float[] MoneyEarnCooldownPerLevel => moneyEarnCooldownPerLevel;

    public int[] MoneyEarnCountPerLevel => moneyEarnCountPerLevel;

    public int[] UpgradePricePerLevel => upgradePricePerLevel;

    public Transform BuildingT => buildingT;

    private bool isSelected;
    
    private event Action<int> onBuildingUpgrade;
    public event Action<int> OnBuildingUpgrade
    {
        add => onBuildingUpgrade += value ?? throw new NullReferenceException();
        
        remove => onBuildingUpgrade -= value ?? throw new NullReferenceException();
    }
    
    private event Action onMoneyEarnReady;
    public event Action OnMoneyEarnReady
    {
        add => onMoneyEarnReady += value ?? throw new NullReferenceException();
        
        remove => onMoneyEarnReady -= value ?? throw new NullReferenceException();
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
        
        if(buildingLevel <= 0)
            moneyIndicators.SetActive(false);
        
        if(animatedCharacters != null && buildingLevel <= 0)
            animatedCharacters.SetActive(false);
            
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
        
        audioPoolService = AudioPoolService.audioPoolServiceInstance;
        UpdateSprites();
    }

    private void Update()
    {
        if (!isMoneyEarnReady)
        {
            moneyEarnTimer += Time.deltaTime;

            if (moneyEarnTimer >= moneyEarnCooldownPerLevel[buildingLevel])
            {
                isMoneyEarnReady = true;
                onMoneyEarnReady?.Invoke();
            } 
        }
    }

    private void OnMouseUp()
    {
        buildingsUpgradeService.OpenUpgradePanel(this);
        outlineSpriteRenderer.gameObject.SetActive(true);
        
        buildingsUpgradeService.OnDeselectBuilding += DeselectBuilding;
    }
    
    public void CollectMoney()
    {
        if (!isMoneyEarnReady)
            return;

        playerMoneyService.MoneyCount += moneyEarnCountPerLevel[buildingLevel];

        isMoneyEarnReady = false;
        moneyEarnTimer = 0;
        
        onMoneyEarn?.Invoke();

        buildingsUpgradeService.CastCollectMoneySound();
        
    }
    
    public void UpgradeLevel()
    {
        if(buildingLevel >= maxBuildingLevel)
            throw new Exception("ДОСТИГНУТ МАКСИМАЛЬНЫЙ УРОВЕНЬ, УЛУЧШЕНИЕ НЕВОЗМОЖНО!");
        
        buildingLevel++;
        
        UpdateSprites();
        
        onBuildingUpgrade?.Invoke(buildingLevel);
        
        if(buyPlace != null && buyPlace.activeSelf)
            buyPlace.SetActive(false);

        if(!moneyIndicators.activeSelf)
            moneyIndicators.SetActive(true);
        
        if(animatedCharacters != null && !animatedCharacters.activeSelf)
            animatedCharacters.SetActive(true);

        audioPoolService.CastAudio(onUpgradeSound);
        
        upgradeParticle.Play();
    }

    private void UpgradeNotificationCheck(int moneyCount)
    {
        if(buildingLevel >= maxBuildingLevel || buildingLevel <= 0)
        {
            notificationObj.SetActive(false);
            return;
        }

        var isBuildingCanUpgrade = moneyCount >= upgradePricePerLevel[buildingLevel];
        notificationObj.SetActive(isBuildingCanUpgrade);  
    }

    private void DeselectBuilding()
    {
        outlineSpriteRenderer.gameObject.SetActive(false);
        
        buildingsUpgradeService.OnDeselectBuilding -= DeselectBuilding;
    }

    private void UpdateSprites()
    {
        if (buildingSpritePerLevel[buildingLevel] != null)
        {
            spriteRenderer.color = Color.white;
            spriteRenderer.sprite = buildingSpritePerLevel[buildingLevel];
        }
        else
        {
            spriteRenderer.color = GetClearColor();
            spriteRenderer.sprite = null;
        }
        
        if (buildingSpritePerLevelSelected[buildingLevel] != null)
        {
            outlineSpriteRenderer.color = Color.white;
            outlineSpriteRenderer.sprite = buildingSpritePerLevelSelected[buildingLevel];
        }
        else
        {
            outlineSpriteRenderer.color = GetClearColor();
            outlineSpriteRenderer.sprite = null;
        }        
        
        if (buildingBottomPlatformPerLevel[buildingLevel] != null)
        {
            buildingBottomPlatform.color = Color.white;
            buildingBottomPlatform.sprite = buildingBottomPlatformPerLevel[buildingLevel];
        }
        else
        {
            buildingBottomPlatform.color = GetClearColor();
            buildingBottomPlatform.sprite = null;
        }   
        
        Color GetClearColor()
        {
            return new Color(0, 0, 0, 0);
        }
    }
}
