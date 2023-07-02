using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstDialogEvent : DrawDialog
{

    public override void EndDialog()
    {
        InitDialog();

        nextButton.SetActive(false);
        background.SetActive(false);

        if (characterState != null)
            EndingDialog();


        //���� ��ȭ�� �ִ��� üũ
        if (nextDialog > -1)
        {
            DialogEvent(nextDialog);
        }
    }

}
