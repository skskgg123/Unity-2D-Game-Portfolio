using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    public AttackButton attackButton;


    //ī��Ʈ �ʱ�ȭ
    private void OnEnable()
    {
        attackButton.isCountButton = false;
        attackButton.isStrongAttack = false;
        //attackButton.count = 0;
    }

}
