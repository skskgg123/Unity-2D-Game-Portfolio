using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyAttack : MonoBehaviour
{
    public int unlockLevel = 3;

    public EnemyType eType = EnemyType.ENEMY;
    private bool isEnemy = false;

    //Enemy의 애니메이션
    private Animator animator;

    public GameObject skillEffect;
    public GameObject attackEffect;
    public AttackButton attackButton;
    public CharacterState character;
    
    //Enemy의 체력
    public float health = 20f;
    [HideInInspector]
    public float currentHealth;
    private float fakeHealth;
    public float barSpeed = 5;
    public float resetHealth;

    //Enemy 정보
    public Image healthBar;
    public bool isDeath = false;
    public GameObject healthInfo;
    public bool isHitAnim = false;

    public float skillDamage = 20f;

    //맞을때 깜박거리는 효과
    private SpriteRenderer enemyRenderer;

    public DrawDialog drawDialog;
    public int dialogNumber = 0;

    public TextMeshProUGUI enemyDamageText;
    public GameObject damageUI;
    // 적 스킬 사용 도중 중첩되지 않도록 방지
    bool runningEnemySkill = false;

    [Header("Boss")]
    public SpriteRenderer fakeBoss;
    public bool isRevive = false;
    public GameObject reviveEffect;
    public bool isPotion = false;
    public bool isRealBoss = false;
    public GameObject bossEffect;
    public GameObject bossSkillEffect;
    public float testNum;

    void Start()
    {
        animator = GetComponent<Animator>();

        currentHealth = health;

        enemyRenderer = GetComponent<SpriteRenderer>();
        //drawDialog.GetComponent<DrawDialog>();



    }

    // Update is called once per frame
    void Update()
    {
        
        if (fakeHealth > 0)
        {
            fakeHealth -= Time.deltaTime * barSpeed;
            if (fakeHealth <= 0)
                fakeHealth = 0;
        }
        healthBar.fillAmount = (currentHealth + fakeHealth) / health;
        RealBossRevive();

        switch(eType)
        {
            case EnemyType.ENEMY:
                isEnemy = true;
                break;
        }
        
    }

    //Enemy enum 상태 조정
    public void SetType(EnemyType type)
    {
        eType = type;

        //리얼 보스 전환후 다이아로그 실행
        if (eType == EnemyType.REALBOSS)
        {
            drawDialog.DialogSelectEvent(dialogNumber);
            isEnemy = false;
        }
    }

    //Enemy 부활하면서 체력 다 채워주기
    public void RealBossRevive()
    {
        //fakeHealth와는 반대로 체력 차오르게 보여주기
        if(isRevive)
        {
            resetHealth += Time.deltaTime * 30;

            healthBar.fillAmount = (resetHealth) / health; 

            //보스로 각성후 풀피일경우 플레이어 포션 장려
            if (healthBar.fillAmount == 1)
            {
                SetType(EnemyType.REALBOSS);
                isRevive = false;
                isPotion = true;
            }
        }



    }

    //부활 이펙트
    public void BossReviveEffect()
    {
        reviveEffect.SetActive(true);
        if (attackButton.clip[8] != null)
            attackButton.BossRevival();
    }

    //애니메이션 행동 후 다시 Idle 상태로 돌아오게 해주는 이벤트
    public void EnemyIdle()
    {
        animator.SetInteger("eState", 0);
    }

    //데미지
    public void TakeDamage(float damage)
    {
        //데미지처리
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, health);

        fakeHealth += damage;

        enemyDamageText.text = damage.ToString();

        if (currentHealth <= 0)
        {

            Death();
        }
        else
        {
            StartCoroutine(Hit());

            //맞는 애니메이션이 있을경우
            if(isHitAnim)
            {
                animator.SetInteger("eState", 4);
            }
        }
        Debug.Log("적의 체력 : " + currentHealth);


    }

    //스킬버튼
    public void SkillButton()
    {
        //죽으면 끝
        if (isDeath)
        {
            return;
        }


        attackButton.isClick = true;
        animator.SetInteger("eState", 3);

        //버튼 중복 클릭 방지
        if (attackButton.isClick)
        {
            attackButton.enemySkillButton.SetActive(false);
        }
    }

    //이펙트 코루틴으로 맞춰서 필드에 계속 남지않게 지정
    IEnumerator AttackEffect()
    {
        attackEffect.SetActive(true);

        yield return new WaitForSeconds(0.7f);

        attackEffect.SetActive(false);
    }

    IEnumerator BossEffect()
    {
        if (bossEffect != null)
        {
            bossEffect.SetActive(true);

            yield return new WaitForSeconds(0.7f);

            bossEffect.SetActive(false);

        }
    }

    //스킬 이펙트 중복 방지
    public void EnemySkillGo()
    {
        if(runningEnemySkill)
        {
            return;
        }
        StartCoroutine(EnemySkill());
    }

    //일반 적들의 스킬 이펙트
    IEnumerator EnemySkill()
    {

        runningEnemySkill = true;

        //if(!attackButton.isBoss)
        skillEffect.SetActive(true);
        //else
        yield return new WaitForSeconds(0.3f);

        //보스가 아닌 일반 몹일때
        if (!attackButton.isBoss)
        {
            character.TakeDamage(skillDamage);
            attackButton.EnemySkillSound();
        }
        //보스일때 특정 시야 가리기 효과
        else 
        {
            attackButton.BossSkillSound();
            yield return new WaitForSeconds(9);
        }


        yield return new WaitForSeconds(1f);
        runningEnemySkill = false;

        skillEffect.SetActive(false);


    }

    public void SkillActionSound()
    {
        attackButton.ActionSound();
    }

    public void BossSkillGo()
    {
        if (runningEnemySkill)
        {
            return;
        }
        StartCoroutine(BossSkill());
    }


    //보스의 스킬 이펙트
    IEnumerator BossSkill()
    {
        runningEnemySkill = true;

        bossSkillEffect.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        character.TakeDamage(skillDamage);

        yield return new WaitForSeconds(testNum);

        runningEnemySkill = false;
        bossSkillEffect.SetActive(false);
    }

    IEnumerator PoisonSkill()
    {

        yield return new WaitForSeconds(0.4f);

        skillEffect.SetActive(true);
        character.TakeDamage(skillDamage);
        
        yield return new WaitForSeconds(0.4f);

        character.TakeDamage(skillDamage);

        yield return new WaitForSeconds(0.4f);

        character.TakeDamage(skillDamage);

        yield return new WaitForSeconds(0.4f);

        character.TakeDamage(skillDamage);

        yield return new WaitForSeconds(0.4f);

        character.TakeDamage(skillDamage);
        yield return new WaitForSeconds(0.2f);

        
        skillEffect.SetActive(false);

        yield return null;
    }

    public void PoisonMushroom()
    {
        animator.SetInteger("eState", 3);
        StartCoroutine(PoisonSkill());
    }

    IEnumerator Hit()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        yield return new WaitForSeconds(0.4f);

        renderer.color = new Color(1, 1, 1, 0);

        damageUI.SetActive(true);
        attackButton.EnemyHit();

        yield return new WaitForSeconds(0.1f);

        renderer.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.1f);

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

    //데미지 피격 효과 다른 방법, 데미지 UI랑 맞추기위해 사용은 하지않음
    /*IEnumerator DamageAction()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        yield return new WaitForSeconds(0.4f);

        damageUI.SetActive(true);

        for (int i = 0; i < 3; i++)
        {

            foreach (SpriteRenderer renderer in renderers)
            {
                Color c = renderer.color;
                c.a = 0f;
                renderer.color = c;
            }

            yield return new WaitForSeconds(0.1f);

            foreach (SpriteRenderer renderer in renderers)
            {
                Color c = renderer.color;
                c.a = 1f;
                renderer.color = c;
            }

            //이게 빠지면 0.3초동안 투명해짐
            yield return new WaitForSeconds(0.1f);

            damageUI.SetActive(false);
        }

        
    }*/

    private void Death()
    {
        //중복 죽음 방지
        if (isDeath)       
            return;

        //죽기
        if (currentHealth <= 0)
        {
            healthBar.fillAmount = 0f;
            if (!attackButton.isBoss)
            {
                isDeath = true;
                animator.SetInteger("eState", 2);

                healthInfo.SetActive(false);
                attackButton.EnemyDie();
                Destroy(gameObject, 1);

                //죽으면 다이아로그 연출, 죽을거면 걍 죽지 구질구질한 이벤트
                if (drawDialog != null && isEnemy)
                    drawDialog.DialogEvent(dialogNumber);
            }
            else
            {
                animator.SetInteger("eState", 2);
                

                StopCoroutine(EnemySkill());
                skillEffect.SetActive(false);
                
                StartCoroutine(RealBoss());

                attackButton.isBoss = false;
                isRealBoss = true;
            }

            LevelClear();
        }
    }

    private void LevelClear()
    {
        Debug.Log("LevelClear");
        //nowLevel 저장한다 : 기존에 저장되어 있는 데이터 체크 
        //저장할때의 시점 체크 
        int nowLevel = PlayerPrefs.GetInt("NowLevel", 1);
        if (unlockLevel > nowLevel)
        {
            PlayerPrefs.SetInt("NowLevel", unlockLevel);
        }
    }

    IEnumerator RealBoss()
    {
        if (fakeBoss != null)
        {
            isDeath = true;


            yield return new WaitForSeconds(3f);


            isRevive = true;
            animator.SetBool("isRealBoss", true);

            resetHealth = 0f;
            currentHealth = health;

        }

    }




}

public enum EnemyType
{
    ENEMY,
    REALBOSS,
}