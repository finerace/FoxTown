using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractSmoothPanelService : MonoBehaviour
{

    [SerializeField] private Image[] images;
    [SerializeField] private SpriteRenderer[] sprites;
    [SerializeField] private TMP_Text[] labels;

    [Space] 
    
    [SerializeField] private float animationSpeed;
    [SerializeField] private float targetTransparency = 1;
    private bool animationIsEnded;
    
    private void Update()
    {
        if(animationIsEnded)
            return;
        
        var timeStep = Time.deltaTime * animationSpeed;
        
        ChangeImages();
        ChangeLabels();
        ChangeSprites();

        CheckAnimationEnd();
        
        void ChangeImages()
        {
            foreach (var image in images)
            {
                var newColor = image.color;
                newColor.a = Mathf.Lerp(newColor.a,targetTransparency,timeStep);

                image.color = newColor;
            }
        }
        
        void ChangeLabels()
        {
            foreach (var label in labels)
            {
                var newColor = label.color;
                newColor.a = Mathf.Lerp(newColor.a,targetTransparency,timeStep);

                label.color = newColor;
            }
        }

        void ChangeSprites()
        {
            foreach (var sprite in sprites)
            {
                var newColor = sprite.color;
                newColor.a = Mathf.Lerp(newColor.a,targetTransparency,timeStep);

                sprite.color = newColor;
            }
        }

        void CheckAnimationEnd()
        {
            if (images.Length > 0)
                CheckAlpha(images[0].color.a);

            if (labels.Length > 0) 
                CheckAlpha(labels[0].color.a);
                
            if(sprites.Length > 0)
                CheckAlpha(sprites[0].color.a);
            
            void CheckAlpha(float alpha)
            {
                if(animationIsEnded)
                    return;
                
                if (alpha >= targetTransparency - 0.01f)
                    animationIsEnded = true;
            }
        }
    }

    protected void SetNewTargetTransparency(float newTargetTransparency)
    {
        animationIsEnded = false;
        
        if (newTargetTransparency < 0 || newTargetTransparency > 1)
            throw new Exception("ЦЕЛЕВОЕ ЗНАЧЕНИЕ ПРОЗРАЧНОСТИ НЕ МОЖЕТ БЫТЬ МЕНЬШЕ 0 ИЛИ БОЛЬШЕ 1 !!!1");

        targetTransparency = newTargetTransparency;

    }

    protected void SetAllNewTransparency(float newTransparency)
    {
        if (newTransparency < 0 || newTransparency > 1)
            throw new Exception("ЗНАЧЕНИЕ ПРОЗРАЧНОСТИ НЕ МОЖЕТ БЫТЬ МЕНЬШЕ 0 ИЛИ БОЛЬШЕ 1 !!!1");
        
        ChangeImages();
        ChangeLabels();
        ChangeSprites();
        
        void ChangeImages()
        {
            foreach (var image in images)
            {
                var newColor = image.color;
                newColor.a = newTransparency;

                image.color = newColor;
            }
        }
        
        void ChangeLabels()
        {
            foreach (var label in labels)
            {
                var newColor = label.color;
                newColor.a = newTransparency;

                label.color = newColor;
            }
        }

        void ChangeSprites()
        {
            foreach (var sprite in sprites)
            {
                var newColor = sprite.color;
                newColor.a = newTransparency;

                sprite.color = newColor;
            }
        }
    }
    
}
