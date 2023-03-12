using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSliderChanger : MonoBehaviour
{
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Slider volumeSlider;
    
    [SerializeField] private string audioAttribute;

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(SetSound);
        
        SetSound(volumeSlider.value);
    }

    private void SetSound(float num)
    {
        if (num <= 0.01f)
            mainMixer.SetFloat(audioAttribute, -80);
                
        if (num >= 0.5f)
        {
            num -= 0.5f;
            num *= 2f;

            num *= 10;
        }
        else
        {
            num = 0.5f - num;
            num *= 2f;

            num *= -50;
        }

        mainMixer.SetFloat(audioAttribute, num);
    }
}
