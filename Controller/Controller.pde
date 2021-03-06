import processing.serial.*;

import cc.arduino.*;
import org.firmata.*;

import oscP5.*;
import netP5.*;

Arduino arduino;

OscP5 oscP5;
NetAddress myRemoteLocation;

int buttonPin = 8;
int buttonState = 0;
int lastButtonState = 1;



void setup() {
  arduino = new Arduino(this, Arduino.list()[0], 57600);

  size(400, 400);
  frameRate(25);
  /* start oscP5, listening for incoming messages at port 12000 */
  oscP5 = new OscP5(this, 8000);

  /* myRemoteLocation is a NetAddress. a NetAddress takes 2 parameters,
   * an ip address and a port number. myRemoteLocation is used as parameter in
   * oscP5.send() when sending osc packets to another computer, device, 
   * application. usage see below. for testing purposes the listening port
   * and the port of the remote location address are the same, hence you will
   * send messages back to this sketch.
   */
  myRemoteLocation = new NetAddress("127.0.0.1", 12000);
}


void draw() {
  lastButtonState = buttonState;
  buttonState = arduino.digitalRead(buttonPin);
  

  background(0);

  /* in the following different ways of creating osc messages are shown by example */
  OscMessage myMessage = new OscMessage("/test");
  OscMessage myMessage1 = new OscMessage("/test1");


  myMessage.add(1023-arduino.analogRead(0)); /* add an int to the osc message */
  myMessage.add(1023-arduino.analogRead(1)); /* add an int to the osc message */
  myMessage.add(arduino.analogRead(2)); /* add an int to the osc message */
  myMessage.add(1023-arduino.analogRead(3)); /* add an int to the osc message */
  myMessage1.add(1);

  if (lastButtonState > 0 && buttonState == 0)
  {
    //println("key pressed!");
    oscP5.send(myMessage1, myRemoteLocation); 

  }


  /* send the message */
  oscP5.send(myMessage, myRemoteLocation); 
  //print(1023-arduino.analogRead(0));
  //print('\t');
  //println(1023-arduino.analogRead(1));
  //print(arduino.analogRead(2));
  //print('\t');
  //println(1023-arduino.analogRead(3));
}




/* incoming osc message are forwarded to the oscEvent method. */
void oscEvent(OscMessage theOscMessage) {
  /* print the address pattern and the typetag of the received OscMessage */
  print("### received an osc message.");
  print(" addrpattern: "+theOscMessage.addrPattern());
  print(" typetag: "+theOscMessage.typetag());
  println(" timetag: "+theOscMessage.timetag());
}