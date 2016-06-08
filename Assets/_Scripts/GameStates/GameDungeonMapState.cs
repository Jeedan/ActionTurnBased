﻿using UnityEngine;
using System.Collections;
using System;

public class GameDungeonMapState : IState
{
    GameController game;

    public GameDungeonMapState()
    {

    }

    public GameDungeonMapState(GameObject go)
    {
        game = go.GetComponent<GameController>();
    }

    public void OnEnter()
    {
        // set the camera to orthographic 
        //as the map will be a 2D image with 2D images for the dungoen icons
        Camera.main.orthographic = true;
        game.dungeonMapContainer.SetActive(true);
        Debug.Log("OnEnter: Game DungeonMap");
    }

    public void OnExit()
    {
        // set the camera to be perspective again when we leave to a battle 
        // as the battle will be a 3D scene
        Camera.main.orthographic = false;
        game.dungeonMapContainer.SetActive(false);
        Debug.Log("OnExit: Game DungeonMap");
    }

    public void OnUpdate()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("OnUpdate: Game DungeonMap");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                GameObject dungeon = hitInfo.transform.gameObject;
                DungeonInfo dungeonInfo = dungeon.GetComponent<DungeonInfo>();
                if (dungeon.tag.Equals("Dungeon"))
                {

                    Debug.Log(dungeonInfo.dungeonName);

                    // TODO battle state test
                    IState BattleState = new GameBattleControllerState(game.gameObject, dungeonInfo);
                    game.AddState("BattleScene", BattleState);

                    game.ChangeState("BattleScene");
                    // TODO: change state to a dungeon
                    // dungeon will be only combat
                    // will be handled in battleController
                    // pass DungeonInfo via constructor
                    // create a battle state and we pass the battleState
                    // to gameController's stack of states
                    // on end of battle we will save all our information and pop the state
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // take us back to the overworld
            game.GoToOverWorld();
        }
    }
}