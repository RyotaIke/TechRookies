using UniRx;
using UnityEngine;
using System.Collections;

/// <summary>
/// ステージの情報を管理する
/// グリッドもここ
/// </summary>
public class StageManager : ObservableMonoBehaviour {

	// グリッドの縦横それぞれのマス目の数
	private int xGridAmount = 10;
	private int yGridAmount = 100;
	// マス目の情報を管理する（左上原点）
	private int[,] grid;

	// Use this for initialization
	void Start () {
		initializeGrid ();		
	}

	/// <summary>
	/// ステージのサイズを元にグリッドを初期化する
	/// </summary>
	public void initializeGrid()
	{
		//ステージのサイズを元にグリッドの１辺のサイズを計算
		float gridSize  = gameObject.transform.lossyScale.x / xGridAmount;
		//グリッドのサイズを元にyのグリッド数を計算
		int yGridAmount = (int)Mathf.Floor(gameObject.transform.lossyScale.y / gridSize);

		grid = new int[yGridAmount,xGridAmount];

		grid = new int[yGridAmount,xGridAmount];
		for (int i = 0; i < xGridAmount; i++) {
			for (int j = 0; j < yGridAmount; j++) {
				grid [j, i] = 0;
			}
		}


		//printGrid ();
	}

	/// <summary>
	/// debug用ファンクション
	/// </summary>
	public void printGrid()
	{
		for (int i = 0; i < xGridAmount; i++) {
			for (int j = 0; j < yGridAmount; j++) {
				Debug.Log("grid["+j+","+i+"] = " + grid [j, i]);
			}
		}
	}

//	public int getIndexByPosition(Vector2 position)
//	{
//		
//	}

//	public bool isValidPosition()
//	{
//		// そもそも画面の幅に収まっているかどうか
//		// 
//	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns>The grid size.</returns>
//	public float getGridSize()
//	{
//		
//	}
		
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
