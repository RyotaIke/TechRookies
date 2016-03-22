using UnityEngine;
using System.Collections;

public class PhotonCoinController : MonoBehaviour {

	void Awake()
	{
		this.gameObject.transform.SetParent(GameObject.Find("Stage").transform);
		this.gameObject.transform.localScale = new Vector3 (0.017f, 0.0017f, 1f);
	}
}
