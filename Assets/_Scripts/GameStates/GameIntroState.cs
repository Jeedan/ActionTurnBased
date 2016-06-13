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
    GameController game;

    public GameIntroState()
    {

    }

    public GameIntroState(GameObject go)
    {
        game = go.GetComponent<GameController>();
    }

    public void OnEnter()
    {
        game.userInterFace.titleScreenGO.SetActive(true);
        Debug.Log("OnEnter: Game Intro");

        // fade to clear
        game.screenFader.FadeToggle();
    }

    public void OnExit()
    {
        game.userInterFace.titleScreenGO.SetActive(false);
        Debug.Log("OnExit: Game Intro");
        // fade to clear
    }

    public void OnUpdate()
    {
        // we wait for the screen to fade before doing anything
        if (game.screenFader.inProgress) return;

        if (Input.anyKeyDown)
        {
            game.screenFader.FadeToggle(GoToMainMenu);
        }
        //  Debug.Log("OnUpdate: Game Intro");
    }

    void GoToMainMenu()
    {
        game.ChangeState("GameMainMenu");
        // fade to clear
        game.screenFader.FadeToggle();
    }
}
