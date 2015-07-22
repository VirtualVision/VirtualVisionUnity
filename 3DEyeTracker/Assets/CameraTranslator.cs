using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class CameraTranslator : MonoBehaviour {
	
	private float cameraSpeed = 3.5f;
	public float rotationAngle = 45.0f;
	private Transform tr;
	private Transform camTr;
	
	// Use this for initialization
	void Start () {
		tr = GameObject.FindGameObjectWithTag ("DummyCamera").transform;
		camTr = GameObject.FindGameObjectWithTag ("MainCamera").transform;
	}
	
	// Update is called once per frame
	void Update () {
		//Movement
		//These will stop at bounds of walls(hardcoded)
		//no movement in y direction
		if (Input.GetKey (KeyCode.W)) {
			Vector3 temp = vecConvert(camTr.forward);
			temp.y = 1.5f;
			temp.x = Mathf.Clamp((tr.position.x +(temp.x * cameraSpeed * Time.deltaTime)), -9.5f, 9.5f);
			temp.z = Mathf.Clamp((tr.position.z +(temp.z * cameraSpeed * Time.deltaTime)), -9.5f, 9.5f);
			tr.position = temp;
		}
		if (Input.GetKey (KeyCode.A)) {
			Vector3 temp = vecConvert(-camTr.right);
			temp.y = 1.5f;
			temp.x = Mathf.Clamp((tr.position.x + (temp.x * cameraSpeed * Time.deltaTime)), -9.5f, 9.5f);
			temp.z = Mathf.Clamp((tr.position.z + (temp.z * cameraSpeed * Time.deltaTime)), -9.5f, 9.5f);
			tr.position = temp;
		}
		if (Input.GetKey (KeyCode.S)) {
			Vector3 temp = vecConvert(-camTr.forward);
			temp.y = 1.5f;
			temp.x = Mathf.Clamp((tr.position.x + (temp.x * cameraSpeed * Time.deltaTime)), -9.5f, 9.5f);
			temp.z = Mathf.Clamp((tr.position.z + (temp.z * cameraSpeed * Time.deltaTime)), -9.5f, 9.5f);
			tr.position = temp;
		}
		if (Input.GetKey (KeyCode.D)) {
			Vector3 temp = vecConvert(camTr.right);
			temp.y = 1.5f;
			temp.x = Mathf.Clamp((tr.position.x + (temp.x * cameraSpeed * Time.deltaTime)), -9.5f, 9.5f);
			temp.z = Mathf.Clamp((tr.position.z + (temp.z * cameraSpeed * Time.deltaTime)), -9.5f, 9.5f);
			tr.position = temp;
		}
		//Rotation
		if (Input.GetKeyDown (KeyCode.Q)) {
			tr.Rotate(Vector3.up * -rotationAngle, Space.World);
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			tr.Rotate(Vector3.up * +rotationAngle, Space.World);
		}
	}
	//
	Vector3 vecConvert(Vector3 v){
		return (new Vector3 (v.x, 0, v.z)).normalized;	}
	
}