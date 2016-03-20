using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;

/// <summary>
/// ブロック基底クラス
/// </summary>
public abstract class BlockBase : MonoBehaviour
{

	[SerializeField]
	private int _amountUntilBreak = 2; // 何回乗ったらブロックが壊れるか

	private bool isDrackable = true;

	public bool IsDrackable
	{
		set { isDrackable = value; }
	}

	private void Start(){

		// プレイヤーが _amountUntilBreak で指定した回数ブロックに乗ったら破壊する
		this.OnCollisionExit2DAsObservable ()
			.Where(coll => LayerMask.LayerToName(coll.gameObject.layer) == "Player" && _isOnBlock(coll.transform.position))
			.Skip (_amountUntilBreak - 1)
			.Subscribe (_ => Break());

		this.OnCollisionStay2DAsObservable ()
			.Where (coll => LayerMask.LayerToName (coll.gameObject.layer) == "Player" && _isOnBlock (coll.transform.position))
			.Subscribe (_ => onBlock ());
	}

	/// <summary>
	/// プレイヤーがブロックに乗っている間に呼ばれる
	/// </summary>
	public virtual void onBlock()
	{
		// なにもなし
	}

	/// <summary>
	/// ブロックを破壊する
	/// </summary>
	public virtual void Break()
	{
		Destroy (gameObject);
	}

	/// <summary>
	/// ブロックの上かどうか（横や下からの当たり判定を避けるために使います）
	/// </summary>
	protected bool _isOnBlock(Vector3 position)
	{
		if ((position.y > gameObject.transform.position.y)
			&& (position.x >= (gameObject.transform.position.x - (gameObject.transform.lossyScale.x / 2)))
			&& (position.x <= (gameObject.transform.position.x + (gameObject.transform.lossyScale.x / 2)))) {
			return true;
		}
		return false;
	}
}
