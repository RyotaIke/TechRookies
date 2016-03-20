using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class RegisterWindow : SingletonMonoBehaviour<RegisterWindow> {

	/// <summary>
	/// ユーザー名を入力するInputField
	/// </summary>
	[SerializeField]
	private InputField nameField;
	/// <summary>
	/// 登録ボタンを押下した時のエラー文言表示テキスト
	/// </summary>
	[SerializeField]
	private Text attentionTextField;
	/// <summary>
	/// 登録をするためのWindow
	/// </summary>
	[SerializeField]
	private GameObject registerWindow;
	/// <summary>
	/// 登録が成功したときのポップアップ
	/// </summary>
	[SerializeField]
	private GameObject successWindow;
	/// <summary>
	/// 登録が失敗したときのポップアップ
	/// </summary>
	[SerializeField]
	private GameObject failedWindow;

	[SerializeField]
	private Image bg;

	/// <summary>
	/// 登録ボタンを押下したときの処理
	/// </summary>
	public void OnClickRegisterBtn(){

		if (nameField.text != null) {
			attentionTextField.enabled = false;
			bg.enabled = false;
			StartCoroutine (RegisterUser (nameField.text));
		} else {
			bg.enabled = true;
			attentionTextField.enabled = true;
		}
	}

	/// <summary>
	/// ResisterWindowをアクティブにする
	/// </summary>
	public void ActivateResisterWindow()
	{
		bg.enabled = true;
		registerWindow.SetActive (true);
	}
	/// <summary>
	/// ResisterWindowを非アクティブにする
	/// </summary>
	public void DeactivateResisterWindow()
	{
		registerWindow.SetActive (false);
	}

	public void DeactivateWindows()
	{
		successWindow.SetActive (false);
		failedWindow.SetActive (false);
	}

	/// <summary>
	/// ユーザー登録をするためのAPI接続
	/// </summary>
	/// <returns>The user.</returns>
	/// <param name="userName">User name.</param>
	private IEnumerator RegisterUser(string userName){

		WWWForm wwwForm = new WWWForm();
		wwwForm.AddField("keyword", "data");//不正接続防止用キーワード

		// 登録する情報をパラメータとしてセットする
		string arg = TitleManager.Instance.TerminalId + "/" + userName;
		string url = ApiList.ApiList.BASEAPIURL + ApiList.ApiList.REGISTERUSER + "/" + arg;
		Debug.Log("======================="+url+"=========================");
		WWW result = new WWW(url , wwwForm);
		// レスポンスを待つ
		yield return result;
		Debug.Log (result.text);

		JsonObj jsonData = Json.Deserialize(result.text) as Dictionary<string, object>;
		if (jsonData ["is_success"]) {
			successWindow.SetActive (true);
			DeactivateResisterWindow ();
		} else {
			failedWindow.SetActive (false);
			DeactivateResisterWindow ();
		}
		
	}

}
