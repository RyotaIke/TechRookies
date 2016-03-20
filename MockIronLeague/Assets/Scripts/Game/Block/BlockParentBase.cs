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

	private int spawnPosition;
	public int SpawnPosition { set { spawnPosition = value; } }

	// Use this for initialization
	void Start () {
		scale = (Camera.main.orthographicSize * 2) / (float)Screen.width * ((float)Screen.width / (float)Screen.height);
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
			//  グリッドに合うように位置を調整する

			if (true) {
				// 離したエリアが大丈夫なエリアなら
				foreach (Transform child in gameObject.transform)
				{
					// UIの後ろに表示されるようにlayerをいじる
					//  当たり判定をonにする
					child.GetComponent<SpriteRenderer> ().sortingLayerName = "Default";
					child.GetComponent<BoxCollider2D> ().isTrigger = false;
				}
				gameObject.transform.parent.GetComponent<SupportPlayerController> ().readyBlocks [spawnPosition] = false;
				// 	親をstageに変更する
				gameObject.transform.SetParent (gameObject.transform.parent.transform.parent.transform.parent);


				// 	ドラッグできないようにisDrackableをfalseにする
				isDrackable = false;
			} else {
				// ダメだったら
				// 	positionとscaleを元に戻す
				gameObject.transform.localScale = beforeUIBlockScale;
				gameObject.transform.localPosition = beforeDragPosition;
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
