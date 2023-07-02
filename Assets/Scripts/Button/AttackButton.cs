using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    [Header("Buttons")]
    //Plyer 버튼 
    public GameObject attackButton;
    public GameObject shieldButton;
    public GameObject strongButton;
    public GameObject enemySkillButton;
    public GameObject bossSkillButton;

    //버튼 중복 클릭 방지
    public bool isClick = false;

    [Header("PlayerState")]
    //플레이어 능력치
    public CharacterState characterState;
    public float attackDamage = 5f; 

    [Header("EnemyState")]
    //적 능력치
    public EnemyAttack enemyState;
    public float enemyDamage = 10f;
    public bool isBoss = false;
    public float enemySkillAction = 10f;

    //player 버튼 카운트다운
    private float countdown = 0f;
    [Header("Countdown")]
    public float buttonTimer = 2f;

    //enemy 카운트다운
    private float enemyCountdown = 0f;
    public float enemyTurn = 10f;
    public TextMeshProUGUI enemyTimeText;
    public float opneDelay = 11;

    [Header("ButtonPosition")]
    //버튼 랜덤 등장 x값
    public float spawnmaxRangeX = 150;
    public float spawnminRangeX = 500;
    public float spawnRangeY = 390;

    [Header("Animator")]
    //공격 애니메이터
    public Animator animator;
    public Animator enemyAnimator;

    [Header("Count")]
    //카운트
    public int count = 0;
    public bool isCountButton = false;
    public bool isStrongAttack = false;

    //스페셜 콤보 어택
    private int specialCombo = 0;
    private bool isCombo = false;
    private bool isReadyCombo = false;
    public TextMeshProUGUI hitText;
    public Animator hitTextAnim;
    public GameObject specialAtt;
    public Animation specialButtonAnim;

    //사운드
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

        //적이 죽지않았을때만 행동
        if (!enemyState.isDeath && !characterState.isDeath)
        {
            //플레이어의 공격 카운트다운
            if (countdown <= 0f)
            {
                ButtonPosition();

                StartCoroutine(AButton());

                countdown = buttonTimer;
            }
            countdown -= Time.deltaTime;

            SpecialAttack();
            //Enemy의 공격 카운트다운
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
              Debug.Log("콤보초기화 : " + specialCombo);*/
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
            Debug.Log(specialCombo + " 콤보");
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
        Debug.Log("카운트 : " + count);

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
