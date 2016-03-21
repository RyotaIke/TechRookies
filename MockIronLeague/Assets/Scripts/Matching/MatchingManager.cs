using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MatchingManager : MonoBehaviour {

	[SerializeField]
	private GameObject matchingWindow;

	[SerializeField]
	private Text[] playerNames;

	private GameObject canvas;

	void Awake() {
		//SceneManager.LoadScene (Const.Scene.CANVAS_MATCHING, LoadSceneMode.Additive);
	}
		
	void Start ()
	{
		//canvas = GameObject.Find("Canvas");
	}


	public void activatePlayer (string objectName)
	{
		Debug.Log ("===================================objectName : " + objectName + "=================");
		playerNames [(int)PlayerInfo.Instance.playerType].text = objectName;
	}


	public void startGameCoroutin()
	{
		StartCoroutine ("startGame");
	}

	private IEnumerator startGame() {
		yield return new WaitForSeconds (3.0f);
		SceneManager.LoadScene (Const.Scene.GAME, LoadSceneMode.Single);
	}
}
