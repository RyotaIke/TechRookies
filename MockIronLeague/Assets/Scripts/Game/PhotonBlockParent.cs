using UnityEngine;
using System.Collections;

public class PhotonBlockParent : MonoBehaviour {

	void Awake()
	{
		this.gameObject.transform.SetParent(GameObject.Find("Stage").transform);
		this.gameObject.transform.localScale = new Vector3 (0.1f, 0.01f, 1f);
	}
}
