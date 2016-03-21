using UnityEngine;
using System.Collections;

/// <summary>
/// photon接続周りをテストするために作ったダミークラス
/// </summary>
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
