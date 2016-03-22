using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BgmManager : SingletonMonoBehaviour<BgmManager> {

	public enum BgmStatus
	{
		TITLE,
		GAME,
		FINISH,
		RESULT
	}
	public BgmStatus bgmStatus;

	[SerializeField]
	private AudioSource[] audioSources;
	[SerializeField]
	private AudioClip[] audioClip;

	public void ChangeBgm()
	{
		Debug.Log (bgmStatus);
		switch (bgmStatus) {
		case BgmStatus.TITLE:
			audioSources [0].DOFade (0, 1f);
			audioSources [1].clip = audioClip [0];
			audioSources [1].Play ();
			audioSources [1].DOFade (1, 1f);
			break;
		case BgmStatus.GAME:
			audioSources [1].DOFade (0, 1f);
			audioSources [0].clip = audioClip [1];
			audioSources [0].Play ();
			audioSources [0].DOFade (1, 1f);
			break;
		case BgmStatus.RESULT:
			audioSources [0].DOFade (0, 1f);
			audioSources [1].clip = audioClip [0];
			audioSources [1].Play ();
			audioSources [1].DOFade (1, 1f);
			break;
		}
	}

}
