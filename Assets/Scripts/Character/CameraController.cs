using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	[Header("Settings")]
	public bool mouseLock = true;
	public bool bobbing = true;
	public float mouseSensitivity = 6;

	[Space]
	[Header("Movement")]
	public Transform target;
	public float rotationSmoothTime = .02f;
	public Vector2 pitchMinMax = new Vector2 (-40, 85);

	float yaw;
	float pitch;

	Vector3 rotationSmoothVelocity;
	Vector3 CurrentRotation;

	[Space]
	[Header("Bobbing")]
	public float bobbingSpeed = 0.18f;
	public float bobbingAmount = 0.2f;
	public float midpoint = 2f;

	private float timer = 0.0f;

	void Start()
	{
		if(mouseLock)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	void LateUpdate () 
	{
		yaw += Input.GetAxis ("Mouse X") * mouseSensitivity;
		pitch -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
		pitch = Mathf.Clamp (pitch, pitchMinMax.x, pitchMinMax.y);

		CurrentRotation = Vector3.SmoothDamp (CurrentRotation, new Vector3 (pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
		transform.eulerAngles = CurrentRotation;

		transform.position = target.position;

		HeadBob ();
	}

	void HeadBob()
	{
		float waveslice = 0.0f;
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		Vector3 cSharpConversion = transform.localPosition;

		if(Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
		{
			timer = 0f;
		} else 
		{
			waveslice = Mathf.Sin (timer);
			timer += bobbingSpeed;
			if(timer > Mathf.PI * 2)
			{
				timer = timer - (Mathf.PI * 2);
			}
		}

		if(waveslice != 0)
		{
			float translateChange = waveslice * bobbingAmount;
			float totalAxes = Mathf.Abs (horizontal) + Mathf.Abs (vertical);
			totalAxes = Mathf.Clamp (totalAxes, 0.0f, 1.0f);
			translateChange = totalAxes * translateChange;
			cSharpConversion.y = midpoint + translateChange;
		}
		else 
		{
			cSharpConversion.y = midpoint;	
		}

		transform.localPosition = cSharpConversion;
	}
}
