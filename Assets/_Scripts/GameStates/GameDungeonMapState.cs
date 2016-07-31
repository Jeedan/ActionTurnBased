using UnityEngine;
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
       // Camera.main.orthographic = true;
        game.dungeonMapContainer.SetActive(true);
        Debug.Log("OnEnter: Game DungeonMap");
    }

    public void OnExit()
    {
        // set the camera to be perspective again when we leave to a battle 
        // as the battle will be a 3D scene
       // Camera.main.orthographic = false;
        game.dungeonMapContainer.SetActive(false);
        Debug.Log("OnExit: Game DungeonMap");
    }

    public void OnUpdate()
    {
        // we wait for the screen to fade before doing anything
        if (game.screenFader.inProgress) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {

                Debug.Log("OnUpdate: Game DungeonMap");
                // if we clicked on a dungeon icon set it as our gameobject
                GameObject dungeon = hitInfo.transform.gameObject;
                if (dungeon.tag.Equals("Dungeon"))
                {
                    // get the dungeon info from the clicked icon
                    DungeonInfo dungeonInfo = dungeon.GetComponent<DungeonInfo>();
                    CreateNewDungeon(dungeonInfo);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // take us back to the overworld
            game.PlayGameButton();
        }
    }

    // dungeon will be only combat
    // will be handled in battleController
    // pass DungeonInfo via constructor
    void CreateNewDungeon(DungeonInfo dungeonInfo)
    {

        Debug.Log(dungeonInfo.dungeonName);

        // TODO battle state test
        if (dungeonInfo.unlocked == true)
        {
            game.currentDungeon = dungeonInfo;

            IState BattleState = new GameBattleControllerState(game.gameObject, dungeonInfo);
            game.AddState("BattleScene", BattleState);

            game.screenFader.FadeToggle(FadeAndGoToBattleScene);
        }
    }

    void FadeAndGoToBattleScene()
    {

        game.ChangeState("BattleScene");
        game.screenFader.FadeToggle();
    }
}
