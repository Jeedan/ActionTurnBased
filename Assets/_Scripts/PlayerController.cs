using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public Entity player;
    GameController game;

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
        if (target != null)
        {
            player.DealDamage(target, player.damage);
            
        }
    }

    public void Dodge()
    {
        Debug.Log("Dodged enemy attack!");
    }
}
