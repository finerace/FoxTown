using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MoneyEarnCircleBounce : AbstractBounceService
{

    [SerializeField] private BuildingItem buildingItem;

    [Space] 
    
    [SerializeField] private float idleBounceHight;
    [SerializeField] private float idleBounceWidth;
    private Coroutine currentIdleCoroutine;
    
    protected new void Start()
    {
        base.Start();

        buildingItem.OnMoneyEarn += Bounce;
        buildingItem.OnMoneyEarn += SetNewIdleCoroutine;
    }

    private void SetNewIdleCoroutine()
    {
        if (currentIdleCoroutine != null)
        {
            StopCoroutine(currentIdleCoroutine);
        }

        currentIdleCoroutine =
            StartCoroutine(IdleBounce());
    }
    
    private IEnumerator IdleBounce()
    {
        var waitCooldown = new WaitForSeconds(5);

        while (true)
        {
            yield return waitCooldown;
            
            if(buildingItem.IsMoneyEarnReady)
                ManualBounce(idleBounceHight,idleBounceWidth);
            else
                yield break;
        }
    }
    
}
