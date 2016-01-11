#pragma strict

import UnityEngine.UI;

private var UDPHost : String = "127.0.0.1";
private var listenerPort : int = 12000;
private var broadcastPort : int = 57131;
private var oscHandler : Osc;

private var data : float[];
private var button : int;

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
			
	oscHandler.SetAddressHandler("/test1", HandleButtonMessage);
	oscHandler.SetAddressHandler("/test", HandleControllerMessage);
	
}

function Update () {
	for(var i : int = 0; i < 4; ++i) {
		sources[i].volume = data[i] ;
	}

	if(button == 1) {
		button = 0;
		var a : boolean = GameObject.FindGameObjectWithTag("Finish").GetComponent.<ReachGoal>().Complete();
		if(!a) GameObject.FindGameObjectWithTag("Respawn").GetComponent.<Respawner>().Respawn();
	}
}	

public function HandleControllerMessage(oscM : OscMessage) : void
{	
	for (var i : int = 0; i < oscM.Values.Count; ++i) {
		var value : float = oscM.Values [i];
		data[i] = value / 1023.0F;
	}
}

public function HandleButtonMessage(oscM : OscMessage) : void
{	
	button = oscM.Values[0];
}