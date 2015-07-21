using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class CameraController : MonoBehaviour {

	private float cameraSpeed = 3.5f;
	public float rotationAngle = 45.0f;
	private Transform tr;

	// Use this for initialization
	void Start () {
		tr = GameObject.FindGameObjectWithTag ("DummyCamera").transform;
	}
	
	// Update is called once per frame
	void Update () {
		//Movement
		//This goes straight through walls
		Vector3 euler = tr.rotation.eulerAngles;
		if (Input.GetKey (KeyCode.W)) {
			tr.position += (tr.forward * cameraSpeed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.A)) {
			tr.position += (-tr.right * cameraSpeed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.S)) {
			tr.position += (-tr.forward * cameraSpeed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.D)) {
			tr.position += (tr.right * cameraSpeed * Time.deltaTime);
		}
		//Rotation
		if (Input.GetKeyDown (KeyCode.Q)) {
			tr.Rotate(Vector3.up * -rotationAngle, Space.World);
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			tr.Rotate(Vector3.up * +rotationAngle, Space.World);
		}
	}
}
