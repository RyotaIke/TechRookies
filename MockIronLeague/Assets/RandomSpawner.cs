using UnityEngine;
using System.Collections;

public class RandomSpawner : MonoBehaviour {

	public GameObject[] spawnObjectList;

	private bool canSpawn = true;

	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		spawnObject ();
	}
	
	/// <summary>
	/// spawnObjectListの中からランダムで１つ選んで設置する
	/// </summary>
	public void spawnObject()
	{
		if (PhotonNetwork.isMasterClient && canSpawn) {
			PhotonNetwork.Instantiate (
				"Prefabs/" + spawnObjectList [Random.Range (0, spawnObjectList.Length)].name,
				gameObject.transform.position,
				Quaternion.identity,
				0
			);
			canSpawn = false;
		}
	}
}
