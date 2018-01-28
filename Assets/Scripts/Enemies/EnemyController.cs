using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMotor))]
public class EnemyController : MonoBehaviour {

	public Relay relayFocus;
	public HomeBase baseFocus;

	public float health = 100;
	public float attackTimer;
	public float strength = 3;

	public Relay[] relays;

	EnemyMotor motor;

	bool targetFound = false;
	bool focusBase = false;

	float shortestDist = 0;
	Relay closestTower = null;

	public AudioClip[] barks;
	public float barkFireRate = 20;
	private float nextBarkTime = 0f;

	void Start () 
	{
		relayFocus = null;

		motor = GetComponent<EnemyMotor> ();

		GameObject[] goRelays = GameObject.FindGameObjectsWithTag ("Relay");

		relays = new Relay[goRelays.Length]; 

		for (int i = 0; i < relays.Length; i++)
		{
			relays [i] = goRelays [i].GetComponent<Relay> ();
		}
	}

	void Update () 
	{
		if(!focusBase)
		{
			if(!targetFound)
			{
				for(int i = 0; i < relays.Length; i++)
				{
					float distance = Vector3.Distance (transform.position, relays[i].transform.position);
					if(distance < shortestDist || closestTower == null)
					{
						shortestDist = distance;
						closestTower = relays[i];
					}
				}

				FocusRelay (closestTower);
				targetFound = true;
			}

			if(Vector3.Distance (transform.position, relayFocus.transform.position) <= relayFocus.radius)
			{
				AttackRelay ();
			}
				
			if(relayFocus.health <= 0)
			{
				focusBase = true;
				targetFound = false;
			}
		} else 
		{
			if(!targetFound)
			{
				baseFocus = GameObject.FindGameObjectWithTag ("Home Base").GetComponent<HomeBase> ();
				motor.FollowBase (baseFocus);
			}

			if(Vector3.Distance (transform.position, baseFocus.transform.position) <= baseFocus.radius)
			{
				AttackBase ();
			}
		}

		if(health <= 0)
		{
			Object.Destroy (gameObject);
		}

		if (Time.deltaTime >= nextBarkTime) {
			nextBarkTime = Time.deltaTime + 300000f; // Random.Range(0f, barkFireRate);
			Manager.instance.RandomizeBarks (barks);
		}
	}

	void AttackRelay ()
	{
		attackTimer += Time.deltaTime;
		if(attackTimer >= 1f)
		{
			relayFocus.Damage (strength);
			attackTimer = 0;
		}
	}

	void AttackBase ()
	{
		attackTimer += Time.deltaTime;
		if(attackTimer >= 1f)
		{
			baseFocus.Damage (strength);
			attackTimer = 0;
		}
	}

	void FocusRelay(Relay newFocus)
	{
		relayFocus = newFocus;
		motor.FollowRelay (relayFocus);
	}

	void RemoveAttackFocus()
	{
		relayFocus = null;
		motor.StopFollowingTarget ();
	}

	public void Damage(float amount)
	{
		health -= amount;
	}
}
