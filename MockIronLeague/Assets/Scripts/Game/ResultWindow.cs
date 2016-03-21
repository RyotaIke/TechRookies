using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResultWindow : SingletonMonoBehaviour<ResultWindow> {

	[SerializeField]
	private Image bg;
	[SerializeField]
	private GameObject resultWindow;
	[SerializeField]
	private Text[] playerNameTexts;
	[SerializeField]
	private Sprite lifeRed;
	[SerializeField]
	private Image[] player_1Lifes;
	[SerializeField]
	private Text player_1LifePoint;
	[SerializeField]
	private Image[] player_2Lifes;
	[SerializeField]
	private Text player_2LifePoint;
	[SerializeField]
	private GameObject[] player_1flags;
	[SerializeField]
	private GameObject[] player_2flags;
	[SerializeField]
	private Text player_1CoinPoint;
	[SerializeField]
	private Text player_2CoinPoint;
	[SerializeField]
	private Text player_1TotalPoint;
	[SerializeField]
	private Text player_2TotalPoint;

	public void SetResulWindow()
	{
		int player_1Tp = 0;
		int player_2Tp = 0;

		for (int i = 0; i < PlayerInfo.Instance.playerNames.Length; i++) {
			playerNameTexts [i].text = PlayerInfo.Instance.playerNames[i];
		}
		int player_1LeftLife = PlayerInfo.Instance.Player1LeftLife;
		for (int i = 0; i < player_1LeftLife; i++) {
			player_1Lifes [i].sprite = lifeRed;
		}
		int player_1Lp = player_1LeftLife * 100;
		player_1Tp = player_1Tp + player_1Lp;
		player_1LifePoint.text = player_1Lp.ToString() + "pts";
		int player_2LeftLife = PlayerInfo.Instance.Player2LeftLife;
		for (int i = 0; i < player_2LeftLife; i++) {
			player_2Lifes [i].sprite = lifeRed;
		}
		int player_2Lp = player_2LeftLife * 100;
		player_2Tp = player_2Tp + player_2Lp;
		player_2LifePoint.text = player_2Lp.ToString() + "pts";
		bool hasFlagPlayer_1 = PlayerInfo.Instance.HasGoalFalgPlayer_1;
		if (hasFlagPlayer_1) {
			player_1Tp = player_1Tp + 1000;
		}
		SetGoalFlag (hasFlagPlayer_1, player_1flags);
		bool hasFlagPlayer_2 = PlayerInfo.Instance.HasGoalFalgPlayer_2;
		if (hasFlagPlayer_2) {
			player_2Tp = player_1Tp + 1000;
		}
		SetGoalFlag (hasFlagPlayer_2, player_2flags);
		int player_1GetCoin = PlayerInfo.Instance.Player1GetCoin;
		int player_1Cp = player_1GetCoin * 10;
		player_1Tp = player_1Tp + player_1Cp;
		player_1CoinPoint.text = "×" + player_1GetCoin.ToString() + "=" + player_1Cp.ToString() + "pts";
		int player_2GetCoin = PlayerInfo.Instance.Player3GetCoin;
		int player_2Cp = player_2GetCoin * 10;
		player_2Tp = player_2Tp + player_2Cp;
		player_2CoinPoint.text = "×" + player_2GetCoin.ToString() + "=" + player_2Cp.ToString() + "pts";

		player_1TotalPoint.text = player_1Tp.ToString ();
		player_2TotalPoint.text = player_2Tp.ToString ();

		ActivateResultWindow ();
	}

	private void SetGoalFlag(bool hasFlag, GameObject[] targetFlags)
	{
		if (hasFlag) {
			targetFlags [0].SetActive (true);
			targetFlags [1].SetActive (false);
		} else {
			targetFlags [0].SetActive (false);
			targetFlags [1].SetActive (true);
		}
	}

	/// <summary>
	/// ResultWindowを表示する
	/// </summary>
	public void ActivateResultWindow()
	{
		bg.enabled = true;
		resultWindow.SetActive (true);
	}

	/// <summary>
	/// 結果画面のCloseボタンを押下した時
	/// </summary>
	public void OnClickCloseBtn()
	{
		GameObject playerInfo = GameObject.Find ("PlayerInfo(Clone)");
		DestroyObject (playerInfo);
		SceneManager.LoadScene (Const.Scene.TITLE, LoadSceneMode.Single);
		PhotonNetwork.Disconnect ();
	}

}
