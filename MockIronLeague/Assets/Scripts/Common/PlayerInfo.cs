using UnityEngine;
using System.Collections;

public class PlayerInfo : SingletonMonoBehaviour<PlayerInfo> {

	/// <summary>
	/// プレイヤーのタイプ
	/// </summary>
	public enum PlayerType
	{
		PLAYER_1,
		PLAYER_2,
		PLAYER_3,
		PLAYER_4
	}
	public PlayerType playerType;

	/// <summary>
	/// プレイヤーの名前
	/// </summary>
	public string playerName;
	public string PlayerName
	{
		get {
			return playerName;
		}
		set {
			playerName = value;
		}
	}

	/// <summary>
	/// プレイヤーの残機
	/// </summary>
	public int playerLeftLife;
	public int PlayerLeftLife
	{
		get{
			return playerLeftLife;
		}
		set{
			playerLeftLife = value;
		}
	}

	/// <summary>
	/// プレイヤー
	/// </summary>
	private int playerGetCoin;
	public int PlayerGetCoin 
	{
		get {
			return playerGetCoin;
		}
		set {
			playerGetCoin = value;
		}
	}
}
