using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    StateMachine gameStateMachine;

    public GameUserInterface userInterFace;
    public GameObject dungeonMapContainer; // container of our dungeon map "scene"
    public GameObject battleSceneContainer; // container of our battle scene

    public PlayerController playerController;

    // right now we will instantiate an enemy in BattleScene and pass it to this value
    public Entity currentEnemy;

    public GameObject[] Enemies;
    // Use this for initialization
    void Awake()
    {
        gameStateMachine = new StateMachine();
        userInterFace = gameObject.GetComponent<GameUserInterface>();

        playerController = FindObjectOfType<PlayerController>();
    }

    void Start()
    {
        dungeonMapContainer.SetActive(false);
        battleSceneContainer.SetActive(false);
        InitializeStates();
    }

    void InitializeStates()
    {

        IState gameIntroState = new GameIntroState(gameObject);
        IState gameMainMenuState = new GameMainMenuState(gameObject);
        IState gameOverWorldState = new GameOverWorldState(gameObject);
        IState gameDungeonMapState = new GameDungeonMapState(gameObject);


        gameStateMachine.AddState("GameIntro", gameIntroState);
        gameStateMachine.AddState("GameMainMenu", gameMainMenuState);
        gameStateMachine.AddState("GameOverWorld", gameOverWorldState);
        gameStateMachine.AddState("GameDungeonMap", gameDungeonMapState);
        
        gameStateMachine.ChangeState("GameIntro");
    }

    // Update is called once per frame
    void Update()
    {
        gameStateMachine.OnUpdate();
    }

    public void ChangeState(string _state)
    {
        gameStateMachine.ChangeState(_state);
    }

    public void AddState(string _name, IState _state)
    {
        gameStateMachine.AddState(_name, _state);
    }

    public void PopState(string key)
    {
        gameStateMachine.PopState(key);
    }
    // TODO this might need refactoring 
    // Main menu is more fickle than i thought
    public void PlayGameButton()
    {
        Debug.Log("PRESSED PLAY GAME");
        GoToOverWorld();
    }

    public void GoToOverWorld()
    {
        ChangeState("GameOverWorld");
    }

    public void GoToDungeonMapButton()
    {
        // DUNGEONS NEED THEIR OWN STATE
        // each dungeon would be its own state?
        // load level data from what dungeon was clicked?

        Debug.Log("PRESSED DUNGEON MAP");
        ChangeState("GameDungeonMap");
    }
}
