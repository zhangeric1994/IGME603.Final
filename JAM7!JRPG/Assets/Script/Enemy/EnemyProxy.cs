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

    public void StartCombat(PlayerExplorationController player, bool isBoss)
    {
        if (!level)
        {
            if (isBoss)
                level = GameManager.Singleton.CreateCombat("Level/BossLevel");
            else
                level = GameManager.Singleton.CreateCombat("Level/NormalLevel");
        }
        level.enemyProxy = this;
        players.Add(player);

        level.SpawnPlayer(player.PlayerID);
    }

    public void EndCombat()
    {
        foreach (PlayerExplorationController player in players)
        {
            GameManager.Singleton.GetPlayerCombatController(player.PlayerID).gameObject.SetActive(false);
            player.ReturnToExploration();
        }

        var items = GameObject.FindGameObjectsWithTag("Item");
        
        foreach (var item  in items)
        {
            DestroyImmediate(item);
        }
        
        GameManager.Singleton.EndCombat(level);
        Destroy(gameObject);
    }
}
