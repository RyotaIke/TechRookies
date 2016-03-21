using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BlockParentBase : MonoBehaviour,
IDragHandler,
IPointerDownHandler,
IPointerUpHandler
{
	// ドラッグでの移動量計算の時に必要なscale
	private float scale = 1f;

	// UI上にあるときのscale
	private Vector3 beforeUIBlockScale = new Vector3(0.05f,0.0278f,1f);
	public  Vector3 BeforeUIBlockScale { get { return beforeUIBlockScale; } }
	// Stageにあるときのscale
	private Vector3 stageBlockscale = new Vector3(0.1f,0.055f,1f);
	// ドラッグがダメだった時に戻すためのポジション
	private Vector3 beforeDragPosition;
	// ドラッグ可能かどうか
	private bool isDrackable = false;
	public bool IsDrackable { set { isDrackable = value; } }
	// ブロックを置けるかどうか
	private bool canSet = true;

	private int spawnPosition;
	public int SpawnPosition { set { spawnPosition = value; } }

	// Use this for initialization
	void Start () {
		scale = (Camera.main.orthographicSize * 2) / (float)Screen.width * ((float)Screen.width / (float)Screen.height);

		this.OnTriggerStay2DAsObservable ()
			.Where (coll =>  coll.gameObject.CompareTag("block") || coll.gameObject.CompareTag("Coin") || coll.gameObject.CompareTag("Player"))
			.Subscribe (_ => {
				canSet = false;
			});

		this.OnTriggerExit2DAsObservable()
			.Where (coll =>  coll.gameObject.CompareTag("block") || coll.gameObject.CompareTag("Coin") || coll.gameObject.CompareTag("Player"))
			.Subscribe (_ => { 
				canSet = true;
			});
	}
		
	public void OnPointerDown (PointerEventData eventData)
	{
		if (isDrackable) {
			beforeDragPosition = gameObject.transform.localPosition;
			gameObject.transform.localScale = stageBlockscale;
		}
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		if (isDrackable) {
			if (canSet) {
				// 離したエリアが大丈夫なエリアなら
				gameObject.transform.parent.GetComponent<SupportPlayerController> ().readyBlocks [spawnPosition] = false;

				PhotonNetwork.Instantiate (
					"Prefabs/Photon" + this.name,
					gameObject.transform.position,
					Quaternion.identity,
					0
				);
					
				Destroy (gameObject);

			} else {
				// ダメだったら
				// 	positionとscaleを元に戻す
				gameObject.transform.localScale = beforeUIBlockScale;
				gameObject.transform.localPosition = beforeDragPosition;
				canSet = true;
			}
		}
	}

	public void OnDrag (PointerEventData eventData)
	{
		if (isDrackable) {
			gameObject.transform.position = gameObject.transform.position + new Vector3(eventData.delta.x, eventData.delta.y) * scale;
		}
	}
}
