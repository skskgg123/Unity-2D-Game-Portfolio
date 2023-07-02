using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;


public class DrawDialog : MonoBehaviour
{
    public string xmlFile = "Dialog";

    //xml
    private XmlNodeList allNodes;
    protected Queue<Dialog> dialogs;

    //UI
    public Text nameText;
    public Text sentenceText;
    public GameObject npcImage;
    public GameObject nextButton;
    public GameObject background;

    public bool isButton = false;

    private string dialogSentence;
    public int nextDialog;

    public GameObject explosion;

    public CharacterState characterState;

    public SceneFader fader;
    public string loadToScene = "VillageScenes01";

    private void Start()
    {
        LoadDialogXml(xmlFile);

        dialogs = new Queue<Dialog>();
        //characterState.GetComponent<CharacterState>();
        InitDialog();

    }

    public void InitDialog()
    {
        dialogs.Clear();

        nameText.text = "";
        sentenceText.text = "";
        dialogSentence = "";

        background.SetActive(false);
        npcImage.SetActive(false);
        nextButton.SetActive(false);
        isButton = false;
    }

    public void LoadDialogXml(string fileName)
    {
        TextAsset xmlFile = Resources.Load("Dialog/" + fileName) as TextAsset;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlFile.text);
        allNodes = xmlDoc.SelectNodes("root/dialog");
    }

    IEnumerator StartDialog(int dialogNumber)
    {
        InitDialog();

        yield return new WaitForSeconds(2.2f);

        nextButton.SetActive(true);
        background.SetActive(true);
        foreach (XmlNode node in allNodes)
        {
            int num = int.Parse(node["number"].InnerText);
            if (num == dialogNumber)
            {
                Dialog dialog = new Dialog();
                dialog.number = num;
                dialog.character = int.Parse(node["character"].InnerText);
                dialog.name = node["name"].InnerText;
                dialog.sentence = node["sentence"].InnerText;
                dialog.next = int.Parse(node["next"].InnerText);

                dialogs.Enqueue(dialog);
            }
        }      

        //첫번째 대화를 보여준다
        DrawNext();
    }



    public void DialogEvent(int diaNum)
    {
        StartCoroutine(StartDialog(diaNum));
    }

    public void DialogSelectEvent(int dialogNumber)
    {
        {
            InitDialog();
            nextButton.SetActive(true);
            background.SetActive(true);
            foreach (XmlNode node in allNodes)
            {
                int num = int.Parse(node["number"].InnerText);
                if (num == dialogNumber)
                {
                    Dialog dialog = new Dialog();
                    dialog.number = num;
                    dialog.character = int.Parse(node["character"].InnerText);
                    dialog.name = node["name"].InnerText;
                    dialog.sentence = node["sentence"].InnerText;
                    dialog.next = int.Parse(node["next"].InnerText);

                    dialogs.Enqueue(dialog);
                }
            }

            //첫번째 대화를 보여준다
            DrawNext();
        }
    }

    //dialog Queue 에 있는 내용을 하나씩 꺼내서 보여주기
    public void DrawNext()
    {
        if (dialogs.Count == 0)
        {
            EndDialog();
            return;
        }

        //대화 내용 셋팅
        Dialog dialog = dialogs.Dequeue();

        nameText.text = dialog.name;
        if(dialog.character == 0)
        {
            npcImage.GetComponent<Image>().sprite = null;
        }
        npcImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("characters/image0" + dialog.character.ToString());

        if (npcImage.GetComponent<Image>().sprite != null)
            npcImage.SetActive(true);
        else
            npcImage.SetActive(false);
        

        dialogSentence = dialog.sentence;
        nextDialog = dialog.next;

        //대화문 타이핑 연출
        StartCoroutine(typingSentence(dialog.sentence));

    }

    IEnumerator typingSentence(string typingText)
    {
        isButton = true;
        sentenceText.text = "";

        foreach (char latter in typingText)
        {
            sentenceText.text += latter;
            yield return new WaitForSeconds(0.05f);
        }


        isButton = false;
    }

    public void DrawSentece()
    {
        if (dialogSentence == "")
            return;

        StopAllCoroutines();
        sentenceText.text = dialogSentence;
        isButton = false;

    }

    public virtual void NextButton()
    {      
        if (isButton)
        {    
            DrawSentece();
        }
        else
        {           
            DrawNext();
        }
    }

    IEnumerator EndingEvent()
    {
        yield return new WaitForSeconds(0.5f);

        if (explosion != null)
        {
            explosion.SetActive(true);
        }

        yield return new WaitForSeconds(2);

        characterState.FakeDie();

        yield return new WaitForSeconds(2f);
        fader.FadeTo(loadToScene);

    }

    public virtual void EndDialog()
    {
        
    }

    public void EndingDialog()
    {
        StartCoroutine(EndingEvent());
    }

    


}
