using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤーの情報を管理するクラス
/// </summary>
public class PlayerManager : MonoBehaviour {

	[SerializeField]
	private GameController gameController;
	[SerializeField]
	private int life = 5;
	public int Life
	{
		set { this.life = value; }
		get { return this.life; }
	}

	// 地面に立っているかどうかを判定するためのレイヤー
	public LayerMask whatIsGround;
	// 地面に立っているかどうか
	private bool isGround = true;

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
			.Subscribe (_ => onDeath ());

		// ゴールに浮いたらOnGameFinishを呼ぶ
		this.OnTriggerEnter2DAsObservable ()
			.Where (coll => LayerMask.LayerToName (coll.gameObject.layer) == "Goal")
			.Subscribe (_ => gameController.OnGameFinish());
	}

	void FixedUpdate()
	{
		Debug.Log ("状態チェック : " + m_rigidbody2D.velocity);
		changeAnimation ();
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
		Debug.Log ("heihei " + isGround);
		// 死んでない かつ 地面に接してる時のみジャンプ可能
		if (m_state != State.Death && isGround) {
			Debug.Log ("hei");
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
	/// プレイヤーのが画面外に落ちて死んだ時に呼ばれる
	/// </summary>
	public void onDeath(){
		Debug.Log("死にました");
		life -= 1;
	}
}
