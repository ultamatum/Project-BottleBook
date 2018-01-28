using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMotor))]
public class EnemyController : MonoBehaviour {

	public GameObject focus;

	public float health = 100;
	public float attackTimer;
	public float strength = 3;
	public float interactionRadius = 40;

	public GameObject[] goRelays;
	public GameObject goBase;

	EnemyMotor motor;

	float shortestDist = 0;
	GameObject closestTower = null;

	public AudioClip[] barks;
	public float barkFireRate = 20;
	private float nextBarkTime = 0f;

	void Start () 
	{
		focus = null;

		motor = GetComponent<EnemyMotor> ();

		goRelays = GameObject.FindGameObjectsWithTag ("Relay");

		goBase = GameObject.FindGameObjectWithTag ("Home Base");
	}

	void Update ()
	{
		if (focus == null)
		{
			for (int i = 0; i < goRelays.Length; i++)
			{
				float distance = Vector3.Distance (transform.position, goRelays [i].transform.position);
				if (distance < shortestDist || i == 0)
				{
					shortestDist = distance;
					closestTower = goRelays [i];
				} 
			}
			NewFocus (closestTower);
		}

		if (Vector3.Distance (transform.position, focus.transform.position) <= interactionRadius)
		{
			Attack ();
		}

		if(focus.GetComponent<Relay>())
		{
			if(focus.GetComponent<Relay>().health <= 0)
			{
				NewFocus (goBase);
			}
		}

		if (health <= 0)
		{
			Object.Destroy (gameObject);
		}

		if (Time.deltaTime >= nextBarkTime)
		{
			nextBarkTime = Time.deltaTime + 300000f; // Random.Range(0f, barkFireRate);
			Manager.instance.RandomizeBarks (barks);
		}
	}

	void Attack()
	{
		attackTimer += Time.deltaTime;
		if(attackTimer >= 1f)
		{
			if(focus.GetComponent<Relay>())
			{
				focus.GetComponent<Relay> ().Damage (strength);
			} else if (focus.GetComponent<HomeBase>())
			{
				focus.GetComponent<HomeBase> ().Damage (strength);
			}
			attackTimer = 0;
		}
	}

	void NewFocus(GameObject newFocus)
	{
		focus = newFocus;
		motor.FollowTarget (newFocus);
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
