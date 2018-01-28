using UnityEngine;

public class Relay : MonoBehaviour 
{
	public float radius = 3f;
	public float maxHealth = 100;
	public float health = 100;
	public float decayAmount = 0.4f;
	public float zapEnergyUse = 5f;
	public float strength = 50;
	public float attackDelay = 3f;
	float attackTimer = 0;
	float decayTimer = 0;

	public bool alive = true;

	public GameObject shootFrom;

	public EnemyController target;

	Collider[] enemiesInRadius;

	void Update()
	{
		if (alive)
		{
			decayTimer += Time.deltaTime;
			attackTimer += Time.deltaTime;

			enemiesInRadius = Physics.OverlapSphere (transform.position, radius);

			for (int i = 0; i < enemiesInRadius.Length; i++)
			{
				GameObject enemy = enemiesInRadius [i].gameObject;
				if (enemy.GetComponent<EnemyController> () != null)
				{
					SetTarget (enemy);
				}
			}

			if (target != null)
			{
				Attack ();
			}

			if (decayTimer >= 1)
			{
				health -= decayAmount;
				decayTimer = 0;
			}

			if (health > maxHealth)
			{
				health = maxHealth;
			} else if (health < 0)
			{
				health = 0;
				alive = false;
			}
		}
	}

	void Attack()
	{

		if(attackTimer >= attackDelay)
		{
			health -= zapEnergyUse;
			target.Damage (strength);
			attackTimer = 0;
		}

		DrawLine (shootFrom.transform.position, target.transform.position + new Vector3(0,1,0), Color.red);
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
		health += amount;
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
