using UnityEngine;
using System.Collections;

public class BasicEnemy : MonoBehaviour
{
    Entity enemy;
    Animator enemyAnimator;

    // Use this for initialization
    void Start()
    {
        enemy = GetComponent<Entity>();
        enemyAnimator = GetComponent<Animator>();

        enemy.basicAttack.OnPlayAnticipation += PlayAnticipationAnimation;
    }

    void PlayAnticipationAnimation()
    {
        enemyAnimator.SetTrigger("basic_Anticipation");
    }

}
