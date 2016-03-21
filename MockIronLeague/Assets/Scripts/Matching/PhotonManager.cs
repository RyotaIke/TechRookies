using PhotonRx;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UniRx; 
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : Photon.MonoBehaviour {

	public MatchingManager matchingManager;

	private RoomInfo[] roomInfo = new RoomInfo[0];

	void Start ()
	{
		//ロビーへの接続処理
		StartCoroutine(ConnectCoroutine());
	}

	private IEnumerator ConnectCoroutine()
	{
		//ロビーへの接続結果を通知するストリームを作成
		//接続成功・失敗のストリームの2つをマージしてどちらかのイベント通知の到達を待つ
		var loginStream = this.OnJoinedLobbyAsObservable().Cast(default(object))
			.Merge(this.OnFailedToConnectToPhotonAsObservable().Cast(default(object)))
			.FirstOrDefault() //OnCompletedを発火させるため
			.PublishLast(); //PublishLastは結果をキャッシュする

		//ConnectでStartAsCoroutineより前にストリームの監視を開始する
		loginStream.Connect();

		//接続開始
		PhotonNetwork.ConnectUsingSettings("0.1");
		StartCoroutine (GetRoomList ());

		//結果保存用のオブジェクト
		var result = default(object);

		//StartAsCoroutineは対象のストリームのOnCompletedが発行されるまでnullを返す（コルーチン上で待機する）
		yield return loginStream.StartAsCoroutine(x => result = x, ex => { });

		//結果の型を見て判定する
		if (result is DisconnectCause)
		{
			Debug.Log("接続失敗");
			//
			// ここに失敗処理を書く
			// 失敗の時は遷移しないとかでもいいかも
			//
			yield break;
		}
		Debug.Log ("roomInfo.Length : " + roomInfo.Length);
		Debug.Log("接続成功");
	}

	//PhotonNetwork.ConnectUsingSettingsを行うと呼ばれる
	void OnJoinedLobby()
	{
		string roomName = "kasahara" + roomInfo.Length;
		//ランダムにルームに入る
		Debug.Log("roomName : " + roomName);
		PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions(){isVisible = true, isOpen = true, maxPlayers = 2}, null);
	}

	private IEnumerator GetRoomList(){
		OnReceivedRoomListUpdate ();
		yield return null;
	}

	void OnReceivedRoomListUpdate() {
		// 既存のRoomを取得.
		roomInfo = PhotonNetwork.GetRoomList();
	}

	//ランダムにルームに入れなかった
	void OnPhotonRandomJoinFailed()
	{
		RoomOptions roomOptions = new RoomOptions ();
		roomOptions.maxPlayers = 2;
		roomOptions.isVisible = true;
		roomOptions.isOpen = true;
		string roomName = "kasahara" + roomInfo.Length;
		Debug.Log("OnPhotonRandomJoinFailed");
		//部屋を自分で作って入る
		PhotonNetwork.CreateRoom(roomName, roomOptions, null);
	}

	/// <summary>
	/// Raises the joined room event.
	/// </summary>
	void OnJoinedRoom()
	{
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++) {
			matchingManager.activatePlayer ("player" + (i+1).ToString());
			PlayerInfo.Instance.playerType = (PlayerInfo.PlayerType)i;
			Debug.Log (PlayerInfo.Instance.playerType);
		}
		// ここでキャラクターの役割変えたい
//		switch (PhotonNetwork.playerList.Length) {
//		case 1:
//			GameObject.Find ("PlayerInfo").GetComponent<PlayerInfo> ().PlayerType = 1;
//			break;
//		case 2:
//			GameObject.Find ("PlayerInfo").GetComponent<PlayerInfo> ().PlayerType = 2;
//			break;
//		case 3:
//			GameObject.Find ("PlayerInfo").GetComponent<PlayerInfo> ().PlayerType = 1;
//			break;
//		case 4:
//			GameObject.Find ("PlayerInfo").GetComponent<PlayerInfo> ().PlayerType = 2;
//			break;
//		default:
//			break;
//		}

		// 4人揃ったらゲーム開始
		if (PhotonNetwork.playerList.Length == 2) {
			matchingManager.startGameCoroutin ();
		}
	}

	/// <summary>
	/// Raises the photon player connected event.
	/// </summary>
	void OnPhotonPlayerConnected()
	{
		matchingManager.activatePlayer ("player" + PhotonNetwork.playerList.Length.ToString ());

		if (PhotonNetwork.playerList.Length == 2) {
			matchingManager.startGameCoroutin ();
		}
	}
}