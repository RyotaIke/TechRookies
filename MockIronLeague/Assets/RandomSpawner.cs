using UnityEngine;
using System.Collections;

public class RandomSpawner : MonoBehaviour {

	public GameObject[] spawnObjectList;

	// Use this for initialization
	void Start () {
		spawnObject ();
	}
	
	/// <summary>
	/// spawnObjectListの中からランダムで１つ選んで設置する
	/// </summary>
	public void spawnObject()
	{
		if (PhotonNetwork.isMasterClient) {
			PhotonNetwork.Instantiate (
				"Prefabs/" + spawnObjectList [Random.Range (0, spawnObjectList.Length)].name,
				gameObject.transform.position,
				Quaternion.identity,
				0
			);
		}
	}
}
