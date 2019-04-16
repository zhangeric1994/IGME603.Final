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

    public void StartCombat(PlayerExplorationController player, string desiredLevel)
    {
        if (!level)
        {
            level = GameManager.Singleton.CreateCombat("Level/" + desiredLevel);

        }
        level.enemyProxy = this;
        players.Add(player);

        level.SpawnPlayer(player.PlayerID);


        AudioManager.Instance.PlaySoundEffect("Suck", DelayTime: 0.5f);
    }

    public void EndCombat()
    {
        foreach (PlayerExplorationController player in players)
        {
            GameManager.Singleton.GetPlayerCombatController(player.PlayerID).gameObject.SetActive(false);
            player.ReturnToExploration();
        }

        var items = GameObject.FindGameObjectsWithTag("Item");

        foreach (var item in items)
        {
            DestroyImmediate(item);
        }

        GameManager.Singleton.EndCombat(level);
        Destroy(gameObject);
    }

}
