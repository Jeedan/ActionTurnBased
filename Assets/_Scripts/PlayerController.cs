using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

public class PlayerController : MonoBehaviour
{
    GameController game;
    private Entity player;

    Entity target;

    public float dodgeSpeed = 2.0f;
    // Use this for initialization
    void Start()
    {
        player = GetComponent<Entity>();
        player.myName = "Player";
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
        if (target != null) //&& !target.basicAttack.isAttacking && !player.Dead())
        {

            player.PerformBasicAttack(target);
            
        }
    }

    public void Dodge()
    {
        Debug.Log("Dodged enemy attack!");
        StartCoroutine(DodgeUp());
    }

    public Entity GetPlayerEntity()
    {
        return player;
    }

    // lunge at target and then reset to the original position
    IEnumerator DodgeUp()
    {
        Vector3 startPos = transform.position;
        Vector3 dodgePos = transform.position + (transform.up * 3f);

        float percent = 0;
        while (percent <= 1)
        {
            percent += Time.deltaTime * dodgeSpeed;
            // this interpolates from 0 to 1 and then back from 1 to 0
            float interpolate = (-percent * percent + percent) * 4;
            transform.position = Vector3.Lerp(startPos, dodgePos, interpolate);
            yield return null;
        }
    }
}
