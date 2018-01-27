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

	[Space]
	[Header("Items")]
	public float throwForce = 2;
	public Transform holdingLocation;
	public LayerMask rayMask;

	bool isItemHeld = false;

	Transform heldItem = null;

	void Start()
	{
		if(mouseLock)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	void Update()
	{
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, 100, rayMask))
		{
			if(Input.GetKeyDown(KeyCode.E))
			{
				if(hit.collider.GetComponent<Battery>() != null && Vector3.Distance(transform.position, hit.collider.transform.position) <= hit.collider.GetComponent<Battery>().radius)
				{
					heldItem = hit.collider.transform;
					heldItem.GetComponent<Battery> ().held = true;
					heldItem.GetComponent<Rigidbody> ().useGravity = false;
					heldItem.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
					heldItem.GetComponent<Rigidbody> ().velocity = Vector3.zero;
					heldItem.GetComponent<Rigidbody> ().detectCollisions = false;
					heldItem.transform.position = holdingLocation.transform.position;
					heldItem.transform.parent = holdingLocation;
					heldItem.transform.localRotation = transform.rotation;
					isItemHeld = true;
				}
			}
		}

		if(isItemHeld)
		{
			if(Input.GetMouseButtonUp(0))
			{
				heldItem.GetComponent<Rigidbody> ().useGravity = true;
				heldItem.GetComponent<Rigidbody> ().detectCollisions = true;
				holdingLocation.GetChild(0).gameObject.GetComponent<Rigidbody>().velocity = transform.forward * throwForce;
				holdingLocation.GetChild(0).parent = null;
				heldItem.GetComponent<Battery> ().held = false;
				isItemHeld = false;
				heldItem = null;
			}
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

		if (bobbing) HeadBob ();
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
