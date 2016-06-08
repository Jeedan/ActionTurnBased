using UnityEngine;
using System.Collections;

public class GameBattleControllerState : IState
{
    GameController game;
    DungeonInfo dungeonInfo;


    Transform enemySpawn;

    Entity enemy;
    Entity player;

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
        Debug.Log("OnEnter: Game Battle");
        game.battleSceneContainer.SetActive(true);
        game.userInterFace.battleGUIGO.SetActive(true);

        InitEnemySpawn();
        player = game.playerController.gameObject.GetComponent<Entity>();
        enemy = game.currentEnemy;
        Debug.Log("Dungeon Info: " + dungeonInfo.dungeonName + " minLVL " + dungeonInfo.minLevel + " maxLVL " +dungeonInfo.maxLevel);
    }

    void InitEnemySpawn()
    {
        GameObject enemyGO = Object.Instantiate(game.Enemies[0]) as GameObject;
        enemySpawn = GameObject.Find("_EnemyPosition").transform;
        enemyGO.transform.SetParent(enemySpawn);
        enemyGO.transform.position = enemySpawn.position;
        enemyGO.transform.rotation = enemySpawn.rotation;
        game.currentEnemy = enemyGO.GetComponent<Entity>();
    }

    public void OnExit()
    {
        game.battleSceneContainer.SetActive(false);
        game.userInterFace.battleGUIGO.SetActive(false);
        Debug.Log("OnExit: Game Battle");
    }

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            game.PopState("BattleScene");
            // game.ChangeState("GameDungeonMap");
        }

        if(player.Dead())
        {
            Debug.Log("Game Over player died");
            game.PopState("BattleScene");
        }
        else if (enemy.Dead())
        {
            Debug.Log("Victory you defeated this very tough enemy lets go");
            game.PopState("BattleScene");
        }
        //Debug.Log("OnUpdate: Game Battle ");
    }
}
