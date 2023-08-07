using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            if (s.name == "CoinReceived" /*|| s.name == "CoinGiven"*/)
            {
                s.source.loop = true;
            }

        }
    }


    public void Play(string soundNAME)
    {
        Sound S = Array.Find(sounds, sound => sound.name == soundNAME);
        S.source.Play();
    }

    public void Stop(string NAME)
    {
        Sound ss = Array.Find(sounds, sound => sound.name == NAME);
        ss.source.Stop();
    }
}
