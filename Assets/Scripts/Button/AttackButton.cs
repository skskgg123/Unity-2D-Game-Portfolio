using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    [Header("Buttons")]
    //Plyer ��ư 
    public GameObject attackButton;
    public GameObject shieldButton;
    public GameObject strongButton;
    public GameObject enemySkillButton;
    public GameObject bossSkillButton;

    //��ư �ߺ� Ŭ�� ����
    public bool isClick = false;

    [Header("PlayerState")]
    //�÷��̾� �ɷ�ġ
    public CharacterState characterState;
    public float attackDamage = 5f; 

    [Header("EnemyState")]
    //�� �ɷ�ġ
    public EnemyAttack enemyState;
    public float enemyDamage = 10f;
    public bool isBoss = false;
    public float enemySkillAction = 10f;

    //player ��ư ī��Ʈ�ٿ�
    private float countdown = 0f;
    [Header("Countdown")]
    public float buttonTimer = 2f;

    //enemy ī��Ʈ�ٿ�
    private float enemyCountdown = 0f;
    public float enemyTurn = 10f;
    public TextMeshProUGUI enemyTimeText;
    public float opneDelay = 11;

    [Header("ButtonPosition")]
    //��ư ���� ���� x��
    public float spawnmaxRangeX = 150;
    public float spawnminRangeX = 500;
    public float spawnRangeY = 390;

    [Header("Animator")]
    //���� �ִϸ�����
    public Animator animator;
    public Animator enemyAnimator;

    [Header("Count")]
    //ī��Ʈ
    public int count = 0;
    public bool isCountButton = false;
    public bool isStrongAttack = false;

    //����� �޺� ����
    private int specialCombo = 0;
    private bool isCombo = false;
    private bool isReadyCombo = false;
    public TextMeshProUGUI hitText;
    public Animator hitTextAnim;
    public GameObject specialAtt;
    public Animation specialButtonAnim;

    //����
    public AudioClip[] clip;

    private void Start()
    {
        animator.GetComponent<Animator>();
        enemyAnimator.GetComponent<Animator>();
        characterState.GetComponent<CharacterState>();
        enemyState.GetComponent<EnemyAttack>();

        countdown = buttonTimer;

        enemyCountdown = opneDelay;

        hitTextAnim.SetBool("isOpen", false);
        hitText.text = "";
        
    }

    private void Update()
    {

        //���� �����ʾ������� �ൿ
        if (!enemyState.isDeath && !characterState.isDeath)
        {
            //�÷��̾��� ���� ī��Ʈ�ٿ�
            if (countdown <= 0f)
            {
                ButtonPosition();

                StartCoroutine(AButton());

                countdown = buttonTimer;
            }
            countdown -= Time.deltaTime;

            SpecialAttack();
            //Enemy�� ���� ī��Ʈ�ٿ�
            EnemyAttack();
        }


    }

    IEnumerator AButton()
    {
        
        yield return new WaitForSeconds(1);

        if (!isCountButton && !isStrongAttack)
        {
            attackButton.SetActive(true);
        }
        else
        {
           
            shieldButton.SetActive(true);
            strongButton.SetActive(true);

            yield return new WaitForSeconds(0.7f);

            shieldButton.SetActive(false);
            strongButton.SetActive(false);

        }

        if (enemyState.currentHealth <= enemySkillAction && !enemyState.isRealBoss)
        {
            enemySkillButton.SetActive(true);
            
        }

        if (enemyState.isRealBoss && bossSkillButton != null)
        {
            if(enemyState.currentHealth <= enemySkillAction)
            bossSkillButton.SetActive(true);
        }

        yield return new WaitForSeconds(0.6f);

        attackButton.SetActive(false);

        shieldButton.SetActive(false);

        strongButton.SetActive(false);

        enemySkillButton.SetActive(false);

        if(bossSkillButton != null)
            bossSkillButton.SetActive(false);

        if (!isCombo)
        {
            if (isReadyCombo)
            {
                isReadyCombo = false;
            }
            else
            {
                hitText.text = "";
                hitTextAnim.SetBool("isOpen", false);
                specialCombo = 0;
            }
            
              /*specialCombo = 0;
              Debug.Log("�޺��ʱ�ȭ : " + specialCombo);*/
        }
        isClick = false;
        isCombo = false;

       
    }



    private void ButtonPosition()
    {


        Vector2 spawnPos = new Vector2(Random.Range(spawnminRangeX, spawnmaxRangeX), spawnRangeY);
        attackButton.transform.position = spawnPos;

        Vector2 eSpawnPos = new Vector2(Random.Range(spawnminRangeX, spawnmaxRangeX), spawnRangeY-1);
        enemySkillButton.transform.position = eSpawnPos;

        if(spawnPos == eSpawnPos)
        {
            enemySkillButton.SetActive(false);
        }
    }

    private void EnemyAttack()
    {
        if (!characterState.isDeath)
        {
            if (enemyCountdown <= 0f)
            {
                enemyCountdown = enemyTurn;
                enemyAnimator.SetInteger("eState", 1);
                SoundManager.instance.SFXPlay("PlayerHit", clip[4]);
                characterState.TakeDamage(enemyDamage);
            }
            enemyCountdown -= Time.deltaTime;

            enemyTimeText.text = Mathf.Round(enemyCountdown).ToString();

        }
    }


    public void AttButton()
    {
        if (characterState.isDeath)
        {
            return;
        }

        isClick = true;

        if (isClick)
        {
            isCombo = true;
            specialCombo++;
            attackButton.SetActive(false);
            Debug.Log(specialCombo + " �޺�");
            hitTextAnim.SetBool("isOpen", true);
            hitText.text = specialCombo.ToString();
        }

        count++;
 
        animator.SetInteger("pState", 1);

        if (count == 5)
        {
            isCountButton = true;
            isStrongAttack = true;
            count = 0;
            isReadyCombo = true;
        }
        Debug.Log("ī��Ʈ : " + count);

        enemyState.TakeDamage(attackDamage);

        SoundManager.instance.SFXPlay("PlayerAttack", clip[0]);

        Invoke("ActionSound", 0.5f);
    }

    void SpecialAttack()
    {
        if (specialCombo >= 3)
        {
            specialAtt.SetActive(true);
            specialButtonAnim.Play();
        }
        else
        {
            specialButtonAnim.Stop();
            attackButton.GetComponentInChildren<Image>().color = Color.white;
            specialAtt.SetActive(false);
        }
    }

    public void ActionSound()
    {
        SoundManager.instance.SFXPlay("PlayerAttack", clip[0]);
    }

    public void EnemyHit()
    {
        SoundManager.instance.SFXPlay("EnemyHit", clip[1]);
    }

    public void StrongAttackSound()
    {
        SoundManager.instance.SFXPlay("StrongAttack", clip[2]);
    }

    public void ShieldSound()
    {
        SoundManager.instance.SFXPlay("Shield", clip[3]);
    }

    public void PlayerHit()
    {
        SoundManager.instance.SFXPlay("PlayerHit", clip[1]);
    }

    public void PlayerShieldHit()
    {
        SoundManager.instance.SFXPlay("PlayerShieldHit", clip[5]);
    }

    public void EnemySkillSound()
    {
        SoundManager.instance.SFXPlay("EnemySkill", clip[6]);
    }

    public void EnemyDie()
    {
        SoundManager.instance.SFXPlay("EnemySkill", clip[7]);
    }

    public void BossRevival()
    {
        SoundManager.instance.SFXPlay("BossRevival", clip[8]);
    }

    public void LegendaryPotion()
    {
        SoundManager.instance.SFXPlay("BossRevival", clip[9]);
    }

    public void BossSkillSound()
    {
        SoundManager.instance.SFXPlay("BossRevival", clip[10]);
    }

}
