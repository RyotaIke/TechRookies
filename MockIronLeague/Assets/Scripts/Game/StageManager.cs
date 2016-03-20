using UniRx;
using UnityEngine;
using System.Collections;

/// <summary>
/// ステージの情報を管理する
/// </summary>
public class StageManager : ObservableMonoBehaviour {

	// グリッドの縦横それぞれのマス目の数
	private int yGridAmount = 10;
	private int xGridAmount = 100;
	// マス目の情報を管理する（左上原点）
	private int[,] grid;

	// Use this for initialization
	void Start () {
		initializeGrid ();		
	}

	// gridをすべて0で初期化する
	public void initializeGrid()
	{
		grid = new int[yGridAmount,xGridAmount];
		for (int i = 0; i < xGridAmount; i++) {
			for (int j = 0; j < yGridAmount; j++) {
				grid [j, i] = 0;
			}
		}
	}
		
//	// x軸方向のindexをポジションに変換する（左上原点）
//	public float getPositionByIndexX(int x){}
//
//	// x軸方向のポジションをindexに変換する（左上原点）
//	public int getIndexByPositionX(float x){}
//
//	// y軸方向のindexをポジションに変換する（左上原点）
//	public float getPositionByIndexY(int y){}
//
//	// y軸方向のポジションをindexに変換する（左上原点）
//	public int getIndexByPositionY(float x){}
//
//	// indexをポジションに変換する（左上原点）
//	public int getPositionByIndex(int x, int y){
//		return new Vector3 (
//			getPositionByIndexX(x),
//			getPositionByIndexY(y),
//			0
//		);
//	}

//	public Vector3 getFixedPosition()
//	{
//		Mathf.Round ();
//		result = Math.Round(1.49m, 0, MidpointRounding.AwayFromZero);
//		Console.WriteLine(result); // 出力：1
//	}


}
