using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    public AttackButton attackButton;


    //카운트 초기화
    private void OnEnable()
    {
        attackButton.isCountButton = false;
        attackButton.isStrongAttack = false;
        //attackButton.count = 0;
    }

}
