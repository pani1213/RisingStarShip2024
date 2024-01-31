using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;
public enum AudioMode { FX,BGM}
public class AudioManager : Singleton<AudioManager>
{
    // 1. Resurces 폴더의 BGM,FX에 접근
    // 2. 스크립트 하나로 구현


    public static AudioManager audioManager = null;

    private Dictionary<string, AudioClip> fxContainer = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> bgmContainer = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> clipContainer = null;

    private float BGM_Volume = 0.5f,FX_Volume = 0.15f;

    string clipPath = "";
    bool isNonePlayClip = false;
    AudioSource bgmSource = null;
    AudioSource fxSource = null;
    private List<AudioSource> fxSources = new List<AudioSource>();

    public void InIt()
    {
        audioManager = this;
        // 오디
    }
    public void PlayBgmClip(string _bgmName)
    {
        if (bgmSource == null)
        {
            GameObject obj = new GameObject("Bgm_Clop");
            obj.transform.parent = this.transform;
            bgmSource = obj.AddComponent<AudioSource>();
            bgmSource.volume = BGM_Volume;
            bgmSource.loop = true;


        }
         bgmSource.clip = GetAudioClip(AudioMode.BGM,_bgmName);
         bgmSource.Play();
    }
    public void BGM_Value(Slider _slider)
    {
        if (bgmSource != null)
        { 
            BGM_Volume = _slider.value;
            bgmSource.volume = BGM_Volume;
        }
    }
    public void FX_Value(Slider _slider)
    {
        if (fxSource != null)
        { 
            FX_Volume = _slider.value;
            fxSource.volume = FX_Volume;
        }
    }
    public void PauseBgmClip()
    {
        bgmSource.Pause();
    }
    public void StopBgmClip()
    {
        bgmSource.Stop();
    }
    public void PlayFxClip(string _fxName) 
    {
        isNonePlayClip = true;
        if (fxSources.Count <= 0)
        {
            AddFxClip();
        }
        for (int i = 0; i < fxSources.Count; i++)
        {
            if (!fxSources[i].isPlaying)
            {
                isNonePlayClip = false;
                fxSource = fxSources[i];
                break;
            }
        }
        if (isNonePlayClip)
        {
            AddFxClip();
        }
        fxSource.clip = GetAudioClip(AudioMode.FX, _fxName);
        fxSource.volume = FX_Volume;
        fxSource.Play();
    }
    private void AddFxClip()
    {
        GameObject fx = new GameObject("fx_Clip");
        fx.transform.parent = this.transform;
        fxSource = fx.AddComponent<AudioSource>();
        
        fxSources.Add(fxSource);
    }
    private AudioClip GetAudioClip(AudioMode _mode,string _clipName)
    {
        if (_mode == AudioMode.FX)
        {
            clipContainer = fxContainer;
            clipPath = "FX/";
        }
        else
        {
            clipContainer = bgmContainer;
            clipPath = "BGM/";
        }

        if (!clipContainer.ContainsKey(_clipName))
        {
            clipContainer.Add(_clipName, Resources.Load<AudioClip>(clipPath + _clipName));
        }
        return clipContainer[_clipName];
    }
}
