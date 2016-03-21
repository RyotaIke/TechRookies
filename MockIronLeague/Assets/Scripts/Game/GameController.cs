using UnityEngine;
using System;
using System.Collections;

public class GameController : SingletonMonoBehaviour<GameController>{

	private GameObject playerInfo;

	[Header("サポート用のObject群")]
	[SerializeField]
	private GameObject supportPlayerController;
	[SerializeField]
	private GameObject supportPlayerCanvas;

	[Header("プレイヤー用のObject群")]
	[SerializeField]
	private GameObject playerCanvas;

	void Awake()
	{
		
	}

	// Use this for initialization
	void Start () {
		switch (PlayerInfo.Instance.playerType) {
		case PlayerInfo.PlayerType.PLAYER_1:
			supportPlayerController.SetActive (false);
			supportPlayerCanvas.SetActive (false);
			break;
		case PlayerInfo.PlayerType.PLAYER_2:
			playerCanvas.SetActive (false);
			break;
		case PlayerInfo.PlayerType.PLAYER_3:
			supportPlayerController.SetActive (false);
			supportPlayerCanvas.SetActive (false);
			break;
		case PlayerInfo.PlayerType.PLAYER_4:
			playerCanvas.SetActive (false);
			break;
		}
	}

	/// <summary>
	/// ゲームが終了した時に呼ばれる
	/// </summary>
	public void OnGameFinish()
	{
		Debug.Log ("ゲーム終了");
	}
}
