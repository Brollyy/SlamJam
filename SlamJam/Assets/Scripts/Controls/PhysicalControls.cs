using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PhysicalControls : MonoBehaviour {

	private BaseWave wave;
	private List<GameObject> UIsliders;

	public string OSCHost = "127.0.0.1";
	public int SendToPort = 3200;
	public int ListenerPort = 12000;

	// Use this for initialization
	void Start () {
		UIsliders = new List<GameObject>();
		wave = GameObject.FindGameObjectWithTag("Wave").GetComponent<BaseWave> ();
		for (int i = 0; i < 4; ++i) {
			UIsliders.Add(GameObject.FindGameObjectWithTag ("Slider"+i)) ;
		}

		UDPPacketIO udp = gameObject.GetComponent<UDPPacketIO> ();
		udp.init (OSCHost, SendToPort, ListenerPort);
		Osc handler = gameObject.GetComponent<Osc> ();
		handler.init (udp);
		handler.SetAllMessageHandler (HandleControllerMessage);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void HandleControllerMessage(OscMessage oscM) {
		for (int i = 0; i < oscM.Values.Count; ++i) {
			float value = (float)oscM.Values [i] / 1023.0F;
			wave.setVolume (i, value );
			UIsliders [i].GetComponent<Slider> ().normalizedValue = value;
		}
	}
}
