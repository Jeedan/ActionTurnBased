using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class Abilities : IAbility
{
    public string mName = "Basic Ability";
    protected Transform transform;
    protected Vector3 originalPosition;
    protected Vector3 targetPosition;
    protected GameObject target;

    public bool isAttacking = false;

    protected float returnSpeed = 1.0f;
    public float attackSpeed = 1.5f;

    public float anticiapationDuration = 0.5f;
    public float attackDelay = 0.5f; 
    public float exitDuration = 0.5f;

    public Action OnAttackStart;
    public Action OnDealDamage;
    public Action OnAttackFinish;

    // collision radius
    // we get the collision radius from the capsule collider that is placed on the characters
    protected float attackThreshold = 0.25f;
    protected float myCollisionRadius;
    protected float targetCollisionRadius;

    public Entity owner;

    public virtual void Initialize(Transform transform, CapsuleCollider collisionRadius, Entity owner)
    {
        this.transform = transform;
        myCollisionRadius = collisionRadius.radius;
        this.owner = owner;
        originalPosition = transform.position;
    }

    // Use this for initialization
    protected virtual void Start()
    {
        //originalPosition = transform.position;
        //myCollisionRadius = GetComponent<CapsuleCollider>().radius;
    }

    public void SetTarget(GameObject _target)
    {
        target = _target;
    }

    public void SetTargetPosition(Vector3 _target)
    {
        targetPosition = _target;
    }

    // this is used to indicate the player that we are about to attack
    // sometimes its just the enemy flashing but it can be something else as well
    protected virtual void PlayAnticipationAnimation()
    {
        if (OnAttackStart != null)
        {
            OnAttackStart(); // event for animations and other effects
        }
    }


    // utility to check for a target to deal damage to --- REUSABLE CODE
    protected void CheckForTarget(Vector3 targetPosition)
    {
        Collider[] targetCollider = Physics.OverlapSphere(targetPosition, targetCollisionRadius * 0.7f);

        for (int i = 0; i < targetCollider.Length; ++i)
        {
            Entity ent = targetCollider[i].GetComponent<Entity>();

            if (ent != null)//&& ent.myName.Equals("Player"))
            {
                if (OnDealDamage != null)
                {
                    OnDealDamage();
                }
            }
        }
    }

    //========================================================================================
    //
    // THIS IS WHERE THE ABILITY FLOW STARTS
    // THE FLOW GOES
    // BeginAbility -> Anticipation -> FollowThrough 
    // -> Attack -> (the animation) -> CompleteAttack -> FinishAttack
    //
    //========================================================================================

    public void BeginAbility()
    {
        Anticipation();
    }

    // initialization of ability
    // play an animation
    // wait a few seconds then perform the attack
    protected void Anticipation()
    {
        isAttacking = true;
        owner.StartCoroutine(Anticipate());
       // StartCoroutine(Anticipate());
    }

    // this is where the lerp towards the enemy happens
    // or firing a projectile
    protected void FollowThrough()
    {
        // TODO spawn hit animation
        Debug.Log("FollowThrough!");
        owner.StartCoroutine(Attack());
        //StartCoroutine(Attack());
    }

    protected void CompleteAttack()
    {
        owner.StartCoroutine(FinishAttack());
        //StartCoroutine(FinishAttack());
    }


    public virtual IEnumerator Anticipate()
    {
        Debug.Log("PERFORMING ANTICIPATION...I'm about to smack you!");
        yield return new WaitForSeconds(anticiapationDuration);
        FollowThrough();
    }

    public virtual IEnumerator Attack()
    {
        yield return Lunge();
    }

    public virtual IEnumerator FinishAttack()
    {
        // return to original position
        Vector3 currentPos = transform.position;
        // yield return StartCoroutine(LerpTowardsTarget(currentPos, originalPosition, returnSpeed, exitDuration));
        yield return TweenUtil.LerpTowardsTarget(currentPos, originalPosition, returnSpeed, exitDuration, transform);

        isAttacking = false;
        if (OnAttackFinish != null)
        {
            OnAttackFinish();
        }

    }

    // lunge at target and then reset to the original position
    IEnumerator Lunge()
    {
        Vector3 startPos = transform.position;
        Vector3 attackPos = targetPosition;
        // we want to move to the target but stop just a bit before him
        Vector3 dirToTarget = (attackPos - transform.position).normalized;
        Vector3 attackPosition = attackPos - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackThreshold);

        float percent = 0;
        while (percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;
            // this interpolates from 0 to 1 and then back from 1 to 0
            float interpolate = (-percent * percent + percent) * 4;
            transform.position = Vector3.Lerp(startPos, attackPosition, interpolate);

            if (interpolate >= 0.95f)
            {
                //OVERLAP SPHERE FOR DAMAGE
                CheckForTarget(targetPosition);
            }

            yield return null;
        }

        yield return new WaitForSeconds(exitDuration);

        CompleteAttack();
    }
}
