using System.Collections;
using UnityEngine;

public class CoinController : MonoBehaviour
{
	void OnCollisionEnter2D(Collision2D other)
    {
		if (LayerMask.LayerToName (other.gameObject.layer) == "Player") {
			if (other.gameObject.name == "MainPlayer_1") {
				other.gameObject.GetComponent<PlayerManager> ().OnPlayer1GetCoin ();   
			} else if (other.gameObject.name == "MainPlayer_2") {
				other.gameObject.GetComponent<PlayerManager> ().OnPlayer3GetCoin ();   
			}
			Destroy(gameObject);
        }
    }
}
