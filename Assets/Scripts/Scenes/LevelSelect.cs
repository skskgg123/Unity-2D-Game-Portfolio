using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public SceneFader fader;

    private Button[] levelButtons;
    public Transform Scenes;
    public int nowLevel = 1;
    public AudioClip[] clip;

    private void Start()
    {

        int nowLevel = PlayerPrefs.GetInt("NowLevel", 1);

        levelButtons = new Button[Scenes.childCount];

        //��� ������ ��ư ������Ʈ �޾ƿ��� 
        for (int i = 0; i < levelButtons.Length; i++)
        {
            Transform buttonTrans = Scenes.GetChild(i);
            levelButtons[i] = buttonTrans.GetComponent<Button>();
            if (i >= nowLevel)
            {
                levelButtons[i].interactable = false;
            }
        }

    }


    public void LevelButtonSelect(string sceneName)
    {
        Debug.Log("Level01 �� �ε�");
        fader.FadeTo(sceneName);
        SoundManager.instance.SFXPlay("001_Hover_01", clip[0]);

    }



}
