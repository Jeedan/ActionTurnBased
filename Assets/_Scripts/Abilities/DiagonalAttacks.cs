using UnityEngine;
using System.Collections;

[System.Serializable]
public class DiagonalAttacks : Abilities
{
    private float moveSpeed = 1.0f; // currently unused

   // public Entity caster; // the entity that is attacking / using this ability
   

    public override void Initialize(Transform transform, CapsuleCollider collisionRadius, Entity owner)
    {
        this.transform = transform;
        myCollisionRadius = collisionRadius.radius;
        this.owner = owner;

        mName = "DiagonalAttack";

        //owner = GetComponent<Entity>();
        originalPosition = owner.battlePosition;
    }


    // Use this for initialization
    protected override void Start()
    {
        //caster = GetComponent<Entity>();
        //originalPosition = caster.battlePosition;
        //myCollisionRadius = GetComponent<CapsuleCollider>().radius;
    }

    public override IEnumerator Anticipate()
    {
        Debug.Log("PERFORMING ANTICIPATION...I'm about to smack you!");
        yield return new WaitForSeconds(anticiapationDuration);
        FollowThrough();
    }


    public override IEnumerator Attack()
    {
        yield return FakeUp();
    }

    IEnumerator FakeUp()
    {
        Vector3 startPos = owner.battlePosition;
        yield return TweenUtil.LerpTowardsTarget(startPos, startPos + (Vector3.up * 2.5f), attackSpeed, 0.5f, transform);
        // flash a bit just before we attack
        PlayAnticipationAnimation();
        yield return new WaitForSeconds(attackDelay);
        yield return Lunge();
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

    public override IEnumerator FinishAttack()
    {
        // return to original position
        Vector3 currentPos = transform.position;
        //yield return StartCoroutine(LerpTowardsTarget(startPos, originalPosition, returnSpeed, exitDuration));
        yield return TweenUtil.LerpTowardsTarget(currentPos, originalPosition, returnSpeed, exitDuration, transform);

        isAttacking = false;
        if (OnAttackFinish != null)
        {

            OnAttackFinish();
        }
    }
}
