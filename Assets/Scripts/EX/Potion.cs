using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public GameObject potionEvent;

    public EnemyAttack enemy;

    public CharacterState character;

    public AttackButton attackButton;

    private void Update()
    {
        if(enemy.isPotion)
        {
            potionEvent.SetActive(true);
        }

        if (character.currentHealth >= character.health)
        {
            character.currentHealth = character.health;
        }


    }

    public void RealPotion()
    { 
        character.currentHealth += 15;
        enemy.isPotion = false;
        potionEvent.SetActive(false);
        enemy.isDeath = false;
        enemy.isRevive = false;
        if (attackButton.clip[9] != null)
            attackButton.LegendaryPotion();
    }

    public void FakePotion()
    {
        character.TakeDamage(5);
        enemy.isPotion = false;
        potionEvent.SetActive(false);
        enemy.isDeath = false;
        enemy.isRevive = false;
        attackButton.EnemyHit();
    }



}
