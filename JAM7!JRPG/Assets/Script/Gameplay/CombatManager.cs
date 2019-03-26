using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Vector3[] playerSpawnPoints;
    [SerializeField] private GameObject Door;
    public EnemyProxy enemyProxy;

    private int id = -1;

    public int ID
    {
        get
        {
            return id;
        }

        set
        {
            if (id < 0)
                id = value;
        }
    }

    public void SpawnPlayer(int id)
    {
        PlayerCombatController player = GameManager.Singleton.GetPlayerCombatController(id);
        player.Combat = this;
        player.transform.position = transform.position + playerSpawnPoints[Random.Range(0, playerSpawnPoints.Length)];
        player.gameObject.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (playerSpawnPoints != null)
            for (int i = 0; i < playerSpawnPoints.Length; i++)
                Gizmos.DrawSphere(transform.position + playerSpawnPoints[i], 0.1f);
    }

    void Update()
    {
        if (transform.Find("Enemy").childCount == 0)
        {
            Door.SetActive(true);
        }
    }

    public void endCombat() {
        enemyProxy.EndCombat();
    }
}
