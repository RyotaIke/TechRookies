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

		Observable.Timer (TimeSpan.FromSeconds (2)).Subscribe (_ => {
			_isMovable = true;
		});
			
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

	/// <summary>
	/// ステージを基準にゲーム開始時ののカメラ位置をとってくる
	/// </summary>
	/// <returns>The start position.</returns>
	public Vector3 getStartPosition()
	{
		return new Vector3 (
			gameObject.transform.position.x,
			stage.transform.position.y - (stage.transform.lossyScale.y / 2) + (gameObject.transform.lossyScale.y / 2),
			gameObject.transform.position.z
		);
	}


	/// <summary>
	/// Ises the goal.
	/// </summary>
	private void _isGoal()
	{
		if ((stage.transform.lossyScale.y / 2) < (gameObject.transform.position.y + (gameObject.transform.lossyScale.y / 2))) {
			_isMovable = false;
		}
	}
}