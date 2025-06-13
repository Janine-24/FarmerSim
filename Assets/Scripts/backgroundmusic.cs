using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    public AudioClip bgmClip; 
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private static MusicToggle instance;
    private AudioSource bgmSource;
    private bool isMuted;
    private Image buttonImage;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 清理监听
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // find button in the new scene
        GameObject btnObj = GameObject.Find("music onoff");
        if (btnObj != null)
        {
            buttonImage = btnObj.GetComponent<Image>();

            // add Onclick
            Button btn = btnObj.GetComponent<Button>();
            btn.onClick.RemoveAllListeners(); // remove any existing listeners
            btn.onClick.AddListener(ToggleMusic);

            UpdateButtonImage();
        }
        else
        {
            buttonImage = null;
        }
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
