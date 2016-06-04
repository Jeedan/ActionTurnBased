using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This is where we decide whether we go into a Dungeon to fight and level up
/// or if we want to buy and upgrade our character and look at our stats
/// </summary>
public class GameOverWorldState : IState
{
    GameController game;
    public GameOverWorldState()
    {

    }

    public GameOverWorldState(GameObject go)
    {
        game = go.GetComponent<GameController>();
    }

    public void OnEnter()
    {
        game.userInterFace.overWorldGO.SetActive(true);
        Debug.Log("OnEnter: Game OverWorld");
    }

    public void OnExit()
    {

        game.userInterFace.overWorldGO.SetActive(false);
        Debug.Log("OnExit: Game OverWorld");
    }

    public void OnUpdate()
    {
        Debug.Log("OnUpdate: Game OverWorld");
    }
}
