using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldManager : MonoBehaviour
{
	[SerializeField]
	private Transform[] spawnPositions;
	[SerializeField]
	private int maxEnemies;
	[SerializeField]
	private GameObject[] enemySpawns;
	private List<GameObject> enemies;

    // Start is called before the first frame update
	void Start()
	{
		enemies = new List<GameObject>();
	}

	// Update is called once per frame
	void Update()
	{
		if(enemies.Count < maxEnemies) {
			enemies.Add(Instantiate(enemySpawns[Random.Range(0, enemySpawns.Length)], spawnPositions[Random.Range(0, spawnPositions.Length)].position, Quaternion.identity));
		}
    }
}
