using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//CLASS DESCRIPTION
//Contians methods to manage the volume slider in the main menu and ranch screens
public class VolumeSlider : MonoBehaviour
{
    //Audio source of this scene
    public AudioSource audioSource;
    //Slider attached to this object
    public Slider slider;

    private const float MAX_VOLUME = DataManager.MAX_VOLUME;

    //sets the volume when the slider is updated(i.e. interacted with)
    public void UpdateVolume(){
        float volume = slider.value/MAX_VOLUME;
        audioSource.volume = volume;
        DataManager.instance.volume = volume;

    }

    //sets the volume to a specified volume
    public void UpdateVolume(float volume){
        slider.value = volume * MAX_VOLUME;
        audioSource.volume = volume;
        DataManager.instance.volume = volume;

    }
}
