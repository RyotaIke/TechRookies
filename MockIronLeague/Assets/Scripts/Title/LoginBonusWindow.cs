using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginBonusWindow : SingletonMonoBehaviour<LoginBonusWindow> {

	[SerializeField]
	private Image bg;
	[SerializeField]
	private GameObject loginBonusWindow;

	/// <summary>
	/// ログインボーナスWindowを表示する
	/// </summary>
	public void ActivateLoginBonusWindow()
	{
		bg.enabled = true;
		loginBonusWindow.SetActive (true);
	}

	public void DeactivateLoginBonusWindow()
	{
		StartCoroutine (UpdateLastLogin ());
		bg.enabled = false;
		loginBonusWindow.SetActive (false);
	}

	private IEnumerator UpdateLastLogin()
	{
		WWWForm wwwForm = new WWWForm();
		wwwForm.AddField("keyword", "data");//不正接続防止用キーワード

		// get_debug_index
		string url = ApiList.ApiList.BASE_API_URL + ApiList.ApiList.UPDATE_LAST_LOGIN + "/" + TitleManager.Instance.TerminalId;
		WWW result = new WWW(url, wwwForm);
		// レスポンスを待つ
		yield return result;
		TitleManager.Instance.ActivateStartBtn ();
	}

}
