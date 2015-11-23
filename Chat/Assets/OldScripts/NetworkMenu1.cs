using UnityEngine;
using System.Collections;

public class NetworkMenu1 : MonoBehaviour {

	public string connectionIP = "128.237.217.77";
	public int portNumber = 8271;

	private bool connected = false;

	private void OnConnectedToServer() {
	
		connected = true;
	}

	private void OnServerInitialized() {
	
		connected = true;
	}

	private void OnDisconnectedFromServer() {
		connected = false;
	}

	private void OnGUI()
	{
		if (!connected) {
			connectionIP = GUILayout.TextField (connectionIP);
			int.TryParse (GUILayout.TextField (portNumber.ToString()), out portNumber);

			if (GUILayout.Button ("Connect"))
				Network.Connect (connectionIP, portNumber);

			if (GUILayout.Button ("Host"))
				Network.InitializeServer (4, portNumber, true);
		} else {
			GUILayout.Label ("Connections: " + Network.connections.Length.ToString ());
		}
	}
}
