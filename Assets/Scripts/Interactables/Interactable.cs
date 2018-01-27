using UnityEngine;

public class Interactable : MonoBehaviour 
{
	public float radius = 3f;
	public float health = 100;
	public float decayAmount = 0.4f;
	public float strength = 3;
	float decayTimer = 0;

	public Transform shootFrom;

	EnemyController target;

	Collider[] enemiesInRadius;

	void Update()
	{
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
		health -= 0.1f;
		target.Damage (strength);
		if (target.health <= 0)
			target = null;
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
		
	}
}
