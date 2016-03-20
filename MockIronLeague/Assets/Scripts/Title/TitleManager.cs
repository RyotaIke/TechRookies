using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using Const;

public class TitleManager : SingletonMonoBehaviour<TitleManager> {

	/// <summary>
	/// 端末IDのプロパティ
	/// </summary>
	private string terminalId = null;
	public string TerminalId{get { return terminalId;} }

	/// <summary>
	/// ゲームを開始するためのボタン
	/// </summary>
	private Button startButton;
	Button StartButton{ get { return startButton ?? (startButton = GameObject.Find ("StartButton").GetComponent<Button> ()); } }

	private Text userName;
	Text UserName{ get{ return userName ?? (userName = GameObject.Find ("UserName").GetComponent<Text> ()); } }

	void Awake() {
		SceneManager.LoadScene (Const.Scene.CANVAS_TITLE, LoadSceneMode.Additive);
	}

	// Use this for initialization
	void Start () {
	
		terminalId = SystemInfo.deviceUniqueIdentifier;
		StartCoroutine (CheckRegisteredTerminalId());

		// Mainへの遷移 
		StartButton.OnClickAsObservable ().Subscribe (_ => {
			SceneManager.LoadScene (Const.Scene.GAME, LoadSceneMode.Single);
		});
	}

	/// <summary>
	/// この端末が登録されているかどうかをチェックするためのAPI接続
	/// </summary>
	/// <returns>The registered terminal identifier.</returns>
	private IEnumerator CheckRegisteredTerminalId(){

		//データ送信準備
		WWWForm wwwForm = new WWWForm();
		wwwForm.AddField("keyword", "data");//不正接続防止用キーワード

		// get_debug_index
		string url = ApiList.ApiList.BASEAPIURL + ApiList.ApiList.CHECKREGISTER + "/" + TerminalId;
		WWW result = new WWW(url, wwwForm);
		// レスポンスを待つ
		yield return result;
		Debug.Log (result.text);

		JsonObj jsonData = Json.Deserialize(result.text) as Dictionary<string, object>;
		if (jsonData ["is_register"]) {
			UserName.text = jsonData ["user"] ["name"].ToString ();
			Logger.Log (jsonData ["user"] ["name"], Logger.Class.TitleManager);
		} else {
			Debug.Log ("登録されてないよ！");
			RegisterWindow.Instance.ActivateResisterWindow ();
		}
	}
}