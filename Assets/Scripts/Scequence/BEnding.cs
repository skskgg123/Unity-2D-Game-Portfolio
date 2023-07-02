using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BEnding : MonoBehaviour
{
    public SceneFader fader;
    public string loadToScene = "BattleScene02";
    public float nextDelayTime = 1.5f;

    public EnemyAttack enemy;


    private void Update()
    {
        StartCoroutine(NextScene());
    }

    IEnumerator NextScene()
    {
        
        if (enemy == null)
        {

            yield return new WaitForSeconds(nextDelayTime);

            fader.FadeTo(loadToScene);

        }
    }

}
