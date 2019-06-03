using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayerScript : MonoBehaviour
{
    public AudioClip mainMenu;
    public AudioClip mainMenuIntro;
    public AudioClip bgmIntro;
    public AudioClip bgm;
    public AudioClip gameOver;
    public AudioClip gameOverIntro;

    private AudioSource audioSource;
    private static AudioPlayerScript audioSingleton;

    void OnEnable()
    {
        //attach method to scene load event
        SceneManager.sceneLoaded += OnSceneLoaded;
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        DontDestroyOnLoad(gameObject);
        if (audioSingleton == null)
        {
            audioSingleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //play music on scene load
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        switch (scene.name)
        {
            case "MainMenu":
                PlayMenuMusic();
                break;
            case "MainGame":
                PlayGameBGM();
                break;
        }
    }

    void Start()
    {

    }

    public void PlayGameBGM()
    {
        StartCoroutine(PlayBGMIntro());
    }

    IEnumerator PlayBGMIntro()
    {
        audioSource.clip = bgmIntro;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        //ensures that second part after intro does not override new music
        if (audioSource.clip.Equals(bgmIntro))
        {
            audioSource.clip = bgm;
            audioSource.Play();
        }
    }

    public void PlayGameOverMusic()
    {
        audioSource.clip = gameOver;
        audioSource.Play();
    }

    public void PlayMenuMusic()
    {
        StartCoroutine(PlayMenuIntro());
    }

    IEnumerator PlayMenuIntro()
    {
        audioSource.clip = mainMenuIntro;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length - 1f);
        //ensures that second part after intro does not override new music
        if (audioSource.clip.Equals(mainMenuIntro))
        {
            audioSource.clip = mainMenu;
            audioSource.Play();
        }
    }
}