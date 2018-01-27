using UnityEngine;

public class Relay : MonoBehaviour 
{
	public float radius = 3f;
	public float health = 100;
	public float decayAmount = 0.4f;
	public float zapEnergyUse = 5f;
	public float strength = 50;
	public float attackDelay = 0.5f;
	float attackTimer = 0;
	float decayTimer = 0;

	public Transform shootFrom;

	EnemyController target;

	Collider[] enemiesInRadius;

	void Update()
	{
		decayTimer += Time.deltaTime;
		attackTimer += Time.deltaTime;

		enemiesInRadius = Physics.OverlapSphere (transform.position, radius);

		for(int i = 0; i < enemiesInRadius.Length; i++)
		{
			GameObject enemy = enemiesInRadius [i].gameObject;
			if(enemy.GetComponent<EnemyController>() != null)
			{
				SetTarget (enemy);
			}
		}

		if(target != null)
		{
			Attack ();
		}

		if(decayTimer >= 1) 
		{
			health -= decayAmount;
			decayTimer = 0;
		}
	}

	void Attack()
	{
		Debug.DrawLine (shootFrom.transform.position, target.transform.position);

		if(attackTimer >= attackDelay)
		{
			health -= zapEnergyUse;
			target.Damage (strength);
			attackDelay = 0;
		}
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, radius);
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

	public void Recharge(float amount)
	{
		Debug.Log ("Health before" + health);
		health += amount;
		Debug.Log ("Health after" + health);
	}
}
