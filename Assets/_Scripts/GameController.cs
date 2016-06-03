using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    StateMachine gameStateMachine;

    public GameUserInterface userInterFace;

    public GameUserInterface TitleScreen { get { return userInterFace; } private set { } }
    // Use this for initialization
    void Awake()
    {
        gameStateMachine = new StateMachine();
        userInterFace = gameObject.GetComponent<GameUserInterface>();
    }
    void Start()
    {
        InitializeStates();
    }
    void InitializeStates()
    {

        IState gameIntroState = new GameIntroState(gameObject);
        IState gameMainMenuState = new GameMainMenuState(gameObject);
        IState gameOverWorldState = new GameOverWorldState(gameObject);


        gameStateMachine.AddState("GameIntro", gameIntroState);
        gameStateMachine.AddState("GameMainMenu", gameMainMenuState);
        gameStateMachine.AddState("GameOverWorld", gameOverWorldState);

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

    // TODO this might need refactoring 
    // Main menu is more fickle than i thought
    public void PlayGameButton()
    {
        Debug.Log("PRESSED PLAY GAME");
        ChangeState("GameOverWorld");
    }
}
