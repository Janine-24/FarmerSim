using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    public AudioClip bgmClip; 
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public Image buttonImage;

    private static MusicToggle instance;
    private AudioSource bgmSource;
    private bool isMuted;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // add or get AudioSource
        bgmSource = GetComponent<AudioSource>();
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
        }

        // set play bool
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        // load mute state from PlayerPrefs
        isMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        bgmSource.mute = isMuted;

        if (!isMuted)
        {
            bgmSource.Play();
        }
    }

    private void Start()
    {
        UpdateButtonImage();
    }

    public void ToggleMusic()
    {
        isMuted = !isMuted;
        bgmSource.mute = isMuted;

        if (!isMuted && !bgmSource.isPlaying)
        {
            bgmSource.Play(); // repeat play if not muted
        }

        PlayerPrefs.SetInt("MusicMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        UpdateButtonImage();
    }

    private void UpdateButtonImage()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }
    }
}
