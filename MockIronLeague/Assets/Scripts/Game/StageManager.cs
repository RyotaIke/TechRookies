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
	}
}
