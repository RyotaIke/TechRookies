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
		bg.enabled = false;
		loginBonusWindow.SetActive (false);
		TitleManager.Instance.ActivateStartBtn ();
	}

}
