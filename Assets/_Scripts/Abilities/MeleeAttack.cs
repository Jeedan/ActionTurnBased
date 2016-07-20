using UnityEngine;
using System.Collections;
using System;

public class MeleeAttack : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private GameObject target;

    private float attackDuration = 0.5f; // currently unused
    private float moveSpeed = 1.0f; // currently unused

    private float returnSpeed = 1.0f;
    public float attackSpeed = 1.5f;

    public float anticiapationDuration = 0.5f;
    public float exitDuration = 0.5f;

    public bool isAttacking = false;


    // collision radius
    // we get the collision radius from the capsule collider that is placed on the characters
    private float attackThreshold = 0.25f;
    float myCollisionRadius;
    float targetCollisionRadius;


    public Action OnAttackStart;
    public Action OnDealDamage;
    public Action OnAttackFinish;
    public Action OnPlayAnticipation;

    // Use this for initialization
    void Start()
    {
        originalPosition = transform.position;
        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
    }

    public void SetTarget(GameObject _target)
    {
        target = _target;
    }

    public void SetTargetPosition(Vector3 _target)
    {
        targetPosition = _target;
    }

    void PlayAnticipationAnimation()
    {
        if (OnAttackStart != null)
        {
            OnAttackStart(); // event for animations and other effects
        }

    }

    // initialization of ability
    // play an animation
    // wait a few seconds then perform the attack
    public void Anticipation()
    {
        isAttacking = true;

        PlayAnticipationAnimation();
        StartCoroutine(Anticipate());
    }

    // this is where the lerp towards the enemy happens
    // or firing a projectile
    public void FollowThrough()
    {
        // TODO spawn hit animation
        Debug.Log("FollowThrough!");
        StartCoroutine(Lunge());
        //StartCoroutine(MoveAttackPos());
    }

    public void CompleteAttack()
    {

        StartCoroutine(FinishAttack());
    }

    IEnumerator Anticipate()
    {
        Debug.Log("PERFORMING ANTICIPATION...I'm about to smack you!");
        yield return new WaitForSeconds(anticiapationDuration);
        FollowThrough();
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
                //Vector3 targetPos = new Vector3(-7.91f, 1.57f, 1f);
                //   Debug.Log("lunged" + interpolate);
                //OVERLAP SPHERE FOR DAMAGE
                CheckForTarget(targetPosition);
            }

            yield return null;
        }


        yield return new WaitForSeconds(exitDuration);
        CompleteAttack();
    }


    IEnumerator FinishAttack()
    {
        // return to original position
        Vector3 startPos = transform.position;
        yield return StartCoroutine(LerpTowardsTarget(startPos, originalPosition, returnSpeed, exitDuration));

        isAttacking = false;
        if (OnAttackFinish != null)
        {

            OnAttackFinish();
        }

    }

    // utility to move towards our target --- REUSABLE CODE
    IEnumerator LerpTowardsTarget(Vector3 start, Vector3 end, float speed, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            transform.position = Vector3.Lerp(start, end, t / duration);
            t += Time.deltaTime * speed;
            yield return null;
        }
    }


    IEnumerator MoveAttackPos()
    {
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

        Vector3 startPosition = originalPosition;
        // we want to move to the target but stop just a bit before him
        Vector3 dirToTarget = (targetPosition - transform.position).normalized;
        Vector3 attackPosition = targetPosition - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackThreshold);


        yield return StartCoroutine(LerpTowardsTarget(originalPosition, attackPosition, 1, attackDuration));

        yield return new WaitForSeconds(attackDuration);
        StartCoroutine(Lunge());
    }

    void CheckForTarget(Vector3 targetPosition)
    {
        Collider[] targetCollider = Physics.OverlapSphere(targetPosition, targetCollisionRadius * 0.7f);

        for (int i = 0; i < targetCollider.Length; ++i)
        {
            Entity ent = targetCollider[i].GetComponent<Entity>();

            if (ent != null )//&& ent.myName.Equals("Player"))
            {
                if (OnDealDamage != null)
                {
                    OnDealDamage();
                }
            }
        }
    }
}


//IEnumerator MoveToAttackPosition()
//{
//    targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

//    Vector3 startPosition = originalPosition;
//    float t = 0;

//    Vector3 dirToTarget = (targetPosition - transform.position).normalized;
//    Vector3 newTarPos = targetPosition - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackThreshold);
//    while (t < attackDuration)
//    {
//        Debug.Log("PERFORMING LERP");
//        transform.position = Vector3.Lerp(originalPosition, newTarPos, t / attackDuration);
//        t += Time.deltaTime;
//        yield return null;
//    }

//    yield return new WaitForSeconds(0.3f);

//    StartCoroutine(Lunge());
//}

