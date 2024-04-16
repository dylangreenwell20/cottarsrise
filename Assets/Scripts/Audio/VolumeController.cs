using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider musicSlider, sfxSlider; //music and sfx slider

    private void Start()
    {
        UpdateSliderPositions();
    }

    public void MusicVolume() //change music volume
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
    }

    public void SFXVolume() //change sfx volume
    {
        AudioManager.Instance.SFXVolume(sfxSlider.value);
    }

    public void UpdateSliderPositions() //update slider positions to current volume levels
    {
        musicSlider.value = AudioManager.Instance.musicSource.volume;
        sfxSlider.value = AudioManager.Instance.sfxSource.volume;
    }
}
