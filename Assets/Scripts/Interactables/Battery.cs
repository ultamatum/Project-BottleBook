using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour {

	public float radius = 5f;
	public float charge = 35;

	public bool held = false;

	void Start () 
	{
		
	}

	void Update () 
	{
		if(!held)
		{
			Collider[] relaysInRadius = Physics.OverlapSphere (transform.position, radius);

			for(int i = 0; i < relaysInRadius.Length; i++)
			{
				if(relaysInRadius[i].GetComponent<Relay>())
				{
					Transform relay = relaysInRadius [i].transform;
					Recharge (relay);
				}
			}
		}
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, radius);
	}

	void Recharge (Transform relay)
	{
		relay.GetComponent<Relay> ().Recharge (charge);
		Debug.Log ("zap");
		GameObject.Destroy (transform.gameObject);
	}
}
