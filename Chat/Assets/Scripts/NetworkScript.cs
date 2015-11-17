using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections.Generic;

public class NetworkScript : NetworkManager
{
	//system information -> network -> wifi address
	//public string connectionIP = "128.237.182.237";
	public string connectionIP = "localhost";
	//some ridiculous number
	//public int portNumber = 8271;
	public int portNumber = 7777;
	private string currentMessage = string.Empty;
	private bool connected = false;

    bool toggle = false;
    public GameObject button;

	public List<string> chatHistory = new List<string> ();

    public static short MSGType = 555;
	
    // Use this for initialization
    void Start()
    {
        button = GameObject.Find("ToggleButton");

    }

    // Update is called once per frame
    void Update()
    {

    }
	

	public override void OnClientConnect(NetworkConnection conn) {
		base.OnClientConnect (conn);
		Debug.Log ("client is connected");
		connected = true;
	}

	//automatically called when starting a client
    public override void OnStartClient(NetworkClient mClient)
    {
		Debug.Log ("onstartclient called");
		base.OnStartClient(mClient);
		mClient.RegisterHandler((short)MyMessages.MyMessageTypes.CHAT_MESSAGE, OnClientChatMessage);
    }


	//automatically called when starting a server
    // hook into NetManagers server setup process
    public override void OnStartServer()
    {
		Debug.Log ("onstartserver called");
        base.OnStartServer(); //base is empty
		NetworkServer.RegisterHandler((short)MyMessages.MyMessageTypes.CHAT_MESSAGE, OnServerChatMessage);
		//connected = true;
    }
	

	//when a chat message reaches the server
    private void OnServerChatMessage(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<StringMessage>();
		MyMessages.ChatMessage chat = new MyMessages.ChatMessage ();
		chat.message = msg.value;
		NetworkServer.SendToAll((short) MyMessages.MyMessageTypes.CHAT_MESSAGE, chat);
        //button.GetComponent<ToggleScript>().ToggleColor();
    }

	//when a chat message reaches the client
    private void OnClientChatMessage(NetworkMessage netMsg)
    {
		var msg = netMsg.ReadMessage <StringMessage>();
        //button.GetComponent<ToggleScript>().ToggleColor();
		chatHistory.Add (msg.value);

    }

	private void OnGUI()
	{
		if (!connected) {
			connectionIP = GUILayout.TextField (connectionIP);
			int.TryParse (GUILayout.TextField (portNumber.ToString()), out portNumber);

			//if connect button clicked
			if (GUILayout.Button ("Connect")) {
				this.networkAddress = connectionIP;
				this.networkPort = portNumber;
				this.StartClient();
			}
			//if host button clicked
			//a host is a server and a client at the same time
			if (GUILayout.Button ("Host")) {
				this.networkAddress = connectionIP;
				this.networkPort = portNumber;
				this.StartHost();
			}
		} else {
			//wrong
			GUILayout.Label ("Connections: " + Network.connections.Length.ToString ());
		}

		if (connected) {

			//chat display
			GUILayout.BeginHorizontal (GUILayout.Width (250));
			currentMessage = GUILayout.TextField (currentMessage);
			if (GUILayout.Button ("send")) {
				if (!string.IsNullOrEmpty (currentMessage.Trim ())) {
					MyMessages.ChatMessage msg = new MyMessages.ChatMessage ();
					msg.message = currentMessage;
					NetworkManager.singleton.client.Send((short) MyMessages.MyMessageTypes.CHAT_MESSAGE, msg);
					currentMessage = string.Empty;
				}
			}
			GUILayout.EndHorizontal ();
		
			foreach (string c in chatHistory) {
				GUILayout.Label (c);
			}
		}
	}
}
