using System;
using UnityEngine;

public abstract class AbstractBounceService : MonoBehaviour
{
    [SerializeField] private Transform meshT;

    [Space] 
    
    [SerializeField] private float animationSpeed;

    [Space]
    
    [SerializeField] private float hightTargetValueMultiplier;
    private float defaultHight;
    private float currentHightTargetValue;
    
    
    [SerializeField] private float widthTargetValueMultiplier; 
    private float defaultWidth;
    private float currentWidthTargetValue;

    protected void Start()
    {
        SetFields();
        void SetFields()
        {
            if (meshT == null)
                meshT = transform;

            var meshLocalScale = meshT.localScale;
            defaultWidth = meshLocalScale.x;
            defaultHight = meshLocalScale.y;

            currentHightTargetValue = 1;
            currentWidthTargetValue = 1;
        }
    }

    protected void Update()
    {
        AnimationProcess();
        void AnimationProcess()
        {
            var shopItemLocalScale = meshT.localScale;
            var timeStep = Time.deltaTime * animationSpeed;
            
            shopItemLocalScale.x = 
                Mathf.Lerp(shopItemLocalScale.x, currentWidthTargetValue * defaultWidth, timeStep);

            shopItemLocalScale.y =
                Mathf.Lerp(shopItemLocalScale.y, currentHightTargetValue * defaultHight, timeStep);

            meshT.localScale = shopItemLocalScale;
            
            currentHightTargetValue = Mathf.Lerp(currentHightTargetValue,1,timeStep);
            currentWidthTargetValue = Mathf.Lerp(currentWidthTargetValue, 1, timeStep);
        }
    }

    public void Bounce()
    {
        var hightChange = 0.1f * hightTargetValueMultiplier;
        var widthChange = 0.1f * widthTargetValueMultiplier;

        currentHightTargetValue += hightChange;
        currentWidthTargetValue += widthChange;
    }

    public void ManualBounce(float hightBounce,float widthBounce)
    {
        var hightChange = 0.1f * hightBounce;
        var widthChange = 0.1f * widthBounce;

        currentHightTargetValue += hightChange;
        currentWidthTargetValue += widthChange;
    }
    
    // private void ChangeHightWidth(float hightChange, float widthChange)
    // {
    //     hightChange *= hightTargetValueMultiplier;
    //     widthChange *= widthTargetValueMultiplier;
    //
    //     currentHightTargetValue += hightChange;
    //     currentWidthTargetValue += widthChange;
    // }

}
