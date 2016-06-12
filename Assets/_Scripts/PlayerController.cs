using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

public class PlayerController : MonoBehaviour
{
    GameController game;
    private Entity player;

    Entity target;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<Entity>();
        game = FindObjectOfType<GameController>();
    }
    
    // Update is called once per frame
    void Update()
    {
    }

    public void AttackButton()
    {
        Attack();
    }

    public void DodgeButton()
    {
        Dodge();
    }

    public void Attack()
    {
        target = game.currentEnemy;

        // TODO we should still be able to attack when the enemy is attacking 
        // but only while he is still in anticipation mode
        // if he is in followthrough mode we are out of luck 
        if (target != null && !target.basicAttack.isAttacking && !player.Dead())
        {

            player.OnAttackStart();
            // set the target's position
            player.basicAttack.SetTarget(target.gameObject);
            player.basicAttack.SetTargetPosition(target.transform.position);
            player.basicAttack.Anticipation();
           // player.DealDamage(target, player.damage);
            
        }
    }

    public void Dodge()
    {
        Debug.Log("Dodged enemy attack!");
    }

    public Entity GetPlayerEntity()
    {
        return player;
    }
}
