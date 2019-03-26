/**
 * @author SerapH
 */

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The state of the game
/// </summary>
public enum GameState : int
{
    Start = 1,
    Loading,
    InGame,
}

/// <summary>
/// A FSM for the whole game at the highest level
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The unique instance
    /// </summary>
    public static GameManager Singleton { get; private set; }

    /// <summary>
    /// An event triggered whenever the state of the game changes
    /// </summary>
    public EventOnDataChange2<GameState> OnCurrentGameStateChange { get; private set; }

    [SerializeField] private GameState initialState = GameState.Start;

    private GameState currentGameState;

    /// <summary>
    /// The current state of the game
    /// </summary>
    public GameState CurrentGameState
    {
        get
        {
            return currentGameState;
        }

        private set
        {
            // Reset the current state
            if (value == currentGameState)
            {
#if UNITY_EDITOR
                Debug.Log(LogUtility.MakeLogStringFormat("GameManager", "{0} --> {0}", value));
#endif

                //switch (currentGameState)
                //{
                //}
            }
            else
            {
                // Before leaving the previous state
                //switch (currentGameState)
                //{
                //}

#if UNITY_EDITOR
                Debug.Log(LogUtility.MakeLogStringFormat("GameManager", "{0} --> {1}", value));
#endif

                GameState previousGameState = CurrentGameState;
                currentGameState = value;

                OnCurrentGameStateChange.Invoke(previousGameState, currentGameState);

                // After entering the new state
                switch (currentGameState)
                {
                    case GameState.Loading:
                        Player.CreatePlayer(0);
                        Player.CreatePlayer(1);
                        break;


                    case GameState.InGame:
                        GUIManager.Singleton.Open("HUD");
                        break;
                }
            }
        }
    }

    private GameManager() { }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    private void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);

            OnCurrentGameStateChange = new EventOnDataChange2<GameState>();
        }
        else if (this != Singleton)
            Destroy(gameObject);
    }

    private void Start()
    {
        CurrentGameState = initialState;
    }
}


