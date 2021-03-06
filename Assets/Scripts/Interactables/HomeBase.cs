﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBase : MonoBehaviour {

	public float batterySpawnDelay = 8f;
	public float health = 200;
	public float maxHealth = 200f;
	public float strength = 100;
	public float radius = 5f;
	public float attackDelay = 0.3f;
	public Transform shootFrom;
	public Transform batteryOutput;
	public Transform currentBattery = null;

	float spawnTimer;
	float attackTimer;

	EnemyController target;

	public GameObject battery;

	void Update () 
	{
		spawnTimer += Time.deltaTime;
		attackTimer += Time.deltaTime;

		Collider[] enemiesInRadius = Physics.OverlapSphere (transform.position, radius);
		float shortestDist = 10000;
		GameObject closestEnemy = null;

		for (int i = 0; i < enemiesInRadius.Length; i++)
		{
			GameObject enemy = enemiesInRadius [i].gameObject;
			if (enemy.GetComponent<EnemyController> () != null)
			{
				float distance = Vector3.Distance (transform.position, enemy.transform.position);
				if (distance < shortestDist || closestEnemy == null)
				{
					shortestDist = distance;
					closestEnemy = enemy;
				}
				SetTarget (enemy);
			}

		}

		if(target != null)
		{
			Attack ();
		}

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

	void Attack()
	{

		if(attackTimer >= attackDelay)
		{
			target.Damage (strength);
			attackTimer = 0;
		}

		DrawLine (shootFrom.transform.position, target.transform.position + new Vector3(0,1,0), Color.red);
	}

	void SetTarget (GameObject enemy)
	{
		target = enemy.GetComponent<EnemyController>();
	}

	void RemoveTarget ()
	{
		target = null;
	}

	public void Damage(float amount)
	{
		health -= amount;
	}

	public float BarHealth()
	{
		return health / maxHealth;
	}

	void DrawLine (Vector3 start, Vector3 end, Color color, float duration = 0.2f){

		GameObject myLine = new GameObject ();
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer> ();
		LineRenderer LR = myLine.GetComponent<LineRenderer> ();
		LR.material = new Material (Shader.Find ("Particles/Alpha Blended Premultiply"));
		LR.startColor = color;
		LR.endColor = color;
		LR.startWidth = 0.1f;
		LR.endWidth = 0.1f;
		LR.SetPosition (0, start);
		LR.SetPosition (1, end);
		GameObject.Destroy (myLine, duration);
	}
}
