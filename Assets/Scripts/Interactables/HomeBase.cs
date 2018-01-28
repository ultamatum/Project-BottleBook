using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBase : MonoBehaviour {

	public float batterySpawnDelay = 8f;
	public float health = 200;
	public float maxHealth = 200f;
	public float radius = 5f;
	public Transform batteryOutput;
	public Transform currentBattery = null;

	float spawnTimer;
	public GameObject battery;

	void Update () 
	{
		spawnTimer += Time.deltaTime;

		if(spawnTimer >= batterySpawnDelay && currentBattery == null)
		{
			currentBattery = Instantiate (battery, batteryOutput.position, Quaternion.Euler(0,0,0)).transform;

			spawnTimer = 0;
		}

		if(health > maxHealth)
		{
			health = maxHealth;
		} else if (health < 0)
		{
			health = 0;
		}
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, radius);
	}

	public void Damage(float amount)
	{
		health -= amount;
	}
}
