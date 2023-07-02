using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public Slider bgmSlider;


    private void Start()
    {
        bgmSlider.value = 1;
    }

    public void SetBGM()
    {
        AudioListener.volume = bgmSlider.value;


    }
}
