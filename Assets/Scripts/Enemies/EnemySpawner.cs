using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemy;

	void Start () 
	{
		
	}

	void Update () 
	{
		Instantiate (enemy, transform.position, Quaternion.identity, transform);
	}
}
