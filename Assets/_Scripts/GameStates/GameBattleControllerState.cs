using UnityEngine;
using System.Collections;

enum BattleStates
{
    INTRO,
    IDLE,
    PAUSED,
    ACTION,
    VICTORY,
    DEFEAT
};


public class GameBattleControllerState : IState
{
    GameController game;
    DungeonInfo dungeonInfo;


    Transform enemySpawn;

    GameObject enemyGO;
    Entity enemy;

    PlayerController playerController;

    BasicEnemy basicEnemy;

    Entity player;

    BattleStates currentBattleState;

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
        currentBattleState = BattleStates.INTRO;

        game.battleSceneContainer.SetActive(true);
        game.userInterFace.battleGUIGO.SetActive(true);

        // fade out from black to clear
        game.screenFader.InitFadeOut();

        InitEnemySpawn();
        player = game.playerController.gameObject.GetComponent<PlayerController>();

        if (!player.gameObject.activeInHierarchy)
        {
            player.gameObject.SetActive(true);
            player.Reset();
        }


        playerController = player as PlayerController;
        enemy = game.currentEnemy;
        enemy.gameObject.SetActive(true);
        basicEnemy = enemy.gameObject.GetComponent<BasicEnemy>();
        Debug.Log("Dungeon Info: " + dungeonInfo.dungeonName + " minLVL " + dungeonInfo.minLevel + " maxLVL " + dungeonInfo.maxLevel);
    }

    void InitEnemySpawn()
    {
        // TODO try and get a better randomizer
        var currDungeonLevel = dungeonInfo.dungeonLevel;
        var enemyPrefabChoiceIndex = 0;

        if (currDungeonLevel >= dungeonInfo.Enemies.Length)
        {
            Debug.Log("test hello ?");
            enemyPrefabChoiceIndex = Random.Range(0, dungeonInfo.Enemies.Length);
        }
        else
        {
            Debug.Log("random index");
            enemyPrefabChoiceIndex = 0;
        }

        enemyGO = Object.Instantiate(dungeonInfo.Enemies[enemyPrefabChoiceIndex]) as GameObject;
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

        EnemyAttack();

        if (!player.basicAttack.isAttacking)
        {
            playerController.HandleInput();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !player.basicAttack.isAttacking)
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
            // TODO CHANGE VICTORY STATE
            if (game.currentDungeon.nextDungeon != null)
            {
                game.currentDungeon.nextDungeon.unlocked = true;
            }
        }
        //Debug.Log("OnUpdate: Game Battle ");
    }

    void EnemyAttack()
    {
        if (!player.basicAttack.isAttacking)
        {
            // todo clean up this attack timer
            // TODO entities need states to tell what part of their ability they are currently in
           // BasicEnemy basicEnemy = enemy as BasicEnemy;
            basicEnemy.UpdateAttackTimer();

            if (!basicEnemy.basicAttack.isAttacking && basicEnemy.attackReady)
            {
                basicEnemy.PickRandomMove();
                basicEnemy.PerformBasicAttack(player);
            }
        }
    }

    void ExitBattle()
    {
        game.screenFader.FadeToggle();
        game.PopState("BattleScene");
    }
}
