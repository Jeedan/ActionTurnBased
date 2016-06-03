using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This is where we decide whether we go into a Dungeon to fight and level up
/// or if we want to buy and upgrade our character and look at our stats
/// </summary>
public class GameOverWorldState : IState
{
    public GameOverWorldState()
    {

    }

    public GameOverWorldState(GameObject go)
    {

    }

    public void OnEnter()
    {
        Debug.Log("OnEnter: Game OverWorld");
    }

    public void OnExit()
    {

        Debug.Log("OnExit: Game OverWorld");
    }

    public void OnUpdate()
    {
        Debug.Log("OnUpdate: Game OverWorld");
    }
}
