using UniRx;
using UnityEngine;
using System;
using System.Collections;

public class PlayingArea : MonoBehaviour {

	[SerializeField]
	private float speed = 0.1f; // 画面が上がっていくスピード
	[SerializeField]
	private GameObject stage;

	private bool _isMovable = false;

	void Start()
	{
		gameObject.transform.position = getStartPosition ();

		Debug.Log (gameObject.transform.position);

		Observable.Timer (TimeSpan.FromSeconds (2)).Subscribe (_ => {
			_isMovable = true;
		});
			
	}
		
	public Vector3 getStartPosition()
	{

		Debug.Log (stage.transform.position.y);
		Debug.Log (stage.transform.localScale.y / 2);
		Debug.Log (gameObject.transform.localScale.y / 2);
		return new Vector3 (
			gameObject.transform.position.x,
			stage.transform.position.y - (stage.transform.lossyScale.y / 2) + (gameObject.transform.lossyScale.y / 2),
			gameObject.transform.position.z
		);
	}

	void FixedUpdate ()
	{
		if (_isMovable) {
			Vector3 tempPosition = gameObject.transform.position;
			tempPosition.y += speed;
			gameObject.transform.position = tempPosition;
			_isGoal ();
		}
	}

	private void _isGoal()
	{
		if ((stage.transform.lossyScale.y / 2) < (gameObject.transform.position.y + (gameObject.transform.lossyScale.y / 2))) {
			_isMovable = false;
		}
	}
}