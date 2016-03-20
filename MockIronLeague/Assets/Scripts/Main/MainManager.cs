using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainManager : MonoBehaviour {

	void Awake() {
		SceneManager.LoadScene (Const.Scene.CANVAS_TITLE, LoadSceneMode.Additive);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
