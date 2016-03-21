using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using Const;
using DG.Tweening;

public class TitleManager : SingletonMonoBehaviour<TitleManager> {

	/// <summary>
	/// 端末IDのプロパティ
	/// </summary>
	private string terminalId = null;
	public string TerminalId{get { return terminalId;} }

	[SerializeField]
	private Button startBtn;
	[SerializeField]
	private Text userName;

	/// <summary>
	/// 取得したJsonデータを格納
	/// </summary>
	private JsonObj userData;

	[SerializeField]
	private GameObject playerInfo;

	void Awake() {
		//SceneManager.LoadScene (Const.Scene.CANVAS_TITLE, LoadSceneMode.Additive);
		terminalId = SystemInfo.deviceUniqueIdentifier;
	}

	// Use this for initialization
	void Start () {

		StartCoroutine (CheckRegisteredTerminalId());
		DontDestroyOnLoad (Instantiate (playerInfo).gameObject);

		// Mainへの遷移 
		startBtn.OnClickAsObservable ().Subscribe (_ => {
			SceneManager.LoadScene (Const.Scene.MATCHING, LoadSceneMode.Single);
		});
	}

	/// <summary>
	/// この端末が登録されているかどうかをチェックするためのAPI接続
	/// </summary>
	/// <returns>The registered terminal identifier.</returns>
	public IEnumerator CheckRegisteredTerminalId(){

		//データ送信準備
		WWWForm wwwForm = new WWWForm();
		wwwForm.AddField("keyword", "data");//不正接続防止用キーワード

		// get_debug_index
		string url = ApiList.ApiList.BASE_API_URL + ApiList.ApiList.CHECK_REGISTER + "/" + TerminalId;
		WWW result = new WWW(url, wwwForm);
		// レスポンスを待つ
		yield return result;

		userData = Json.Deserialize(result.text) as Dictionary<string, object>;
		if (userData ["is_register"]) {
			SetUserName ();
			StartCoroutine (CheckLoginBonus ());
		} else {
			RegisterWindow.Instance.ActivateResisterWindow ();
		}
	}

	/// <summary>
	/// 取得したデータをもとにユーザー情報をセットする
	/// </summary>
	private void SetUserName()
	{
		userName.text = userData ["user"] ["name"].ToString ();
		PlayerInfo.Instance.PlayerName = userData ["user"] ["name"].ToString ();
		PlayerInfo.Instance.PlayerLeftLife = 3;
	}

	public void ActivateStartBtn()
	{
		startBtn.interactable = true;
	}

	/// <summary>
	/// ログインボーナスを取得しているかどうかチェックする
	/// </summary>
	/// <returns>The login bonus.</returns>
	private IEnumerator CheckLoginBonus()
	{
		WWWForm wwwForm = new WWWForm();
		wwwForm.AddField("keyword", "data");//不正接続防止用キーワード

		// get_debug_index
		string url = ApiList.ApiList.BASE_API_URL + ApiList.ApiList.CHECK_GOT_LOGIN_BONUS + "/" + TerminalId;
		WWW result = new WWW(url, wwwForm);
		// レスポンスを待つ
		yield return result;

		JsonObj jsonData = Json.Deserialize(result.text) as Dictionary<string, object>;
		if (!jsonData ["is_today_login"]) {
			// ログインボーナスWindow開く
			LoginBonusWindow.Instance.ActivateLoginBonusWindow ();
		} else {
			ActivateStartBtn ();
		}
	}
}