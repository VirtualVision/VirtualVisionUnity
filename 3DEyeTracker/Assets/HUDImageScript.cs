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

	public Transform tr;
	public Vector3 vec;
	public Ray ray;
	public float transX;
	public float transY;
	public bool isVisible;


	private NetMQContext context;
	private NetMQSocket server; 
	private NetMQSocket client; 

	private GameObject targetPrefab;

	// initialized sprites
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

		calCount = 0;
		curCount = 0;
		gameObject.GetComponent<Image> ().sprite = calImages [calCount];
		//max coords 1180*564
		transX = 0;
		transY = 0;
		targetPrefab = GameObject.FindGameObjectWithTag ("RaycastTarget");
		targetPrefab.SetActive (false);
		targetPrefab.layer = 2;//ignore raycast layer
		isVisible = false;

		AsyncIO.ForceDotNet.Force();
		//setup sockets
		//hangs????
		//http://forum.unity3d.com/threads/netmq-basic.298104/
		//must compile myself
		//https://github.com/zeromq/netmq/issues/98
		context = NetMQContext.Create ();
		server = context.CreatePublisherSocket ();
		server.Bind("tcp://127.0.0.1:5556");
		client = context.CreateSubscriberSocket ();
		client.Connect("tcp://127.0.0.1:5556");
		client.Subscribe ("coord");
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
		if (Input.GetKeyDown(KeyCode.Space)){
			calCount++;
		}
		if (Input.GetKeyDown(KeyCode.C)){
			if(isVisible){
				isVisible = false;
				targetPrefab.SetActive(false);
			}
			else{
				isVisible = true;
				targetPrefab.SetActive(true);
				server.SendMore("coord").Send ("200 200");
				string top = client.ReceiveString ();
				string message = client.ReceiveString ();
				Debug.Log (message);
				string[] coord = message.Split ();
				transX = int.Parse (coord [0]);
				transY = int.Parse (coord [1]);
			}
		}

		if (calCount != curCount) {
			calCount = calCount % 10;
			curCount = calCount;

			gameObject.GetComponent<Image> ().sprite = calImages [calCount];

		}
		tr = GameObject.FindGameObjectWithTag("MainCamera").transform;
		vec = (tr.forward) + (tr.right * ((transX-590) /1000)) + (tr.up * (((-transY)+282)/1000));

		Vector3 orig = Camera.main.transform.position;
		RaycastHit hit;
		Ray ray = new Ray (orig, vec);
		//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (isVisible) {
			if (Physics.Raycast (ray, out hit, 100.0f)) {
				Debug.DrawLine (ray.origin, hit.point);
				targetPrefab.transform.position = hit.point;
				targetPrefab.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
				targetPrefab.SetActive (true);
			} else {
				targetPrefab.SetActive (false);
			}
		}

	}
}
