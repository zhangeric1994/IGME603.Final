using System.Collections.Generic;
using UnityEngine;

public enum PlayerExplorationState
{
    Default = 0,
    Exploring = 1,
    InMenu = 2,
    InCombat = 3,
}

public class PlayerExplorationController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] float walkSpeed = 1;

    public int PlayerID { get; private set; }

    private PlayerExplorationState currentState;

    public PlayerExplorationState CurrentState
    {
        // this allowed to triggger codes when the state switched
        get
        {
            return currentState;
        }

        private set
        {
            if (value == currentState)
            {
                // nothing
            }
            else
            {
                switch (currentState)
                {
                    case PlayerExplorationState.InMenu:
                        HUD.Singleton.HideMenu(PlayerID);
                        break;


                    case PlayerExplorationState.InCombat:
                        HUD.Singleton.ShowExplorationUI(PlayerID);
                        HUD.Singleton.HideCombatUI(PlayerID);
                        gameObject.SetActive(true);
                        break;
                }

                PlayerExplorationState previousState = currentState;
                currentState = value;

                Debug.Log(LogUtility.MakeLogStringFormat("PlayerExplorationController", "{0} --> {1}", previousState, currentState));

                switch (currentState)
                {
                    case PlayerExplorationState.InMenu:
                        HUD.Singleton.ShowMenu(PlayerID);
                        break;


                    case PlayerExplorationState.InCombat:
                        HUD.Singleton.HideExplorationUI(PlayerID);
                        HUD.Singleton.ShowCombatUI(PlayerID);
                        gameObject.SetActive(false);
                        break;
                }
            }
        }
    }

    private PlayerExplorationController() { }

    public void Initialize(int id)
    {
        PlayerID = id;
    }

    public void StartCombat(EnemyProxy enemy)
    {
        CurrentState = PlayerExplorationState.InCombat;

        enemy.StartCombat(this);
    }

    public void EndCombat()
    {
        CurrentState = PlayerExplorationState.Exploring;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        switch (go.tag)
        {
            case "Enemy":
                StartCombat(go.GetComponent<EnemyProxy>());
                break;
        }
    }

    private void Start()
    {
        CurrentState = PlayerExplorationState.Exploring;
    }

    private void Update()
    {
        switch (currentState)
        {
            case PlayerExplorationState.Exploring:
                if (Input.GetButtonDown("Start" + PlayerID))
                    CurrentState = PlayerExplorationState.InMenu;
                else
                    transform.Translate(Time.deltaTime * walkSpeed * new Vector3(Input.GetAxis("Horizontal" + PlayerID), Input.GetAxis("Vertical" + PlayerID), 0));
                break;
        }
    }
}
