using UnityEngine;
using System.Collections;
using System;

public class MeleeAttack : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    private GameObject target;

    public float moveSpeed = 1.0f;
    public float attackSpeed = 1.5f;

    public float anticiapationDuration = 0.5f;
    public float attackDuration = 0.5f;
    public float exitDuration = 0.5f;

  //  public AnimationCurve TweenEase;

    public bool isAttacking = false;


    // collision radius
    public float attackThreshold = 0.25f;
    float myCollisionRadius;
    float targetCollisionRadius;


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
        if (OnPlayAnticipation != null)
        {
            OnPlayAnticipation(); // event for animations and other effects
        }

    }

    // initialization of ability
    // play an animation
    // wait a few seconds then perform the attack
    public void Anticipation()
    {
        // TODO start a coroutine
        isAttacking = true;

        PlayAnticipationAnimation();
        StartCoroutine(Anticipate());
    }

    public void FollowThrough()
    {
        // this is where the lerp towards the enemy happens
        // or firing a projectile
        // TODO spawn hit animation
        // TODO perform lerp

        StartCoroutine(Lunge());
        //StartCoroutine(MoveAttackPos());
    }
    public void CompleteAttack()
    {
        StartCoroutine(FinishAttack());
    }

    IEnumerator FinishAttack()
    {
        // return to original position
        Vector3 startPos = transform.position;
        yield return StartCoroutine(LerpTowardsTarget(startPos, originalPosition, moveSpeed, exitDuration));
        isAttacking = false;
        OnAttackFinish();
    }

    IEnumerator Anticipate()
    {
        Debug.Log("PERFORMING ANTICIPATION...I'm about to smack you!");
        yield return new WaitForSeconds(anticiapationDuration);
        FollowThrough();
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
            yield return null;
        }

        yield return new WaitForSeconds(exitDuration);
        CompleteAttack();
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

