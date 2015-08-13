using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Threading;
using UnityEngine.VR;
using NetMQ;

public class HUDImageScript : MonoBehaviour {
	

	public int calCount;
	public int curCount;
	public Sprite[] calImages;
	public Sprite[] secImages;

	public Transform tr;
	public Vector3 vec;
	public Ray ray;
	public float transX;
	public float transY;
	public bool calibrated;
	public float textTime;
	public int trackingMode;

	private NetMQContext context;
	private NetMQSocket calibrator; 
	private NetMQSocket client; 

	private GameObject targetPrefab;
	private Text uiText;

	// initialized sprites
	//
	void Start () {
		AsyncIO.ForceDotNet.Force();
		calImages = new Sprite[10];
		calImages[0] = Resources.Load<Sprite>("notargets");
		calImages[1] = Resources.Load<Sprite>("targetTL");
		calImages[2] = Resources.Load<Sprite>("targetTM");
		calImages[3] = Resources.Load<Sprite>("targetTR");
		calImages[4] = Resources.Load<Sprite>("targetML");
		calImages[5] = Resources.Load<Sprite>("targetMM");
		calImages[6] = Resources.Load<Sprite>("targetMR");
		calImages[7] = Resources.Load<Sprite>("targetBL");
		calImages[8] = Resources.Load<Sprite>("targetBM");
		calImages[9] = Resources.Load<Sprite>("targetBR");

		secImages = new Sprite[9];
		secImages [0] = Resources.Load<Sprite> ("sectionTL");
		secImages [1] = Resources.Load<Sprite> ("sectionTM");
		secImages [2] = Resources.Load<Sprite> ("sectionTR");
		secImages [3] = Resources.Load<Sprite> ("sectionML");
		secImages [4] = Resources.Load<Sprite> ("sectionMM");
		secImages [5] = Resources.Load<Sprite> ("sectionMR");
		secImages [6] = Resources.Load<Sprite> ("sectionBL");
		secImages [7] = Resources.Load<Sprite> ("sectionBM");
		secImages [8] = Resources.Load<Sprite> ("sectionBR");


		calCount = 0;
		curCount = 0;
		gameObject.GetComponent<Image> ().sprite = calImages [calCount];
		//max coords 1180*564
		transX = 0;
		transY = 0;
		textTime = -2.0f;
		targetPrefab = GameObject.FindGameObjectWithTag ("RaycastTarget");
		uiText = GameObject.FindGameObjectWithTag ("UIText").GetComponent<Text> ();;
		targetPrefab.SetActive (false);
		targetPrefab.layer = 2;//ignore raycast layer
		calibrated = false;
		trackingMode = 0;

		AsyncIO.ForceDotNet.Force();
		//setup sockets
		//hangs????
		//http://forum.unity3d.com/threads/netmq-basic.298104/
		//must compile myself
		//https://github.com/zeromq/netmq/issues/98
		context = NetMQContext.Create ();
		calibrator = context.CreatePublisherSocket ();
		calibrator.Bind("tcp://127.0.0.1:5567");
		client = context.CreateSubscriberSocket ();
		client.Connect("tcp://127.0.0.1:5568");
		client.Subscribe ("ScreenPoints");
		Debug.Log (System.Environment.Version);
		/*server.SendMore("coord").Send ("200 200");
		string top = client.ReceiveString ();
		string message = client.ReceiveString ();
		Debug.Log (message);
		string[] coord = message.Split ();
		transX = int.Parse (coord [0]);
		transY = int.Parse (coord [1]);*/
	}
	
	// Update is called once per frame
	// 
	void Update () {
		AsyncIO.ForceDotNet.Force();
		string top;
		string message;
		if (client.TryReceiveFrameString (out top)) {
			if(client.TryReceiveFrameString (out message)){
				Debug.Log (message);
				string[] coord = message.Split ();
				transX = int.Parse (coord [0]);
				transY = int.Parse (coord [1]);
				calibrated = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown (KeyCode.JoystickButton0)){
			calCount++;
			textTime = 5.0f;
			if (calCount != 1) {
				calibrator.Send ("Calibrate");
			}
			else{
				calibrated = false;
				trackingMode = 0;
				targetPrefab.SetActive(false);
			}
			if(calCount == 10){
				calibrated = true;
				targetPrefab.SetActive(true);
				uiText.text = "Press A to recalibrate" + Environment.NewLine + "Press B to toggle eye tracking modes";
				textTime = 0.0f;
				trackingMode = 1;
			}
		}
		if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown (KeyCode.JoystickButton1)){
			if (calibrated){
				if(trackingMode == 0){
					targetPrefab.SetActive(true);

				}
				if(trackingMode > 0){
					targetPrefab.SetActive(false);
				}
				trackingMode = (trackingMode + 1) % 3;
			}
			else if (calCount == 0){
			//change gui 
				uiText.text = "Press A to begin calibration";
				textTime = 0.0f;
			}
		}

		if (calCount != curCount) {
			calCount = calCount % 10;
			curCount = calCount;

			gameObject.GetComponent<Image> ().sprite = calImages [calCount];
		}
		if (trackingMode == 0 && calibrated) {
			gameObject.GetComponent<Image> ().sprite = calImages [0];
		}
		if (trackingMode == 1) {
			tr = GameObject.FindGameObjectWithTag ("MainCamera").transform;
			vec = (tr.forward) + (tr.right * ((transX - 590) / 1000)) + (tr.up * (((-transY) + 282) / 1000));
			//vec = (tr.forward) + (tr.right * (transX)) + (tr.up * ((transY)));
			Vector3 orig = GameObject.FindGameObjectWithTag ("MainCamera").transform.position;
			RaycastHit hit;
			Ray ray = new Ray (orig, vec);
			//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 100.0f)) {
				//Debug.DrawLine (ray.origin, hit.point);
				targetPrefab.transform.position = hit.point;
				targetPrefab.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
				targetPrefab.SetActive (true);
			} else {
				targetPrefab.SetActive (false);
			}
		} else if (trackingMode == 2) {
			int sectionIndex = 0;
			if(transX > 368){
				sectionIndex++;
				if(transX > 810){
					sectionIndex++;
				}
			}
			if (transY > 163){
				sectionIndex +=3;
				if(transY > 400){
					sectionIndex+=3;
				}
			}
			gameObject.GetComponent<Image> ().sprite = secImages [sectionIndex];
		}
		if (textTime >= 2.0) {
			uiText.text = "";
		} else {
			textTime += Time.deltaTime;
		}
	}


}
