using UnityEngine;
using System.Collections;
using System;

public class Entity : MonoBehaviour
{
    public string myName = "entity";
    public bool isDead = false;
    public float health;
    private float maxHealth;

    public float damage = 1;
    private bool hasDealtDamage = false;

    public Action OnAttackStart;
    public Action OnAttackFinish;

    public MeleeAttack basicAttack;



    // TODO very simple attack test
    public bool attackReady = false;
    public float timeBetweenAttacks = 3.0f;
    private float nextAttack = 0;

    Entity target;
    public float attackTimer = 0;
    // Use this for initialization
    void Awake()
    {
        maxHealth = health;
        basicAttack = GetComponent<MeleeAttack>();

        if (basicAttack != null)
        {
            basicAttack.OnAttackFinish += OnAttackComplete;
            basicAttack.OnDealDamage += DealDamageToTarget;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Dead())
        {
            Debug.Log("We dead");
            gameObject.SetActive(false);
        }

    }


    public void DealDamage(Entity target, float dmg)
    {
        // do not deal damage if we already dealt damage this frame
       
        if (hasDealtDamage) return;

        if (target.Dead())
        {
            basicAttack.isAttacking = false;
            hasDealtDamage = false;
        }

        if (target != null && !target.Dead())
        {
            target.TakeDamage(dmg);
            hasDealtDamage = true;
            Debug.Log("target took " + dmg);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public bool Dead()
    {
        return health <= 0;
    }

    // if we use an object pooler we can use this function to reset our entities
    public void Reset()
    {
        health = maxHealth;
        isDead = false;

        if (OnAttackFinish != null)
        {
            Debug.Log("dood");
            OnAttackFinish();
        }
    }


    //========================================================================================
    //
    // THIS IS WHERE WE HAVE ATTACK METHODS
    //
    //========================================================================================

    private void OnAttackComplete()
    {

        hasDealtDamage = false;
        // this is used for UI stuff
        if (OnAttackFinish != null)
        {
            Debug.Log("dood");
            OnAttackFinish();
        }
    }

    private void DealDamageToTarget()
    {
        DealDamage(target, damage);
    }

    public void PerformBasicAttack(Entity _target)
    {
        target = _target;
        
        if (target != null)
        {

            if (OnAttackStart != null)
            {
                OnAttackStart();
            }

            // set the target's position
            basicAttack.SetTarget(target.gameObject);
            basicAttack.SetTargetPosition(target.transform.position);
            basicAttack.Anticipation();
        }
    }


    public void UpdateAttackTimer()
    {
        // TODO update the attack timer
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
