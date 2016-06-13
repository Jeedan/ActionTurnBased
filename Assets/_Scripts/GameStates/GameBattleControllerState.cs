using UnityEngine;
using System.Collections;

public class GameBattleControllerState : IState
{
    GameController game;
    DungeonInfo dungeonInfo;


    Transform enemySpawn;

    GameObject enemyGO;
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

        // fade out from black to clear
        game.screenFader.InitFadeOut();

        InitEnemySpawn();
        player = game.playerController.gameObject.GetComponent<Entity>();

        if (!player.gameObject.activeInHierarchy)
        {
            player.gameObject.SetActive(true);
            player.Reset();
        }
        enemy = game.currentEnemy;
        Debug.Log("Dungeon Info: " + dungeonInfo.dungeonName + " minLVL " + dungeonInfo.minLevel + " maxLVL " + dungeonInfo.maxLevel);
    }

    void InitEnemySpawn()
    {
        enemyGO = Object.Instantiate(game.Enemies[0]) as GameObject;
        enemySpawn = GameObject.Find("_EnemyPosition").transform;
        enemyGO.transform.SetParent(enemySpawn);
        enemyGO.transform.position = enemySpawn.position;
        enemyGO.transform.rotation = enemySpawn.rotation;
        game.currentEnemy = enemyGO.GetComponent<Entity>();
    }

    public void OnExit()
    {
        // TODO SAVE OR RESET PLAYER AND ENEMY VALUES AND BATTLE STATE
        GameObject.Destroy(enemyGO);
        game.battleSceneContainer.SetActive(false);
        game.userInterFace.battleGUIGO.SetActive(false);
        Debug.Log("OnExit: Game Battle");
        
    }

    public void OnUpdate()
    {
        // we wait for the screen to fade before doing anything
        if (game.screenFader.inProgress) return;

        if (!player.basicAttack.isAttacking)
        {
            // todo clean up this attack timer
            // TODO entities need states to tell what part of their ability they are currently in
            enemy.UpdateAttackTimer();
            if (!enemy.basicAttack.isAttacking && enemy.attackReady)
            {
                Debug.Log("Enemy about to attack");
                enemy.PerformBasicAttack(player);

            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            game.screenFader.FadeToggle(ExitBattle);
            // game.ChangeState("GameDungeonMap");
        }

        // TODO temporary win and defeat condition
        if (player.Dead())
        {
            Debug.Log("Game Over player died");
            game.screenFader.FadeToggle(ExitBattle);
        }
        else if (enemy.Dead() && !player.basicAttack.isAttacking)
        {
            Debug.Log("Victory you defeated this very tough enemy lets go");
            game.screenFader.FadeToggle(ExitBattle);
        }
        //Debug.Log("OnUpdate: Game Battle ");
    }

    void ExitBattle()
    {
        game.screenFader.FadeToggle();
        game.PopState("BattleScene");
    }
}
