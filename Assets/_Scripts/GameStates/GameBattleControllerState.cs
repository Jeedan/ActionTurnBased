using UnityEngine;
using System.Collections;

public class GameBattleControllerState : IState
{
    GameController game;
    DungeonInfo dungeonInfo;
    public GameBattleControllerState()
    {

    }

    public GameBattleControllerState(GameObject go)
    {
        game = go.GetComponent<GameController>();
    }

    public GameBattleControllerState(GameObject go, DungeonInfo dungeonInfo)
    {
        game = go.GetComponent<GameController>();
        this.dungeonInfo = dungeonInfo;
    }

    public void OnEnter()
    {

        game.battleSceneContainer.SetActive(true);
        Debug.Log("OnEnter: Game Battle");
        Debug.Log("Dungeon Info: " + dungeonInfo.dungeonName + " minLVL " + dungeonInfo.minLevel + " maxLVL " +dungeonInfo.maxLevel);
    }

    public void OnExit()
    {
        // game.userInterFace.titleScreenGO.SetActive(false);
        game.battleSceneContainer.SetActive(false);
        Debug.Log("OnExit: Game Battle");
    }

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            game.PopState("BattleScene");
            // game.ChangeState("GameDungeonMap");
        }

        Debug.Log("OnUpdate: Game Battle ");
    }
}
