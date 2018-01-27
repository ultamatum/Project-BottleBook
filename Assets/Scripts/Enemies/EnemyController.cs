using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMotor))]
public class EnemyController : MonoBehaviour {

	public Transform[] towers = new Transform[5];

	EnemyMotor motor;

	void Start () 
	{
		motor = GetComponent<EnemyMotor> ();
	}

	void Update () 
	{
		float shortestDist = 10000;
		Vector3 closestTower = Vector3.zero;
		
		for(int i = 0; i < towers.Length; i++)
		{
			float distance = Vector3.Distance (transform.position, towers [i].position);
			if(distance < shortestDist)
			{
				shortestDist = distance;
				closestTower = towers [i].position;
			}
		}

		motor.MoveToPoint (closestTower);
	}
}
