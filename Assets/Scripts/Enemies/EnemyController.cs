using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMotor))]
public class EnemyController : MonoBehaviour {

	public Interactable focus;

	public float health = 100;
	public float attackTimer;
	public float strength = 3;

	public Interactable[] towers;

	EnemyMotor motor;

	float shortestDist = 0;
	Interactable closestTower = null;

	void Start () 
	{
		focus = null;

		motor = GetComponent<EnemyMotor> ();

		GameObject[] relays = GameObject.FindGameObjectsWithTag ("Relay");

		towers = new Interactable[relays.Length]; 

		for (int i = 0; i < relays.Length; i++)
		{
			towers [i] = relays [i].GetComponent<Interactable> ();
		}
	}

	void Update () 
	{
		if(focus == null)
		{
			for(int i = 0; i < towers.Length; i++)
			{
				float distance = Vector3.Distance (transform.position, towers[i].transform.position);
				if(distance < shortestDist || closestTower == null)
				{
					shortestDist = distance;
					closestTower = towers[i];
				}
			}

			SetAttackFocus (closestTower);
		}

		if(Vector3.Distance (transform.position, focus.transform.position) <= focus.radius)
		{
			Attack ();
		}

		if(health <= 0)
		{
			Object.Destroy (gameObject);
		}
	}

	void Attack ()
	{
		attackTimer += Time.deltaTime;
		if(attackTimer >= 1f)
		{
			focus.Damage (strength);
			attackTimer = 0;
		}
	}

	void SetAttackFocus(Interactable newFocus)
	{
		focus = newFocus;
		motor.FollowTarget (focus);
	}

	void RemoveAttackFocus()
	{
		focus = null;
		motor.StopFollowingTarget ();
	}

	public void Damage(float amount)
	{
		health -= amount;
	}
}
