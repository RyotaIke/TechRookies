using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// キャラクターの操作用
/// 移動とジャンプのみを担当
/// </summary>
public class PlayerController : Photon.MonoBehaviour
{
	public Button rightButton;
	public Button leftButton;
	public Button jumpButton;

	public float speed     = 4f;
	public float jumpPower = 500f;

	public PlayerManager player;
	[SerializeField]
	private GameObject gameController;

	[SerializeField]
	private PlayerManager player1Manager;
	[SerializeField]
	private PlayerManager player2Manager;

	void Start()
	{
		switch (PlayerInfo.Instance.playerType) {
		case PlayerInfo.PlayerType.PLAYER_1:
			player = player1Manager;
			break;
		case PlayerInfo.PlayerType.PLAYER_2:
			break;
		case PlayerInfo.PlayerType.PLAYER_3:
			player = player2Manager;
			break;
		case PlayerInfo.PlayerType.PLAYER_4:
			break;
		}

		rightButton.OnPointerDownAsObservable ().Subscribe (_ => {
				player.rpcMove(1,speed);
			});

		rightButton.OnPointerUpAsObservable ()
			.Subscribe (_ => {
				player.rpcMove(0,speed);
			});

		leftButton.OnPointerDownAsObservable ().Subscribe (_ => {
				player.rpcMove(-1,speed);
			});

		leftButton.OnPointerUpAsObservable ()
			.Subscribe (_ => {
				player.rpcMove(0,speed);
			});

		jumpButton.OnPointerDownAsObservable ()
			.Subscribe (_ => {
				player.rpcJump(jumpPower);
			});
	}
} 