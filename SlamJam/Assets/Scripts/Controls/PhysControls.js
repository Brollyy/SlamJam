#pragma strict

import UnityEngine.UI;

private var UDPHost : String = "127.0.0.1";
private var listenerPort : int = 12000;
private var broadcastPort : int = 57131;
private var oscHandler : Osc;

private var data : float[];

private var sources : AudioSource[];

public function Start ()
{	
	sources = GameObject.FindGameObjectWithTag("Tracklist").GetComponents.<AudioSource>();
	data = new float[4];
	for(var i : int = 0; i < 4; ++i) {
		data[i] = 0.0F;
	}

	var udp : UDPPacketIO = GetComponent("UDPPacketIO");
	udp.init(UDPHost, broadcastPort, listenerPort);
	oscHandler = GetComponent("Osc");
	oscHandler.init(udp);
			
	//oscHandler.SetAddressHandler("/test", updateText);
	oscHandler.SetAddressHandler("/test", HandleControllerMessage);
	
}

function Update () {
	for(var i : int = 0; i < 4; ++i) {
		sources[i].volume = data[i] ;
	}
}	

public function HandleControllerMessage(oscM : OscMessage) : void
{	
	for (var i : int = 0; i < oscM.Values.Count; ++i) {
		var value : float = oscM.Values [i];
		data[i] = value / 1023.0F;
	}
}