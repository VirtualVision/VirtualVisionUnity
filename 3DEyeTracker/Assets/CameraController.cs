using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	private float cameraSpeed = 3.5f;
	public float rotationAngle = 45.0f;
	private Transform tr;
	
	// Use this for initialization
	void Start () {
		tr = GameObject.FindGameObjectWithTag ("MainCamera").transform;
	}
	
	// Update is called once per frame
	void Update () {
		//Movement
		CharacterController controller = GetComponent<CharacterController>();
		Vector3 moveDirection;
		Vector3 xDir = (tr.forward * Input.GetAxis("Vertical"));
		xDir.y = 0f;
		Vector3 zDir = (tr.right * Input.GetAxis ("Horizontal"));
		zDir.y = 0f;
		xDir.Normalize ();
		zDir.Normalize ();
		moveDirection = (new Vector3 (xDir.x + zDir.x, 0, zDir.z + xDir.z)).normalized;
		moveDirection *= cameraSpeed * Time.deltaTime;
		//Rotation
		if (Input.GetKeyDown (KeyCode.Q)) {
			tr.Rotate(Vector3.up * -rotationAngle, Space.World);
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			tr.Rotate(Vector3.up * +rotationAngle, Space.World);
		}
		controller.Move (moveDirection);
	}
}