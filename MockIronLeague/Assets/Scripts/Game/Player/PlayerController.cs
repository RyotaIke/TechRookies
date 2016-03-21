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

	void Start()
	{
		rightButton.OnPointerDownAsObservable ()
			.Subscribe (_ => {
				player.rpcMove(1,speed);
			});

		rightButton.OnPointerUpAsObservable ()
			.Subscribe (_ => {
				player.rpcMove(0,speed);
			});

		leftButton.OnPointerDownAsObservable ()
			.Subscribe (_ => {
				player.rpcMove(-1,speed);
			});

		leftButton.OnPointerUpAsObservable ()
			.Subscribe (_ => {
				player.rpcMove(0,speed);
			});

		jumpButton.OnPointerDownAsObservable ()
			.Subscribe (_ => {
				player.Jump(jumpPower);
			});
	}
} 