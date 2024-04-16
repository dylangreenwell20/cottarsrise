using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; //instance of audio manager to access it in other scripts easily

    public Sound[] musicSounds, sfxSounds; //audio arrays
    public AudioSource musicSource, sfxSource, walkSource, runSource; //sources of audio

    public VolumeController volumeController; //update slider positions to volume levels

    private void Awake()
    {
        if(Instance == null) //if no instance exists
        {
            Instance = this; //create instance
            DontDestroyOnLoad(gameObject); //dont delete this game object
        }
        else
        {
            Destroy(gameObject); //destroy game object if instance exists
        }
    }

    private void Start()
    {
        PlayMusic("MainMenuMusic");
    }

    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "MainMenu")
        {
            volumeController = GameObject.Find("MainMenuCanvas").transform.Find("Background").Find("Settings").gameObject.GetComponent<VolumeController>(); //get volume controller
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name); //find music called "name"

        if(s == null) //if music not found
        {
            Debug.Log("Music not found...");
        }
        else
        {
            //Debug.Log("playing music!!");
            musicSource.clip = s.clip; //set music clip
            musicSource.Play(); //play music
        }
    }

    public void PlaySFX(string name, AudioSource sfxLocation) //play sfx at custom audio source location
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name); //find sound effect called "name"

        if (s == null) //if sound effect not found
        {
            Debug.Log("SFX not found...");
        }
        else
        {
            sfxLocation.PlayOneShot(s.clip); //play sfx at custom location
        }
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
        walkSource.volume = volume;
        runSource.volume = volume;
    }
}
