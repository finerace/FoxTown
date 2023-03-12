using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SmoothEnablePanelAnimationService : AbstractSmoothPanelService
{
    private void OnEnable()
    {
        
        
        SetAllNewTransparency(0);
        SetNewTargetTransparency(1);
    }
}
