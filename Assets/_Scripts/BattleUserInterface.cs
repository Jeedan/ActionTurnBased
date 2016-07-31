using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class BattleUserInterface : MonoBehaviour
{
    public Button attackButton;
    public Button dodgeButton;

    public Button[] dodgeButtons;

    public Text attackBtnText;
    public Text dodgeBtnText;

    public PlayerController player;

    // TODO new HealthBar slider stuff
    GameController game;
    private Entity currentEnemy;

    public Slider playerHealthBar;
    public Slider enemyHealthBar;

    void OnEnable()
    {
        player = FindObjectOfType<PlayerController>();
        game = FindObjectOfType<GameController>();

        if (currentEnemy != null)
        {
            currentEnemy = game.currentEnemy;
        }
    }

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        attackButton = GameObject.Find("AttackBtn").GetComponent<Button>();

        attackBtnText = attackButton.GetComponentInChildren<Text>();

        // the 3 dodge buttons
        dodgeButtons[0] = GameObject.Find("dodgeUp").GetComponent<Button>();
        dodgeButtons[1] = GameObject.Find("dodgeLeft").GetComponent<Button>();
        dodgeButtons[2] = GameObject.Find("dodgeDown").GetComponent<Button>();


        InitializeButtonListeners(dodgeButtons[0], player.DodgeButtonUp);
        InitializeButtonListeners(dodgeButtons[1], player.DodgeButtonLeft);
        InitializeButtonListeners(dodgeButtons[2], player.DodgeButtonDown);

        player.OnDodgeStart += OnPlayerActionStart;
        player.OnDodgeEnd += OnPlayerActionEnd;

        InitializeButtonListeners(attackButton, player.AttackButton);

        player.GetPlayerEntity().basicAttack.OnAttackStart += OnPlayerActionStart;
        player.GetPlayerEntity().basicAttack.OnAttackFinish += OnPlayerActionEnd;

        player.basicAttack.OnDealDamage += UpdateEntityHealthBars;

        //currentEnemy.basicAttack.OnDealDamage += UpdateEntityHealthBars;

    }

    private void InitializeButtonListeners(Button btn, UnityAction listener)
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(listener);
    }

    private void OnPlayerActionStart()
    {
        UpdateButtonInteractivity(false);
    }

    private void OnPlayerActionEnd()
    {
        UpdateButtonInteractivity(true);
    }

    private void OnPlayerAttack()
    {
        UpdateButtonInteractivity(false);
    }

    private void OnPlayerDodge()
    {
        UpdateButtonInteractivity(false);
    }

    public void UpdateButtonInteractivity(bool value)
    {
        attackButton.interactable = value;
        for (int i = 0; i < dodgeButtons.Length; ++i)
        {
            dodgeButtons[i].interactable = value;
        }
    }

    private void UpdateEntityHealthBars()
    {
        if (player != null)
        {
            playerHealthBar.value = player.health;
        }

        if (currentEnemy != null)
        {
            enemyHealthBar.value = currentEnemy.health;
        }
    }
}
