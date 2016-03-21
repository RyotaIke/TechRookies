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
	/// プレイヤー1の残機
	/// </summary>
	public int player1LeftLife;
	public int Player1LeftLife
	{
		get{
			return player1LeftLife;
		}
		set{
			player1LeftLife = value;
		}
	}

	/// <summary>
	/// プレイヤー1の残機
	/// </summary>
	public int player2LeftLife;
	public int Player2LeftLife
	{
		get{
			return player2LeftLife;
		}
		set{
			player2LeftLife = value;
		}
	}

	/// <summary>
	/// プレイヤー
	/// </summary>
	public int player1GetCoin;
	public int Player1GetCoin 
	{
		get {
			return player1GetCoin;
		}
		set {
			player1GetCoin = value;
		}
	}

	/// <summary>
	/// プレイヤー
	/// </summary>
	public int player3GetCoin;
	public int Player3GetCoin 
	{
		get {
			return player3GetCoin;
		}
		set {
			player3GetCoin = value;
		}
	}

	/// <summary>
	/// The player names.
	/// </summary>
	public string[] playerNames = new string[4];
}
