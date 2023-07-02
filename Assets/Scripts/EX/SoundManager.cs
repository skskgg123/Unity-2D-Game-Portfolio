using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{ 
    public static SoundManager instance;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);          
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject audio = new GameObject(sfxName + "Sound");
        AudioSource audioSource = audio.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(audio, clip.length);

    }

    public void BackgroundSound(AudioClip clip)
    {
        GameObject audio = new GameObject("Sound");
        AudioSource audioSource = audio.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        audioSource.loop = true;
        audioSource.volume = 0.3f;
    }



}
