using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ゲーム中のUIを管理する裏す
/// </summary>
public class CanvasController : MonoBehaviour {

	[SerializeField]
	private GameObject playerLife;

	private PlayerManager player;
	List<GameObject> playerLifeList = new List<GameObject>();

	void Start () {
		initialize ();

		// playerのライフに変動があった時を監視
		player
			.ObserveEveryValueChanged (p => p.Life)
			.Subscribe (_ => onLifeDecreased());
	}

	/// <summary>
	/// ゲーム中のUIを管理する裏す
	/// </summary>
	private void initialize()
	{
		// プレイヤーの取得
		player = GameObject.Find ("Player").GetComponent<PlayerManager>();

		// プレイヤーのライフをprefabから生成
		for(int i = 0; i < player.Life; i++){
			playerLifeList.Add (Instantiate (playerLife,new Vector3(50,-450 + (50 * i),0),gameObject.transform.rotation) as GameObject);
			playerLifeList[i].transform.SetParent (gameObject.transform, false);
		}
	}

	/// <summary>
	/// ライフが減った時に呼び出される
	/// </summary>
	private void onLifeDecreased()
	{
		if (playerLifeList.Count > player.Life) {
			Destroy (playerLifeList [playerLifeList.Count - 1]);
			playerLifeList.RemoveAt (playerLifeList.Count - 1);
		}
	}
}
