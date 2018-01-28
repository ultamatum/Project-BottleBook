using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMotor))]
public class EnemyController : MonoBehaviour {

	public Relay focus;

	public float health = 100;
	public float attackTimer;
	public float strength = 3;

	public Relay[] relays;

	EnemyMotor motor;

	float shortestDist = 0;
	Relay closestTower = null;

	public AudioClip[] barks;
	public float barkFireRate;
	private float nextBarkTime = 0f;

	void Start () 
	{
		focus = null;

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
		if(focus == null)
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

		if (Time.time >= nextBarkTime) {
		
			nextBarkTime = Time.time + 1f / barkFireRate;
			Manager.instance.RandomizeBarks (barks);
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

	void SetAttackFocus(Relay newFocus)
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
