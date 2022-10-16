using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider slider;

    public void UpdateVolume(){
        float volume = slider.value/15f;
        audioSource.volume = volume;
        DataManager.instance.volume = volume;

    }

    public void UpdateVolume(float volume){
        slider.value = volume * 15;
        audioSource.volume = volume;
        DataManager.instance.volume = volume;

    }
}
