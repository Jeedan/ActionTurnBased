using UnityEngine;
using System.Collections;
using System;



public class GameMainMenuState : IState
{
    GameController game; 

    public GameMainMenuState()
    {

    }

    public GameMainMenuState(GameObject go)
    {
        game = go.GetComponent<GameController>();
    }

    public void OnEnter()
    {
        game.userInterFace.mainMenuGO.SetActive(true);
        Debug.Log("OnEnter: Game MainMenu");
    }

    public void OnExit()
    {
        game.userInterFace.mainMenuGO.SetActive(false);
        Debug.Log("OnExit: Game MainMenu");
    }

    public void OnUpdate()
    {
        Debug.Log("OnUpdate: Game MainMenu");
    }

}
