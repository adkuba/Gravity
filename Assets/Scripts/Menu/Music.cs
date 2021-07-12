using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioClip mainLoop;
    private AudioSource musicSource;
    private List<int> playedMusic = new List<int>();

    void Start()
    {
        musicSource = GetComponent<AudioSource>();

        //max 1 music controller
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Music");
        if (gameObjects.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

   
    void Update()
    {
        if (!musicSource.isPlaying && PlayerPrefs.GetInt("canPlayMusic", 1) == 1)
        {
            //all music played
            if (playedMusic.Count == audioClips.Length)
            {
                playedMusic.Clear();
            }
            //90% chance of main loop
            //10% chance of music
            float random = Random.value;
            if (random <= 0.9f)
            {
                musicSource.clip = mainLoop;
                musicSource.Play();
            }
            else
            {
                int index = Random.Range(0, audioClips.Length);
                while (playedMusic.Contains(index))
                {
                    index = Random.Range(0, audioClips.Length);
                }
                musicSource.clip = audioClips[index];
                musicSource.Play();
                playedMusic.Add(index);
            }
        }
        if (musicSource.isPlaying && PlayerPrefs.GetInt("canPlayMusic", 1) == 0)
        {
            musicSource.Stop();
        }
    }
}
