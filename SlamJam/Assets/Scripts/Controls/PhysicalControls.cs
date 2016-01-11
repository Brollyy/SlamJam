using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PhysicalControls : MonoBehaviour {

	private AudioSource[] aSources;
	private float[] volumes;
	private int n;

	public string OSCHost = "127.0.0.1";
	public int SendToPort = 3200;
	public int ListenerPort = 12000;

	// Use this for initialization
	void Start () {
		aSources = GameObject.FindGameObjectWithTag("Tracklist").GetComponents<AudioSource> ();
		n = Mathf.Min (4, aSources.Length);
		volumes = new float[n];
		for (int i = 0; i < n; ++i) {
			volumes [i] = 0.0F;
		}

		UDPPacketIO udp = gameObject.GetComponent<UDPPacketIO> ();
		udp.init (OSCHost, SendToPort, ListenerPort);
		Osc handler = gameObject.GetComponent<Osc> ();
		handler.init (udp);
		handler.SetAllMessageHandler (HandleControllerMessage);
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < n; ++i) {
			aSources [i].volume = volumes [i];
		}
	}

	void HandleControllerMessage(OscMessage oscM) {
		for (int i = 0; i < Mathf.Min(oscM.Values.Count, n); ++i) {
			float value = (float)oscM.Values [i] / 1023.0F;
			volumes[i] = value;
		}
	}
}
