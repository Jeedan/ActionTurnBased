using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

public class PlayerController : Entity
{
    GameController game;

    //private Entity player;

    // public Entity targetTEST;

    public Action OnDodgeStart;
    public Action OnDodgeEnd;

    public float dodgeSpeed = 2.0f;
    public float dodgeDistance = 4.0f;

    

    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        //player = GetComponent<Entity>();
        //player.myName = "Player";
        myName = "Player";
        game = FindObjectOfType<GameController>();
        theSprite = GetComponentInChildren<Renderer>();
        origColor = theSprite.material.color;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            DodgeButtonUp();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            DodgeButtonLeft();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            DodgeButtonDown();
        }
    }

    public override void Reset()
    {
        base.Reset();

        if (OnDodgeEnd != null)
        {
            OnDodgeEnd();
        }
        theSprite.material.color = origColor;

        transform.position = battlePosition;

    }

    public void AttackButton()
    {
        Attack();
    }

    public void DodgeButtonUp()
    {
        Dodge(Vector3.up);
    }

    public void DodgeButtonLeft()
    {
        Dodge(Vector3.left);
    }

    public void DodgeButtonDown()
    {
        Dodge(Vector3.down);
    }

    public void Attack()
    {
        //targetTEST = game.currentEnemy;
        target = game.currentEnemy;
        //target = targetTEST;

        // TODO we should still be able to attack when the enemy is attacking 
        // but only while he is still in anticipation mode
        // if he is in followthrough mode we are out of luck 
        if (target != null) //&& !target.basicAttack.isAttacking && !player.Dead())
        {

            //player.PerformBasicAttack(target);
            PerformBasicAttack(target);

        }
    }

    private void Dodge(Vector3 direction)
    {
        Debug.Log("Dodged enemy attack!");
        StartCoroutine(DodgeTo(direction));
    }

    public Entity GetPlayerEntity()
    {
        return this;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        StartCoroutine(Flash());
    }

    // lunge at target and then reset to the original position
    IEnumerator DodgeTo(Vector3 direction)
    {
        if(transform.position != battlePosition)
        {
            yield break;
        }

        if (OnDodgeStart != null)
        {
            OnDodgeStart();
        }

        yield return TweenUtil.PingPong(direction, dodgeDistance, dodgeSpeed, transform);

        if (OnDodgeEnd != null)
        {
            transform.position = battlePosition;
            OnDodgeEnd();
        }
    }

    // TODO FLASH ON HURT
    public Renderer theSprite;
    public Color origColor;
    public Color flashColor;
    public int flashAmount = 3;
    public float flashTimer;
    public bool flashing;

    public void FlashColor()
    {
        flashing = !flashing;
        if (flashing)
        {
            theSprite.material.color = flashColor;
        }
        else
        {
            //theSprite.color = Color.Lerp(theSprite.color, origColor, flashTimer * Time.deltaTime);
            theSprite.material.color = origColor;

        }
    }

    IEnumerator Flash()
    {
        var t = 0;
        var flashes = flashAmount;
        while (t < flashes)
        {
            Debug.Log("Flashing " + flashing + " " + t);
            FlashColor();
            t++;
            yield return new WaitForSeconds(flashTimer);
        }

    }
}
