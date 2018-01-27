using UnityEngine;

public class GunController : MonoBehaviour {

	public float damage = 10;
	public float range = 100f;

	Camera camera;

	void Start()
	{
		camera = Camera.main;
	}

	void Update () 
	{
		if (Input.GetButtonDown ("Fire1"))
		{
			Shoot ();
		}
	}

	void Shoot ()
	{
		
	}
}
