using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float walkSpeed = 2;
	public float runSpeed = 6;
	public float gravity = -12;
	public float jumpHeight = 1;
	public float turnSmoothTime = 0.2f;
	public float speedSmoothTime = 0.1f;
	float velocityY;
	float turnSmoothVel;
	float speedSmoothVel;
	float currentSpeed;

	Transform mainCamera;
	CharacterController controller;

	void Start () 
	{
		mainCamera = Camera.main.transform;
		controller = GetComponent<CharacterController> ();
	}

	void Update () 
	{
		Vector2 rawInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		Vector2 input = rawInput.normalized;

		bool isRunning = Input.GetKey (KeyCode.LeftShift);

		Move (input, isRunning);

		if(Input.GetKeyDown(KeyCode.Space))
		{
			Jump ();
		}

	}

	void Move(Vector2 input, bool isRunning)
	{
		if (input != Vector2.zero) {
			float targetRotation = Mathf.Atan2 (input.x, input.y) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
			if((targetRotation - transform.eulerAngles.y) > 170 || ((transform.eulerAngles.y - targetRotation) > 170))
			{
				transform.eulerAngles += Vector3.up * targetRotation;
			} else
			{
				transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVel, turnSmoothTime);
			}
		}

		float targetSpeed = ((isRunning) ? runSpeed : walkSpeed) * input.magnitude;
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVel, speedSmoothTime);

		velocityY += Time.deltaTime * gravity;

		Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

		controller.Move (velocity * Time.deltaTime);
		currentSpeed = new Vector2 (controller.velocity.x, controller.velocity.z).magnitude;

		if (controller.isGrounded)
			velocityY = 0;
	}

	void Jump()
	{
		if(controller.isGrounded)
		{
			float jumpVelocity = Mathf.Sqrt (-2 * gravity * jumpHeight);
			velocityY = jumpVelocity;
		}
	}
}
