using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    public bool isDead = false;
    public float health;
    private float maxHealth;

    public float damage = 1;
    // Use this for initialization
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (Dead())
        {
            Debug.Log("We dead");
            gameObject.SetActive(false);
        }
    }

    public void UpdateAttackTimer()
    {
        // TODO update the attack timer
        // attack timer should start at 0
        // it should charge up over time
        // attackTimer starts at 0
        // attackTimer += Time.DeltaTime;
        // when it has reached readyTime
        // 
        
    }

    public void DealDamage(Entity target, float dmg)
    {
        if (target != null && !target.Dead())
        {
            target.TakeDamage(dmg);
            Debug.Log("target took " + dmg);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;  
    }

    public bool Dead()
    {
        return health <= 0;
    }

    // if we use an object pooler we can use this function to reset our entities
    public void Reset()
    {
        health = maxHealth;
        isDead = false;
    }
}
