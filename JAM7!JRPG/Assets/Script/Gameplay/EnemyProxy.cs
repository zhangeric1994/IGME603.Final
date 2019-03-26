using System.Collections.Generic;
using UnityEngine;

public class EnemyProxy : MonoBehaviour
{
    public int ID { get; private set; }

    private HashSet<PlayerExplorationController> players = new HashSet<PlayerExplorationController>();

    public void Initialize(int id)
    {
        ID = id;
    }

    public void StartCombat(PlayerExplorationController player)
    {
        players.Add(player);
    }
}
