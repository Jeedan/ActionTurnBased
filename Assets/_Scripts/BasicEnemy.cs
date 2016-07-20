using UnityEngine;
using System.Collections;

public class BasicEnemy : MonoBehaviour
{
    Entity enemy;
    Animator enemyAnimator;

    // TODO very simple attack test
    public float attackTimer = 0;
    public bool attackReady = false;
    public float timeBetweenAttacks = 3.0f;

    // Use this for initialization
    void Start()
    {
        enemy = GetComponent<Entity>();
        enemyAnimator = GetComponent<Animator>();

        enemy.basicAttack.OnPlayAnticipation += PlayAnticipationAnimation;

        enemy.basicAttack.OnAttackStart += PlayAnticipationAnimation;
        enemy.basicAttack.OnAttackFinish += OnBasicAttackFinish;
    }

    void PlayAnticipationAnimation()
    {

        if (enemyAnimator)
        {
            enemyAnimator.SetTrigger("basic_Anticipation");
        }
    }

    void OnBasicAttackFinish()
    {
        Debug.Log("Basic Enemy is done attacking");
    }

    // todo move to an enemy AI class
    public void UpdateAttackTimer()
    {
        // TODO update the attack timer
        // attack timer should start at 0
        // it should charge up over time
        // attackTimer starts at 0
        // attackTimer += Time.DeltaTime;
        // when it has reached readyTime
        // 
        if (!enemy.basicAttack.isAttacking)
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
