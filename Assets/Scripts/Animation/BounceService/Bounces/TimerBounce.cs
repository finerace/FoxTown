using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerBounce : AbstractBounceService
{

    [Space] 
    
    [SerializeField] private float bounceCooldown;

    protected new void Start()
    {
        StartCoroutine(BounceTImer());
        
       base.Start(); 
    }
    
    private IEnumerator BounceTImer()
    {
        var cooldownWait = new WaitForSeconds(bounceCooldown);

        while (true)
        {
            yield return cooldownWait;
            
            Bounce();
        }
    }

}
