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

	private void SetResult(){
		ResultWindow.Instance.SetResulWindow ();
	}

	/// <summary>
	/// ゲームが終了した時に呼ばれる
	/// </summary>
	public void OnGameFinish(string playerName)
	{
		// ここでどっちが旗をとったのかを同期させる
		// 引数なし
		object[] args = new object[]{
			playerName == "Player_1",
			playerName == "Player_2"
		};

		// RPCメソッドの名前、引数を合わせる
		gameObject.GetComponent<PhotonView>().RPC(
			"syncResult",                  // メソッド名
			PhotonTargets.All,          // ネットワークプレイヤー全員に対して呼び出す
			args);                      // 引数


		// 結果表示
		SetResult ();
	}

	[PunRPC]
	public void syncResult(bool isPlayer1Goal, bool isPlayer2Goal)
	{
		PlayerInfo.Instance.HasGoalFalgPlayer_1 = isPlayer1Goal;
		PlayerInfo.Instance.HasGoalFalgPlayer_2 = isPlayer2Goal;
	}
}
