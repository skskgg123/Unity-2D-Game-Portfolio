using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOpening : MonoBehaviour
{
    public SceneFader fader;
    public AudioClip bgClip;

    private void Start()
    {
        fader.FadeIn(1f);

        SoundManager.instance.BackgroundSound(bgClip);

    }
}
