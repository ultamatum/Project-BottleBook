using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public bool mouseLock = true;

	public float mouseSensitivity = 6;
	public float rotationSmoothTime = .02f;
	float yaw;
	float pitch;

	public Vector2 pitchMinMax = new Vector2 (-40, 85);

	Vector3 rotationSmoothVelocity;
	Vector3 CurrentRotation;

	public Transform target;

	void Start()
	{
		if(mouseLock)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	void LateUpdate () {
		yaw += Input.GetAxis ("Mouse X") * mouseSensitivity;
		pitch -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
		pitch = Mathf.Clamp (pitch, pitchMinMax.x, pitchMinMax.y);

		CurrentRotation = Vector3.SmoothDamp (CurrentRotation, new Vector3 (pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
		transform.eulerAngles = CurrentRotation;

		transform.position = target.position;
	}
}
