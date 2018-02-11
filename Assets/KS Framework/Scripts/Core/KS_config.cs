using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_config : MonoBehaviour {

	public enum KS_GameDataLocations{
		/// <summary>
		/// Save in My Documents
		/// </summary>
		MyDocuments,
		/// <summary>
		/// Save in My Documents/My games/
		/// </summary>
		MyGames,
		/// <summary>
		/// Save in the users UserData folder
		/// </summary>
		UserData,
		/// <summary>
		/// Save in the games location
		/// </summary>
		GameFile
	}

	public enum KS_SaveDataType
	{
		Text,
		ini,
		JSON,
		XML
	}
}
