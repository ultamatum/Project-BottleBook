using UnityEngine;

public class GunController : MonoBehaviour {

	public float damage = 10;
	public float fireRate = 15f;
	public float impactForce = 30f;
	public float range = 100f;

	public ParticleSystem muzzleFlash;
	public GameObject impactEffect;
	public LayerMask rayMask;

	public	GameObject torus;
	public	GameObject torus2;
	public	GameObject torus3;
	public GameObject beamTarget;

	private float nextFireTime = 0f;

	void Update () 
	{
		if (Input.GetButtonUp ("Fire1") && Time.time >= nextFireTime)
		{
			nextFireTime = Time.time + 1f / fireRate;
			Shoot ();
			createBeam(torus, torus2, torus3, this.transform.position, beamTarget.transform.position, 1f, 3f, 10f);

		}
	}

	void Shoot ()
	{
		//muzzleFlash.Play ();



		RaycastHit hit;
		if(Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, range, rayMask))
		{
			EnemyController enemy = hit.transform.GetComponent<EnemyController> ();
			if(enemy != null)
			{
				enemy.Damage (damage);
			}

			if(hit.rigidbody != null)
			{
				hit.rigidbody.AddForce (-hit.normal * impactForce);
			}

			GameObject impactGO = Instantiate (impactEffect, hit.point, Quaternion.LookRotation (hit.normal));
			impactGO.transform.Rotate (90, 0, 0);
			Destroy (impactGO, 2f);
		}
	}

	void createBeam(GameObject obj, GameObject obj2, GameObject obj3, Vector3 pos, Vector3 pos2, float lifeTime, float moveSpeed, float growthSpeed){

		Vector3 offset2 = pos + this.gameObject.transform.forward ;
		Vector3 offset3 = pos + this.gameObject.transform.forward * 2;

		GameObject GO = Instantiate (obj, pos, this.gameObject.transform.rotation);
		GameObject GO2 = Instantiate (obj2, offset2, this.gameObject.transform.rotation);
		GameObject GO3 = Instantiate (obj3, offset3, this.gameObject.transform.rotation);
		Rigidbody rb = GO.GetComponent<Rigidbody> ();
		Rigidbody rb2 = GO2.GetComponent<Rigidbody> ();
		Rigidbody rb3 = GO3.GetComponent<Rigidbody> ();
		rb.velocity += GO.transform.forward * moveSpeed;
		rb2.velocity += GO2.transform.forward * moveSpeed;
		rb3.velocity += GO3.transform.forward * moveSpeed;

		Destroy (GO, lifeTime);
		Destroy (GO2, lifeTime);
		Destroy (GO3, lifeTime);

	}
}
