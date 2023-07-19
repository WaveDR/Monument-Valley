using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class Sound_Manager : MonoBehaviour
{

    public static Sound_Manager Instance =null;

    public AudioSource[] audioSource_Effects;
    public AudioSource audioSound_Bgm;

    public Sound[] effectSound;
    public Sound[] bgmSound;

    public string[] play_SoundName;



    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Play_SB("BGM_Main");
    }


    public void PlaySE(string _Name)
    {
        for (int i = 0; i < effectSound.Length; i++)
        {
            if(_Name == effectSound[i].name)
            {
                for (int j = 0; j < audioSource_Effects.Length; j++)
                {
                    if (!audioSource_Effects[j].isPlaying)
                    {
                        play_SoundName[j] = effectSound[i].name;
                        audioSource_Effects[j].clip = effectSound[i].clip;
                        audioSource_Effects[j].Play();
                        return;
                    }
                }
                Debug.Log("��� ���� AudioSource�� ��� ���Դϴ�.");
                return;
            }
        }
        Debug.Log(_Name + "���尡 Manager�� ��ϵ��� �ʾҽ��ϴ�");
    }
    public void Play_SB(string _Name)
    {
        for (int i = 0; i < bgmSound.Length; i++)
        {
            if (_Name == bgmSound[i].name)
            {
                if (!audioSound_Bgm.isPlaying)
                {
                    audioSound_Bgm.clip = bgmSound[i].clip;
                    audioSound_Bgm.Play();
                    return;
                }
            }
        }
        Debug.Log(_Name + "���尡 Manager�� ��ϵ��� �ʾҽ��ϴ�");
    }

    public void StopAll_SFX()
    {
        for (int i = 0; i < audioSource_Effects.Length; i++)
        {
            audioSource_Effects[i].Stop();
        }
    }

    public void StopAll_BGM()
    {
        audioSound_Bgm.Stop();
    }
    public void Stop_SFX(string _Name)
    {
        for (int i = 0; i < audioSource_Effects.Length; i++)
        {
            if (play_SoundName[i] == _Name)
            {
                audioSource_Effects[i].Stop();
                return;
            }
        }
        Debug.Log("������� " + _Name + "���尡 �����ϴ�.");
    }
}
