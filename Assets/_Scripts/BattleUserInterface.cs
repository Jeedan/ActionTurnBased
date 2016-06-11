using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class BattleUserInterface : MonoBehaviour
{
    public Button attackButton;
    public Button dodgeButton;


    public Text attackBtnText;
    public Text dodgeBtnText;

    public PlayerController player;

    void OnEnable()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        attackButton = GameObject.Find("AttackBtn").GetComponent<Button>();
        dodgeButton = GameObject.Find("DodgeBtn").GetComponent<Button>();

        attackBtnText = attackButton.GetComponentInChildren<Text>();
        dodgeBtnText = dodgeButton.GetComponentInChildren<Text>();

        InitializeButtonListeners(attackButton, player.AttackButton);
        InitializeButtonListeners(dodgeButton, player.DodgeButton);


        player.GetPlayerEntity().OnAttackStart += OnPlayerAttack;
        player.GetPlayerEntity().OnAttackFinish += OnPlayerAttackFinish;
    }

    private void InitializeButtonListeners(Button btn, UnityAction listener)
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(listener);
    }

    private void OnPlayerAttack()
    {
        attackButton.interactable = false;
    }

    private void OnPlayerAttackFinish()
    {
        attackButton.interactable = true;
    }
}
