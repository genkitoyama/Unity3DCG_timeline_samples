using UnityEngine;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityOSC;

public class OSCSender : MonoBehaviour {

    public string outIP = "127.0.0.1";
    public int outPort = 12345;

	//timelineから送る値
	public float sendValue = 1.0f;

    // Script initialization
    void Start() {
        // init OSC
        OSCHandler.Instance.Init(); 

        // Initialize OSC clients (transmitters)
        OSCHandler.Instance.CreateClient("myClient", IPAddress.Parse(outIP), outPort);
    }

	// Reads all the messages received between the previous update and this one
	void Update() {
        // Send random number to the client
        OSCHandler.Instance.SendMessageToClient("myClient", "/val", sendValue);
        Debug.Log(sendValue);

		var go = GameObject.Find("Cube");
		float scale = Mathf.Max(0.0f, sendValue);
		go.transform.localScale = new Vector3(scale, scale, scale);
    }
}