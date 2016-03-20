using UniRx;
using UnityEngine;
using System.Collections;

public class SupportPlayerController : ObservableMonoBehaviour {

	// 全種類のブロックのリスト
	public GameObject[] blocks;
	// ブロックを配置するUI状のポイントのリスト
	public Transform[] spawnPositions;
	// ネクストの場所
	public Transform nextBlockSpawnPoint;

	public bool[] readyBlocks;
	private GameObject nextBlock;

	public GameObject blockParent;
	// UI上に置かれてる時のブロックのスケール
	private Vector3 localBlockScale = new Vector3(0.05f,0.0278f,1f);

	void Awake()
	{
		readyBlocks= new bool[4];
	}

	// Use this for initialization
	void Start () {
		spawnBlock ();

		UpdateAsObservable ()
			.Select (_ => readyBlocks [0])
			.Where (x => (x == false))
			.Subscribe (_ => {
				setBlockOnSpawnPoint(0);
			});

		UpdateAsObservable ()
			.Select (_ => readyBlocks [1])
			.Where (x => (x == false))
			.Subscribe (_ => {
				setBlockOnSpawnPoint(1);
			});

		UpdateAsObservable ()
			.Select (_ => readyBlocks [2])
			.Where (x => (x == false))
			.Subscribe (_ => {
				setBlockOnSpawnPoint(2);
			});

		UpdateAsObservable ()
			.Select (_ => readyBlocks [3])
			.Where (x => (x == false))
			.Subscribe (_ => {
				setBlockOnSpawnPoint(3);
			});
	}

	/// <summary>
	/// nextにあるブロックを空いている場所にセットする
	/// </summary>
	public void setBlockOnSpawnPoint(int i)
	{
		if (nextBlock != null) {
			nextBlock.GetComponent<BlockParentBase> ().IsDrackable = true;
			nextBlock.GetComponent<BlockParentBase> ().SpawnPosition = i;
			nextBlock.transform.position = spawnPositions [i].position;
			nextBlock = null;
			readyBlocks[i] = true;
			spawnBlock ();
		}
	}

	/// <summary>
	/// blockの中からランダムで１つブロックを選んでnextの場所に設置する
	/// </summary>
	public void spawnBlock()
	{
		GameObject block = (GameObject)Instantiate (
			blocks[Random.Range (0, blocks.Length)],
			nextBlockSpawnPoint.position,
			Quaternion.identity
		);
		block.transform.SetParent (blockParent.transform);
		block.transform.localScale = localBlockScale;
		nextBlock = block;
	}
}
