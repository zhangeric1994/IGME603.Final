using System.Collections.Generic;
using UnityEngine;

public class EnemyProxy : MonoBehaviour
{
    public int ID { get; private set; }

    private CombatManager level;
    private HashSet<PlayerExplorationController> players = new HashSet<PlayerExplorationController>();

    public void Initialize(int id)
    {
        ID = id;
    }

    public void StartCombat(PlayerExplorationController player)
    {
        if (!level)
            level = GameManager.Singleton.CreateCombat("Level/DebugLevel");
        level.enemyProxy = this;
        players.Add(player);

        level.SpawnPlayer(player.PlayerID);
    }

    public void EndCombat()
    {
        foreach (PlayerExplorationController player in players)
        {
            GameManager.Singleton.GetPlayerCombatController(player.PlayerID).gameObject.SetActive(false);
            player.EndCombat();
        }

        GameManager.Singleton.EndCombat(level);
        Destroy(gameObject);
    }
}
