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

    private int playerID;

    private PlayerExplorationState currentState;

    public int PlayerID
    {
        get
        {
            return playerID;
        }

        set
        {
            if (playerID < 0)
                playerID = value;
        }
    }

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
                        HUD.Singleton.HideMenu(playerID);
                        break;


                    case PlayerExplorationState.InCombat:
                        HUD.Singleton.ShowExplorationUI(playerID);
                        HUD.Singleton.HideCombatUI(playerID);
                        break;
                }

                PlayerExplorationState previousState = currentState;
                currentState = value;

                Debug.Log(LogUtility.MakeLogStringFormat("PlayerExplorationController", "{0} --> {1}", previousState, currentState));

                switch (currentState)
                {
                    case PlayerExplorationState.InMenu:
                        HUD.Singleton.ShowMenu(playerID);
                        break;


                    case PlayerExplorationState.InCombat:
                        HUD.Singleton.HideExplorationUI(playerID);
                        HUD.Singleton.ShowCombatUI(playerID);
                        break;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                CurrentState = PlayerExplorationState.InCombat;
                break;
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case PlayerExplorationState.Exploring:
                if (Input.GetButtonDown("Start" + PlayerID))
                    CurrentState = PlayerExplorationState.InMenu;
                else
                    transform.Translate(walkSpeed * new Vector3(Input.GetAxis("Horizontal" + PlayerID), Input.GetAxis("Vertical" + PlayerID), 0));
                break;
        }
    }
}
