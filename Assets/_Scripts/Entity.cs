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

    // events
    //public Action OnAttackStart;
    //public Action OnAttackFinish;
    
    public MeleeAttack basicAttack;

    //Battle position
    public Vector3 battlePosition;



    Entity target;

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

    void Start()
    {

        battlePosition = transform.parent.transform.position;
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

        //if (OnAttackFinish != null)
        //{
        //    Debug.Log("dood");
        //    OnAttackFinish();
        //}
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
    }

    private void DealDamageToTarget()
    {
        DealDamage(target, damage);
    }

    public void PerformBasicAttack(Entity _target)
    {
        target = _target;


        if (target != null && !target.basicAttack.isAttacking && !Dead())
        {
            // set the target's position
            basicAttack.SetTarget(target.gameObject);
            basicAttack.SetTargetPosition(target.battlePosition);
            basicAttack.Anticipation();
        }
    }
}
