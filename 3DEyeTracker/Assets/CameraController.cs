using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private float cameraSpeed = 1.5f;
	public int rotationAngle = 45;
	private Transform tr;

	// Use this for initialization
	void Start () {
		tr = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		//Movement
		//This goes straight through walls
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
			transform.Rotate (0,-rotationAngle,0);
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			transform.Rotate (0,rotationAngle,0);
		}
	}
}
