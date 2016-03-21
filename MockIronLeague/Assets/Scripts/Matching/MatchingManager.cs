using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MatchingManager : MonoBehaviour {

	private GameObject canvas;

	void Awake() {
		SceneManager.LoadScene (Const.Scene.CANVAS_MATCHING, LoadSceneMode.Additive);
	}
		
	void Start ()
	{
		canvas = GameObject.Find("Canvas");
	}


	public void activatePlayer (string objectName)
	{
		if (canvas != null)
		{
			Debug.Log (objectName);
			canvas.transform.Find(objectName).gameObject.SetActive (true);
		}
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
