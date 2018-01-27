using UnityEngine;

public class GunController : MonoBehaviour {

	public float damage = 10;
	public float fireRate = 15f;
	public float impactForce = 30f;
	public float range = 100f;

	public ParticleSystem muzzleFlash;
	public GameObject impactEffect;
	public LayerMask rayMask;

	private float nextFireTime = 0f;

	Camera camera;

	void Start()
	{
		camera = Camera.main;
	}

	void Update () 
	{
		if (Input.GetButton ("Fire1") && Time.time >= nextFireTime)
		{
			nextFireTime = Time.time + 1f / fireRate;
			Shoot ();
		}
	}

	void Shoot ()
	{
		muzzleFlash.Play ();

		RaycastHit hit;
		if(Physics.Raycast (camera.transform.position, camera.transform.forward, out hit, range, rayMask))
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
}
