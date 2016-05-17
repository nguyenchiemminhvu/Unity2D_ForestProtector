using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {

	public GameObject[] SoundFX;

	public GameObject[] SoundBG;

	public AudioSource[] SourceFX;
	
	public static bool SoundMusic = true;
	public static bool SoundSfx = true;
	public static Sound Instance { get; private set; }
	void Awake()
	{
		Instance = this;
		//DontDestroyOnLoad(transform.gameObject);
	}
	// Use this for initialization
	void Start () 
	{
		SourceFX = new AudioSource[SoundFX.Length];
		for(int i = 0 ; i < SoundFX.Length ; i++)
			SourceFX[i] = SoundFX [i].GetComponent<AudioSource>();
		SoundStopSfx ();
	}
	
	// Update is called once per frame
	void Update () {
		//if(GameLoop.PauseGame && SoundBG.audio.isPlaying)
			//SoundBG.audio.Stop();
	}

	public void PlaySoundBG(int index)
	{
		if(SoundMusic && !SourceFX[index].isPlaying)
			SourceFX[index].Play();

	}

	public void PlaySoundFX(int index)
	{

		if(SoundSfx && !SourceFX[index].isPlaying)
		{
			SourceFX[index].Play();
		}
		
	}
	private AudioSource[] allAudioSources;
	public void SoundStopSfx()
	{

//			allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
//			foreach( AudioSource audioS in allAudioSources) {
//				audioS.Stop();


		for(int i = 0 ; i < SourceFX.Length ; i++)
		{
			SourceFX[i].Stop ();
		}
	}

//	public void SoundStop()
//	{
//		for(int i = 0 ; i < SoundBG.Length ; i++)
//			SoundBG [i].GetComponent<AudioSource>().Stop ();
//	}

}
