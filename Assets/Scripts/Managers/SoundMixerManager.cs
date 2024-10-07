using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
	[SerializeField] private AudioMixer audioMixer;

	//Clay's crude hack
	[SerializeField] AudioSource _musicSource = null;
	[SerializeField] AudioSource _FXSource = null;


	public void SetMasterVolume(float level)
	{
		
		//audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
	}
	
	public void SetSoundFXVolume(float level)
	{
		_FXSource.volume = level;
		//audioMixer.SetFloat("FXVolume", Mathf.Log10(level) * 20f);
	}
	
	public void SetMusicVolume(float level)
	{

		_musicSource.volume = level;
		//audioMixer.SetFloat("MusVolume", Mathf.Log10(level) * 20f);
	}
}
 