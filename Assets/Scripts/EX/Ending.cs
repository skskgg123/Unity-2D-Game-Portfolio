using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public DrawDialog dialog;

    public Animator playerAnim;
    public GameObject player;
    public Animator enemyAnim;
    public GameObject enemy;

    public SceneFader sceneFader;

    public GameObject explosion;
    public GameObject endingButton;
    public Animator endingAnim;
    private bool isEnding;
    public GameObject credit;
    public AudioClip clip;

    private void Start()
    {
        StartCoroutine(LastDialog());
    }

    private void Update()
    {
        if(isEnding)
        {
            if (Input.anyKey)
            {
                credit.SetActive(false);
                endingAnim.SetBool("isCredit", false);
                sceneFader.FadeTo("MainMenu");
            }
        }
    }

    IEnumerator LastDialog()
    {

        yield return new WaitForSeconds(3);

        playerAnim.SetInteger("pState", 1);

        yield return new WaitForSeconds(1.5f);

        enemyAnim.SetInteger("eState", 1);
        Destroy(enemy, 1);

        yield return new WaitForSeconds(1);

        explosion.SetActive(true);

        yield return new WaitForSeconds(1);

        playerAnim.SetInteger("pState", 2);
        Destroy(player, 1);

        yield return new WaitForSeconds(1.5f);


        dialog.DialogEvent(6);

        yield return new WaitForSeconds(2.5f);

        explosion.SetActive(false);
        endingButton.SetActive(true);

    }

    public void EndingCredit()
    {
        credit.SetActive(true);
        endingAnim.SetBool("isCredit", true);
        isEnding = true;
        SoundManager.instance.SFXPlay("Ending", clip);
    }


}
