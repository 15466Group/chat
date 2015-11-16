using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections.Generic;

public class NetworkScript : NetworkManager
{

	public string connectionIP = "128.237.217.77";
	public int portNumber = 8271;
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

    public override void OnStartClient(NetworkClient mClient)
    {
		base.OnStartClient(mClient);
        //mClient.RegisterHandler(MSGType, OnClientChatMessage);
		mClient.RegisterHandler((short)MyMessages.MyMessageTypes.CHAT_MESSAGE, OnClientChatMessage);
		connected = true;
    }



    // hook into NetManagers server setup process
    public override void OnStartServer()
    {
        base.OnStartServer(); //base is empty
       // NetworkServer.RegisterHandler(MSGType, OnServerChatMessage);
		NetworkServer.RegisterHandler((short)MyMessages.MyMessageTypes.CHAT_MESSAGE, OnServerChatMessage);
		connected = true;
    }

    private void OnServerChatMessage(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<StringMessage>();
		MyMessages.ChatMessage chat = new MyMessages.ChatMessage ();
		chat.message = msg.value;
		NetworkServer.SendToAll((short) MyMessages.MyMessageTypes.CHAT_MESSAGE, msg);
        button.GetComponent<ToggleScript>().ToggleColor();
    }

    private void OnClientChatMessage(NetworkMessage netMsg)
    {
        //IntegerMessage msg = netMsg.ReadMessage<IntegerMessage>();
		var msg = netMsg.ReadMessage <StringMessage>();
        button.GetComponent<ToggleScript>().ToggleColor();
		chatHistory.Add (msg.value);

    }

	private void OnGUI()
	{
//		if (!connected) {
//			connectionIP = GUILayout.TextField (connectionIP);
//			int.TryParse (GUILayout.TextField (portNumber.ToString()), out portNumber);
//			
//			if (GUILayout.Button ("Connect"))
//				Network.Connect (connectionIP, portNumber);
//			
//			if (GUILayout.Button ("Host"))
//				Network.InitializeServer (4, portNumber, true);
//		} else {
//			GUILayout.Label ("Connections: " + Network.connections.Length.ToString ());
//		}

//		if (connected) {
			GUILayout.BeginHorizontal (GUILayout.Width (280));
			currentMessage = GUILayout.TextField (currentMessage);
			if (GUILayout.Button ("send")) {
				if (!string.IsNullOrEmpty (currentMessage.Trim ())) {
					//GetComponent<NetworkView> ().RPC ("ChatMessage", RPCMode.AllBuffered, new object[] {currentMessage});
					MyMessages.ChatMessage msg = new MyMessages.ChatMessage ();
					msg.message = currentMessage;
					NetworkManager.singleton.client.Send((short) MyMessages.MyMessageTypes.CHAT_MESSAGE, msg);
//					NetworkManager.singleton.client.Send((short) MSGType, msg);
					Debug.Log (NetworkManager.singleton.client);
					currentMessage = string.Empty;
				}
			}
			GUILayout.EndHorizontal ();
		
			foreach (string c in chatHistory) {
				GUILayout.Label (c);
			}
//		}
	}
}
