using UnityEngine;
using System;
using System.Collections;

public class GameController : MonoBehaviour{

	[SerializeField]
	private GameObject player;

	private GameObject playerInfo;

	[Header("サポート用のObject群")]
	[SerializeField]
	private GameObject supportPlayerController;
	[SerializeField]
	private GameObject supportPlayerCanvas;

	[Header("プレイヤー用のObject群")]
	[SerializeField]
	private GameObject playerCanvas;

	[Header("debug用に一時的においている値")]
	public int playerType = 1;// 1だとplayer 2だとsupport

	void Awake()
	{
		playerInfo = GameObject.Find ("PlayerInfo");
	}

	// Use this for initialization
	void Start () {
		if (playerInfo.GetComponent<PlayerInfo>().PlayerType == 1) {
			// サポート側の機能をoffに
			supportPlayerController.SetActive (false);
			supportPlayerCanvas.SetActive (false);
		} else {
			// プレイヤー側の機能をoffに
			playerCanvas = GameObject.Find("Canvas - Player");
			playerCanvas.SetActive (false);
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
