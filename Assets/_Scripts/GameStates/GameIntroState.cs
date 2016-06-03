using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// This will just be a title screen
/// maybe a shrink and grow animation
/// and a blinking 'press a key to start game'
/// </summary>
public class GameIntroState : IState
{
    GameController gameController;
    
    public GameIntroState()
    {

    }

    public GameIntroState(GameObject go)
    {
        gameController = go.GetComponent<GameController>();
    }

    public void OnEnter()
    {
        Debug.Log("OnEnter: Game Intro");
    }

    public void OnExit()
    {
        gameController.userInterFace.titleScreenGO.SetActive(false);
        gameController.userInterFace.mainMenuGO.SetActive(true);
        Debug.Log("OnExit: Game Intro");
    }

    public void OnUpdate()
    {
        if (Input.anyKeyDown)
        {
            gameController.ChangeState("GameMainMenu");
        }
        Debug.Log("OnUpdate: Game Intro");
    }
}
