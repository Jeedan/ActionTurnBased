using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    public string myName = "entity";
    public bool isDead = false;
    public float health;
    protected float maxHealth;

    public float damage = 1;
    protected bool hasDealtDamage = false;

    // events
    //public Action OnAttackStart;
    //public Action OnAttackFinish;

    public Abilities basicAttack;
    public Abilities[] abilityList = new Abilities[] { new MeleeAttack(), new DiagonalAttacks() };

    //Battle position
    public Vector3 battlePosition;

    protected Entity target;

    // Use this for initialization
    protected virtual void Awake()
    {
        maxHealth = health;
        //basicAttack = GetComponent<Abilities>();


    }

    protected virtual void Start()
    {
        battlePosition = transform.parent.transform.position;

        CapsuleCollider collider = GetComponent<CapsuleCollider>();

        basicAttack = new MeleeAttack();
        basicAttack.Initialize(transform, collider, this);

        if (basicAttack != null)
        {
            basicAttack.OnAttackFinish += OnAttackComplete;
            basicAttack.OnDealDamage += DealDamageToTarget;
        }

        abilityList = new Abilities[2];

        abilityList[0] = new MeleeAttack();
        abilityList[1] = new DiagonalAttacks();

        for (int i = 0; i < abilityList.Length; i++)
        {
            abilityList[i].Initialize(transform, collider, this);

            if(abilityList[i] != null)
            {
                abilityList[i].OnAttackFinish += OnAttackComplete;
                abilityList[i].OnDealDamage += DealDamageToTarget;
            }
        }

    }

    public virtual void PickRandomMove()
    {
        var randomIndex = Random.Range(0, abilityList.Length);

        basicAttack = abilityList[randomIndex];

        Debug.Log("Ability choice " + randomIndex);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Dead())
        {
            Debug.Log("We dead");
            gameObject.SetActive(false);
        }
    }


    public virtual void DealDamage(Entity target, float dmg)
    {
        // do not deal damage if we already dealt damage this frame

        if (hasDealtDamage) return;

        if (target != null && target.Dead())
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

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        // TODO ADD UPDATEHEALTH EVENT FOR UI
    }

    public virtual bool Dead()
    {
        return health <= 0;
    }

    // if we use an object pooler we can use this function to reset our entities
    public virtual void Reset()
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

    protected virtual void OnAttackComplete()
    {

        hasDealtDamage = false;
        // this is used for UI stuff
    }

    protected virtual void DealDamageToTarget()
    {
        DealDamage(target, damage);
    }

    public virtual void PerformBasicAttack(Entity _target)
    {

        target = _target;

        if (target != null && !target.basicAttack.isAttacking && !Dead())
        {
            // set the target's position
            basicAttack.SetTarget(target.gameObject);
            basicAttack.SetTargetPosition(target.battlePosition);
            basicAttack.BeginAbility();
            //basicAttack.Anticipation();
        }
    }

    public void TestCoroutineStart(IEnumerator startCor)
    {
        IEnumerator newCor = startCor;
        StartCoroutine(newCor);
    }
}
