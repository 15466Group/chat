using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;

public class ToggleScript1 : MonoBehaviour {

    bool toggle = false;

	// Use this for initialization
	void Start () {
        GetComponent<Image>().color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void clickButton()
    {
        //Toggle yourself first
        ToggleColor();

        //Then toggle the other peer
        if (NetworkManager.singleton.IsClientConnected())
        {
            //If this is a client
            NetworkManager.singleton.client.Send(NetworkScript.MSGType, new IntegerMessage());
        }
        else if (NetworkManager.singleton.isNetworkActive)
        {
            //If this is a server
            NetworkServer.SendToAll(NetworkScript.MSGType, new IntegerMessage());
        }
    }

    public void ToggleColor()
    {
        if (toggle)
        {
            GetComponent<Image>().color = Color.red;
            toggle = false;

        }
        else
        {
            GetComponent<Image>().color = Color.green;
            toggle = true;
        }
 
 
    }
}
