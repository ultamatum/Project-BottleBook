using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMotor : MonoBehaviour {

	public Transform target;
	public float stoppingDistMod = 0.6f;
	NavMeshAgent agent;
	public Animator anim;

	float speed;

	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
		anim = GetComponentInChildren <Animator> ();
	}

	void Update ()
	{
		if(target != null)
		{
			Debug.Log (target);
			agent.SetDestination (target.position);
			FaceTarget ();
		}

		speed = agent.velocity.magnitude;

		anim.SetFloat ("Speed", speed);
	}

	public void MoveToPoint (Vector3 point)
	{
		agent.SetDestination (point);
	}

	public void FollowRelay (Relay newTarget)
	{
		agent.stoppingDistance = newTarget.radius * stoppingDistMod;
		agent.updateRotation = false;

		target = newTarget.transform;
	}

	public void FollowBase (HomeBase newTarget)
	{
		agent.stoppingDistance = newTarget.radius * stoppingDistMod;
		agent.updateRotation = false;

		target = newTarget.transform;
	}

	public void StopFollowingTarget ()
	{
		agent.stoppingDistance = 0;
		agent.updateRotation = true;

		target = null;
	}

	void FaceTarget()
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation (new Vector3 (direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
	}
}
