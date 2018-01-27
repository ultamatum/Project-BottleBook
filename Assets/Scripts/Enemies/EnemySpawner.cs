using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemy;
	public float spawnDelay = 1;
	float spawnTimer = 0;

	void Start () 
	{
		
	}

	void Update () 
	{
		spawnTimer += Time.deltaTime;

		if(spawntimer >= spawnDelay)
		{
			Instantiate (enemy, transform.position, Quaternion.identity, transform);
			spawnTimer = 0;
		}
	}
}
