using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	private float cameraSpeed = 3.5f;
	public float rotationAngle = 45.0f;
	private Transform tr;
	private Transform dummy;
	
	// Use this for initialization
	void Start () {
		tr = GameObject.FindGameObjectWithTag ("MainCamera").transform;
		dummy = GameObject.FindGameObjectWithTag ("DummyCamera").transform;
	}
	
	// Update is called once per frame
	void Update () {
		//Movement
		CharacterController controller = GetComponent<CharacterController> ();
		Vector3 moveDirection;
		Vector3 xDir = (tr.forward * Input.GetAxis ("Vertical"));
		xDir.y = 0f;
		Vector3 zDir = (tr.right * Input.GetAxis ("Horizontal"));
		zDir.y = 0f;
		xDir.Normalize ();
		zDir.Normalize ();
		moveDirection = (new Vector3 (xDir.x + zDir.x, 0, zDir.z + xDir.z)).normalized;
		moveDirection *= cameraSpeed * Time.deltaTime;
		//Rotation
		if (Input.GetKeyDown (KeyCode.Q)) {
			dummy.Rotate (Vector3.up * -rotationAngle, Space.World);
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			dummy.Rotate (Vector3.up * +rotationAngle, Space.World);
		}

		dummy.Rotate (Vector3.up * rotationAngle * Input.GetAxis ("Desktop_Right_X_Axis") * Time.deltaTime, Space.World);
		//Do not move up/down with right stick on controller
		//Vector3 rotateAxis = tr.right;
		//rotateAxis.y = 0f;
		//dummy.Rotate (rotationAngle * Input.GetAxis ("Desktop_Right_Y_Axis") * Time.deltaTime, 0, 0);
		controller.Move (moveDirection);
	}
}