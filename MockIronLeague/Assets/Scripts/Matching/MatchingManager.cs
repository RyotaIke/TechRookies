using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MatchingManager : MonoBehaviour {

	[SerializeField]
	private GameObject matchingWindow;

	[SerializeField]
	private Text[] playerNames;

	[SerializeField]
	PhotonView photonView;

	private GameObject canvas;

	void Awake() {
	}
		
	void Start ()
	{

	}
		
	public void setPlayerName(string playerName)
	{
		// 引数なし
		object[] args = new object[]{
			(int)PlayerInfo.Instance.playerType,
			playerName
		};

		// RPCメソッドの名前、引数を合わせる
		photonView.RPC(
			"_setPlayerName",           // メソッド名
			PhotonTargets.All,          // ネットワークプレイヤー全員に対して呼び出す
			args);                      // 引数
	}

	[PunRPC]
	public void _setPlayerName(int playerType, string playerName)
	{
		Debug.Log ("setPlayerName : " + playerType + " : " + playerName);
		playerNames [playerType].text = playerName;
		PlayerInfo.Instance.playerNames [playerType] = playerName;
		matchingWindow.SetActive (true);
	}

	public void startGameCoroutin()
	{
		StartCoroutine ("startGame");
	}

	private IEnumerator startGame() {
		yield return new WaitForSeconds (3.0f);
		SceneManager.LoadScene (Const.Scene.GAME, LoadSceneMode.Single);
		BgmManager.Instance.bgmStatus = BgmManager.BgmStatus.GAME;
		BgmManager.Instance.ChangeBgm ();
	}
}
