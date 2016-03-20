using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤーの情報を管理するクラス
/// </summary>
public class PlayerManager : MonoBehaviour {

	[SerializeField]
	private GameController gameController;
	[SerializeField]
	private int life = 5;

	public int Life
	{
		set { this.life = value; }
		get { return this.life; }
	}

	void Start()
	{
		// プレイヤーが画面外下い配置されているDeathAreaにぶつかったら onDeath()を呼ぶ
		this.OnTriggerEnter2DAsObservable ()
			.Where (coll => LayerMask.LayerToName (coll.gameObject.layer) == "DeathArea")
			.Subscribe (_ => onDeath ());

		// ゴールに浮いたらOnGameFinishを呼ぶ
		this.OnTriggerEnter2DAsObservable ()
			.Where (coll => LayerMask.LayerToName (coll.gameObject.layer) == "Goal")
			.Subscribe (_ => gameController.OnGameFinish());
	}

	/// <summary>
	/// プレイヤーのが画面外に落ちて死んだ時に呼ばれる
	/// </summary>
	public void onDeath(){
		Debug.Log("死にました");
		life -= 1;
	}
}
