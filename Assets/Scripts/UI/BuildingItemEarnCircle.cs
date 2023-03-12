using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingItemEarnCircle : MonoBehaviour
{

    [SerializeField] private BuildingItem buildingItem;
    [SerializeField] private Image circleImage;

    [Space] 
    
    [SerializeField] private GameObject earnedMoneyBubble;
    [SerializeField] private TMP_Text earnedMoneyLabel;

    [Space] 
    
    [SerializeField] private ParticleSystem earnMoneyParticle;

    private void Start()
    {
        buildingItem.OnMoneyEarn += ActivateParticle;
    }

    private void Update()
    {
        IndicatorWork();
    }

    private void IndicatorWork()
    {
        if (buildingItem.IsMoneyEarnReady)
        {
            circleImage.fillAmount = 1;
            
            if(!circleImage.raycastTarget)
                circleImage.raycastTarget = true;
            
            if(earnedMoneyBubble.activeSelf)
                return;
            
            SetEarnMoneyBubble();
            
            return;
        }

        circleImage.fillAmount = buildingItem.MoneyEarnTimer /
                                 buildingItem.MoneyEarnCooldownPerLevel[buildingItem.BuildingLevel];

        if (circleImage.raycastTarget)
            circleImage.raycastTarget = false;
            
        if(earnedMoneyBubble.activeSelf)
            earnedMoneyBubble.SetActive(false);
    }

    private void SetEarnMoneyBubble()
    {
        StartCoroutine(BubbleDelayActivate());

        earnedMoneyLabel.text = $"{buildingItem.MoneyEarnCountPerLevel[buildingItem.BuildingLevel]}";
    }

    private IEnumerator BubbleDelayActivate()
    {
        yield return new WaitForSeconds(0.3f);
        
        if(buildingItem.IsMoneyEarnReady)
            earnedMoneyBubble.SetActive(true);
    }

    private void ActivateParticle()
    {
        if(earnMoneyParticle != null)
            earnMoneyParticle.Play();
    }
    
}
