using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//CLASS DESCRIPTION
//Contians methods to manage the volume slider in the main menu and ranch screens
public class VolumeSlider : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider slider;

    //sets the volume when the slider is updated(i.e. interacted with)
    public void UpdateVolume(){
        float volume = slider.value/15f;
        audioSource.volume = volume;
        DataManager.instance.volume = volume;

    }

    //sets the volume to a specified volume
    public void UpdateVolume(float volume){
        slider.value = volume * 15;
        audioSource.volume = volume;
        DataManager.instance.volume = volume;

    }
}
