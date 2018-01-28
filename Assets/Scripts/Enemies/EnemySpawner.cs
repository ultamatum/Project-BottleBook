using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemy;
	public float spawnDelay = 10;
	public float radius = 10;
	float spawnTimer = 0;

	void Start () 
	{
		
	}

	void Update () 
	{
		spawnTimer += Time.deltaTime;

		if(spawnTimer >= spawnDelay)
		{
			Vector3 randomPos = Random.insideUnitSphere * radius;
			randomPos.y *= 0;
			Vector3 spawnPosition = transform.position + randomPos;
			Instantiate (enemy, spawnPosition, Quaternion.identity, transform);
			spawnTimer = 0;
		}
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, radius);
	}
}
