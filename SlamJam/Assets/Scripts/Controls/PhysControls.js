#pragma strict

import UnityEngine.UI;

private var UDPHost : String = "127.0.0.1";
private var listenerPort : int = 12000;
private var broadcastPort : int = 57131;
private var oscHandler : Osc;

private var data : float[];

private var wave : BaseWave;
private var UIsliders : GameObject[];

public function Start ()
{	
	wave = GameObject.FindGameObjectWithTag("Wave").GetComponent(BaseWave);
	UIsliders = new GameObject[4];
	for(var i : int = 0; i < 4; ++i) {
		UIsliders[i] = GameObject.FindGameObjectWithTag("Slider"+i);
	}
	data = new float[4];

	var udp : UDPPacketIO = GetComponent("UDPPacketIO");
	udp.init(UDPHost, broadcastPort, listenerPort);
	oscHandler = GetComponent("Osc");
	oscHandler.init(udp);
			
	//oscHandler.SetAddressHandler("/test", updateText);
	oscHandler.SetAddressHandler("/test", HandleControllerMessage);
	
}

function Update () {
	for(var i : int = 0; i < 4; ++i) {
		wave.setVolume (i, data[i] );
		UIsliders [i].GetComponent(Slider).normalizedValue = data[i];
	}
}	

public function HandleControllerMessage(oscM : OscMessage) : void
{	
	for (var i : int = 0; i < oscM.Values.Count; ++i) {
		var value : float = oscM.Values [i];
		data[i] = value / 1023.0F;
	}
}