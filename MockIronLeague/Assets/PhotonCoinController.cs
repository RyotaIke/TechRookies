using UnityEngine;
using System.Collections;

public class PhotonCoinController : MonoBehaviour {

	void Awake()
	{
		this.gameObject.transform.SetParent(GameObject.Find("Stage").transform);
		this.gameObject.transform.localScale = new Vector3 (0.085f, 0.0085f, 1f);
	}
}
