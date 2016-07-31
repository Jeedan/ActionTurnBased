using UnityEngine;
using System.Collections;

public class BasicEnemy : Entity
{
    // Entity enemy;
    Animator enemyAnimator;

    // TODO very simple attack test
    public float attackTimer = 0;
    public bool attackReady = false;
    public float timeBetweenAttacks = 3.0f;


    // TODO FLASH ON HURT
    public SpriteRenderer theSprite;
    public Color origColor;
    public Color flashColor;
    public int flashAmount = 3;
    public float flashTimer;
    public bool flashing;

    // TODO slash attack animation
    public GameObject slashAnimPrefab;
    public bool playedSLash = false;

    protected override void Awake()
    {
        base.Awake();
        theSprite = GetComponentInChildren<SpriteRenderer>();
        origColor = theSprite.color;

    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        //enemy = GetComponent<Entity>();
        enemyAnimator = GetComponent<Animator>();
        //enemy.basicAttack.OnAttackStart += PlayAnticipationAnimation;
        //enemy.basicAttack.OnAttackFinish += OnBasicAttackFinish;
        basicAttack.OnDealDamage += PlaySlashAttackAnimation;
        basicAttack.OnAttackStart += PlayAnticipationAnimation;
        basicAttack.OnAttackFinish += OnBasicAttackFinish;

        for (int i = 0; i < abilityList.Length; i++)
        {
            abilityList[i].OnAttackStart += PlayAnticipationAnimation;
            abilityList[i].OnDealDamage += PlaySlashAttackAnimation;
            abilityList[i].OnAttackFinish += OnBasicAttackFinish;
        }
    }

    public void FlashColor()
    {
        flashing = !flashing;
        if (flashing)
        {
            theSprite.color = flashColor;
        }
        else
        {
            //theSprite.color = Color.Lerp(theSprite.color, origColor, flashTimer * Time.deltaTime);
            theSprite.color = origColor;

        }
    }

    IEnumerator Flash()
    {
        var t = 0;
        var flashes = flashAmount;
        while (t < flashes)
        {
            Debug.Log("Flashing " + flashing + " " + t);
            FlashColor();
            t++;
            yield return new WaitForSeconds(flashTimer);
        }

    }

    void PlayAnticipationAnimation()
    {
        playedSLash = false;
        flashColor = Color.red;
        StartCoroutine(Flash());
        if (enemyAnimator)
        {
            enemyAnimator.SetTrigger("basic_Anticipation");
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        flashColor = Color.black;
        StartCoroutine(Flash());
    }

    void PlaySlashAttackAnimation()
    {
        if (slashAnimPrefab && !playedSLash)
        {
            playedSLash = !playedSLash;
            Vector3 slashSpawn = target.battlePosition;
            slashSpawn.x += 0.35f;
            slashSpawn.z = -4.0f;
            GameObject slashAnim = Instantiate(slashAnimPrefab, slashSpawn, Quaternion.identity) as GameObject;
            Destroy(slashAnim, 0.7f);
        }
    }

    void OnBasicAttackFinish()
    {
        Debug.Log("Basic Enemy is done attacking");
    }

    public void UpdateAttackTimer()
    {
        // attack timer should start at 0
        // it should charge up over time
        // attackTimer starts at 0
        // attackTimer += Time.DeltaTime;
        // when it has reached readyTime
        // 
        if (!basicAttack.isAttacking)
        {

            if (attackTimer < timeBetweenAttacks)
            {
                attackTimer += Time.deltaTime;
                attackReady = false;
                // Debug.Log("enemy attackTimer " + attackTimer);
            }

            else if (attackTimer >= timeBetweenAttacks)
            {
                attackReady = true;
                attackTimer = 0;

                Debug.Log("enemy attackReady ");
            }
        }

        //if(Time.time > nextAttack)
        //{
        //    attackReady = true;
        //    nextAttack = Time.time + timeBetweenAttacks;
        //}

    }
}
