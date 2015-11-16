using UnityEngine;
using UnityEngine.Networking;

using System.Collections;

public class MyMessages {
	public enum MyMessageTypes {
		CHAT_MESSAGE = 1000
	}

	public class ChatMessage : MessageBase {
		public string message;
	}
}