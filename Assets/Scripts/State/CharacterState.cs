using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterState : MonoBehaviour
{
    private Animator animator;

    public GameObject shield;
    public GameObject strongEffect;
    public GameObject attackEffect;
    public GameObject strongAttackEffect;

    public float strongDamage = 10f;

    public float health = 20f;
    [HideInInspector]
    public float currentHealth;

    public Image healthBar;
    public bool isDeath = false;
    public GameObject healthInfo;
    private float fakeHealth;
    public float barSpeed = 5;

    private bool isShield = false;

    public AttackButton attackButton;

    public EnemyAttack enemyAttack;

    public TextMeshProUGUI damageText;
    public GameObject damageUI;

    public GameObject gameOver;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if(fakeHealth > 0)
        {
            fakeHealth -= Time.deltaTime * barSpeed;
            if (fakeHealth <= 0)
                fakeHealth = 0;
        }

        healthBar.fillAmount = (currentHealth + fakeHealth) / health;
    }

    //���ÿ� �⺻ Idel ����
    public void PlayerIdle()
    {
        animator.SetInteger("pState", 0);
    }

    public void FakeDie()
    {
        animator.SetInteger("pState", 3);
    }

    //�ǵ��ư
    public void ShieldButton()
    {
        if(isDeath)
        {
            return;
        }

        isShield = true;
        shield.SetActive(true);
        attackButton.isClick = true;
        attackButton.ShieldSound();

        //�ߺ� ���� ����
        if(attackButton.isClick)
        {
            attackButton.shieldButton.SetActive(false);
        }
    }

    //���Ѱ��� ��ư
    public void StrongButton()
    {
        if (isDeath)
        {
            return;
        }

        animator.SetInteger("pState", 2);
        attackButton.isClick = true;

        enemyAttack.TakeDamage(strongDamage);
        Debug.Log("���� ����!" + strongDamage);

        //�ߺ� ���� ����
        if(attackButton.isClick)
        {
            attackButton.strongButton.SetActive(false);
        }

    }

    //���� ���� ����Ʈ
    IEnumerator StrongEffect()
    {
        strongEffect.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        strongAttackEffect.SetActive(true);
        attackButton.StrongAttackSound();

        yield return new WaitForSeconds(0.4f);

        strongEffect.SetActive(false);
        strongAttackEffect.SetActive(false);

    }

    //���� ����Ʈ
    IEnumerator AttackEffect()
    {
        attackEffect.SetActive(true);

        yield return new WaitForSeconds(0.9f);

        attackEffect.SetActive(false);
    }

    //������ ó��
    public void TakeDamage(float damage)
    {
        //���� �Ű澲��
        if (isShield)
        {
            damage = 0;   
        }

        //������ ���
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, health);

        fakeHealth += damage;

        damageText.text = damage.ToString();

        //�׾����� ����
        if (currentHealth <= 0)
        {
            Death();
        }
        else
        {

             StartCoroutine(Hit());
             
        }    
        Debug.Log("���� ü�� : " + currentHealth);
    }

    //����
    private void Death()
    {
        if (isDeath)
            return;

        if (currentHealth <= 0)
        {
            isDeath = true;
            
            animator.SetInteger("pState", 3);

            healthInfo.SetActive(false);
  
            attackButton.enabled=(false);

            Invoke("Die", 3);
        }

    }

    public void Die()
    {
        gameOver.SetActive(true);

    }

    IEnumerator Hit()
    {
        if (isShield)
        {
            yield return new WaitForSeconds(0.5f);
            shield.SetActive(false);
            isShield = false;
            attackButton.PlayerShieldHit();
        }
        else
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();

            yield return new WaitForSeconds(0.4f);

            renderer.color = new Color(1, 1, 1, 0);

            damageUI.SetActive(true); 
            attackButton.PlayerHit();

            yield return new WaitForSeconds(0.1f);

            if(enemyAttack.isRealBoss)
                attackButton.PlayerHit();
            renderer.color = new Color(1, 1, 1, 1);

            yield return new WaitForSeconds(0.1f);

            if (enemyAttack.isRealBoss)
                attackButton.PlayerHit();

            renderer.color = new Color(1, 1, 1, 0);

            yield return new WaitForSeconds(0.1f);

            renderer.color = new Color(1, 1, 1, 1);

            yield return new WaitForSeconds(0.1f);

            renderer.color = new Color(1, 1, 1, 0);

            yield return new WaitForSeconds(0.1f);

            renderer.color = new Color(1, 1, 1, 1);

            damageUI.SetActive(false);

            yield return null;

        }
    }


}

