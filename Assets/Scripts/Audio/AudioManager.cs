
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    //public static AudioManager Instance { get; private set; }

    [SerializeField] EventReference musicEventMenu; // Reference to the music event in FMOD
    [SerializeField] EventReference musicEventGame; // Reference to the music event in FMOD
    [SerializeField] float rate;
    // [SerializeField] EventReference sfxEvent; // Reference to the SFX event in FMOD  


    private EventInstance musicInstance;

    void Awake()
    {
        /* if (Instance != null) { Destroy(gameObject); return; }
         Instance = this;
         DontDestroyOnLoad(gameObject);*/

    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == LoadScenes.SceneName.Lobby.ToString() ||
            SceneManager.GetActiveScene().name == LoadScenes.SceneName.MainMenu.ToString())
        {
            PlayMusic(musicEventMenu);
        }
        else if (SceneManager.GetActiveScene().name == LoadScenes.SceneName.NetworkedPrototype.ToString())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicInstance.release();
            PlayMusic(musicEventGame);
        }
    }

    public void PlayMusic(EventReference eventReference)
    {
        musicInstance = RuntimeManager.CreateInstance(eventReference);
        musicInstance.start();


    }

    void OnDestroy()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
    }

}
