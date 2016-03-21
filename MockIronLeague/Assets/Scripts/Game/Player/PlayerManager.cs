using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;
using DG.Tweening;

/// <summary>
/// プレイヤーの情報を管理するクラス
/// </summary>
public class PlayerManager : MonoBehaviour {

	[SerializeField]
	private GameController gameController;

	// 地面に立っているかどうかを判定するためのレイヤー
	public LayerMask whatIsGround;
	// 地面に立っているかどうか
	private bool isGround = false;

	// キャラ自体に付いているコンポーネント群 
	public Animator      m_animator;
	public BoxCollider2D m_boxcollier2D;
	public Rigidbody2D   m_rigidbody2D;

	public Vector2 velocity;

	// Photonで挙動を同期させる用
	public PhotonView photonView;

	// キャラクターの状態遷移用
	private State m_state = State.Normal;
	enum State
	{
		Normal,		//その他
		Death,		//死
		Invincible,	//無敵
	}

	void Awake()
	{
		m_animator     = GetComponent<Animator>();
		m_boxcollier2D = GetComponent<BoxCollider2D>();
		m_rigidbody2D  = GetComponent<Rigidbody2D>();
		photonView     = GetComponent<PhotonView> ();
		whatIsGround   = 1 << LayerMask.NameToLayer("Ground");
	}

	void Start()
	{
		// プレイヤーが画面外下い配置されているDeathAreaにぶつかったら onDeath()を呼ぶ
		this.OnTriggerEnter2DAsObservable ()
			.Where (coll => LayerMask.LayerToName (coll.gameObject.layer) == "DeathArea")
			.Subscribe (_ => OnDamaged());

		// ゴールに浮いたらOnGameFinishを呼ぶ
		this.OnTriggerEnter2DAsObservable ()
			.Where (coll => LayerMask.LayerToName (coll.gameObject.layer) == "Goal")
			.Subscribe (_ => gameController.OnGameFinish());
	}

	void FixedUpdate()
	{
		isGround = Physics2D.Linecast (
			transform.position + transform.up * 1,
			transform.position - transform.up * 0.05f,
			whatIsGround);

		changeAnimation ();
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (LayerMask.LayerToName (coll.gameObject.layer) == "Player") {
			// ポジションの差分をみて相手が頭上かどうかを判定
			if ((coll.transform.localPosition.y - gameObject.transform.localPosition.y) >= gameObject.transform.localScale.y) {
				OnDamaged ();
			}
		}
	}

	/// <summary>
	/// キャラクタのアニメーションを制御する
	/// </summary>
	private void changeAnimation()
	{
		if (m_state == State.Death) {
			// 死んでたらdeath anim
			m_animator.SetBool ("Death", true);
			m_animator.SetBool ("Jump" , false);
			m_animator.SetBool ("Walk" , false);
			m_animator.SetBool ("Stay" , false);
		} else if (Mathf.Abs (m_rigidbody2D.velocity.y) > 0.1) {
			// 空中にいたらjump anim
			m_animator.SetBool ("Death", false);
			m_animator.SetBool ("Jump" , true);
			m_animator.SetBool ("Walk" , false);
			m_animator.SetBool ("Stay" , false);
		} else if (Mathf.Abs (m_rigidbody2D.velocity.x) > 0.1) {
			// 左右に動いてたらwalk anim
			m_animator.SetBool ("Death", false);
			m_animator.SetBool ("Jump" , false);
			m_animator.SetBool ("Walk" , true);
			m_animator.SetBool ("Stay" , false);
		} else {
			m_animator.SetBool ("Death", false);
			m_animator.SetBool ("Jump" , false);
			m_animator.SetBool ("Walk" , false);
			m_animator.SetBool ("Stay" , true);
		}
	}

	/// <summary>
	/// 同期とる時に呼ぶジャンプアクション
	/// </summary>
	public void rpcJump(float jumpPower)
	{    
		// 引数なし
		object[] args = new object[]{
			jumpPower
		};

		// RPCメソッドの名前、引数を合わせる
		photonView.RPC(
			"Jump",                  // メソッド名
			PhotonTargets.All,          // ネットワークプレイヤー全員に対して呼び出す
			args);                      // 引数
	}

	[PunRPC]
	public void Jump(float jumpPower)
	{
		// 死んでない かつ 地面に接してる時のみジャンプ可能
		if (m_state != State.Death && isGround) {
			m_rigidbody2D.AddForce(Vector2.up * jumpPower);
		}
	}

	/// <summary>
	/// 同期とる時に呼ぶMoveアクション
	/// </summary>
	public void rpcMove(float move, float speed)
	{
		// 引数なし
		object[] args = new object[]{
			move,
			speed
		};

		// RPCメソッドの名前、引数を合わせる
		photonView.RPC(
			"Move",                  // メソッド名
			PhotonTargets.All,          // ネットワークプレイヤー全員に対して呼び出す
			args);                      // 引数
	}

	[PunRPC]
	public void Move(float move, float speed)
	{
		// 死んでない時のみジャンプ可能
		if (m_state != State.Death) {
			if (Mathf.Abs (move) > 0) {
				Quaternion rot = transform.rotation;
				transform.rotation = Quaternion.Euler (rot.x, Mathf.Sign (move) == 1 ? 0 : 180, rot.z);
			}

			m_rigidbody2D.velocity = new Vector2(move * speed, m_rigidbody2D.velocity.y);
		}
	}

	/// <summary>
	/// コインを獲得した時に呼ばれる
	/// </summary>
	public void OnPlayer1GetCoin()
	{
		PlayerInfo.Instance.Player1GetCoin++;
	}

	/// <summary>
	/// コインを獲得した時に呼ばれる
	/// </summary>
	public void OnPlayer3GetCoin()
	{
		PlayerInfo.Instance.Player3GetCoin++;
	}

	/// <summary>
	/// プレイヤーがダメージをくらった時に呼び出される
	/// ダメージをくらうのは、
	/// ・画面外に落ちた時
	/// ・相手のプレイヤーに踏まれた時
	/// </summary>
	public void OnDamaged(){
		Debug.Log("ダメージを食らいました");
		// ライフを1へらす
		if (gameObject.name == "Player_1" && m_state.Equals(State.Normal)) {
			m_state = State.Invincible;
			OnDamagerPlayerAnim(gameObject);
			PlayerInfo.Instance.Player1LeftLife--;
			if (PlayerInfo.Instance.Player1LeftLife == 0) {
				OnDeath ();
			}
		} else if (gameObject.name == "Player_2" && m_state.Equals(State.Normal)) {
			m_state = State.Invincible;
			OnDamagerPlayerAnim(gameObject);
			PlayerInfo.Instance.Player2LeftLife--;   
			if (PlayerInfo.Instance.Player2LeftLife == 0) {
				OnDeath ();
			}
		}
	}

	private void OnDamagerPlayerAnim(GameObject targetObject)
	{
		Sequence sequence = DOTween.Sequence ();
		sequence.Append (
			targetObject.transform.DOLocalMoveY (0, 0.3f)
		);
		sequence.Append (
			targetObject.transform.DOLocalMoveY(-0.6f, 0.3f)
		);
		sequence.AppendInterval (0.5f);
		sequence.OnComplete (() => SetPlayerPosition (targetObject));
	}

	private void SetPlayerPosition(GameObject targetObject)
	{
		Debug.Log ("targetObject.transform.localPosition : " + targetObject.transform.localPosition);
		targetObject.transform.localPosition = new Vector3 (targetObject.transform.localPosition.x, 0.4f, targetObject.transform.localPosition.z);
		m_state = State.Normal;
	}

	/// <summary>
	/// 残機数が０になったときに呼ばれる
	/// </summary>
	public void OnDeath()
	{
		Debug.Log("ライフが０になり死にました");

		// 下記の感じの挙動になる？
		// m_state = State.Death;
	}
}
