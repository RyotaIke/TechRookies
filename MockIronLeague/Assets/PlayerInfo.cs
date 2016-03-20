using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour {

	private int playerType;
	public int PlayerType
	{
		get { return playerType; }
		set { this.playerType = value; }
	}


	void Awake()
	{
		DontDestroyOnLoad(this);
	}
}
