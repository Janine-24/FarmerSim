using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    public AudioSource bgmSource;        
    public Sprite soundOnSprite;        
    public Sprite soundOffSprite;        
    public Image buttonImage;
    private static MusicToggle instance;

    private bool isMuted = false;

    private void Awake()
    {
        // ensure this is a singleton instance
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // prevent the music duplicated when change scene
            return;
        }
        // set as singleton instance
        instance = this;
        DontDestroyOnLoad(gameObject); // not destroy when change scene  
        Debug.Log("设为 DontDestroyOnLoad 成功");

        isMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;

        if (bgmSource != null)
        {
            bgmSource.loop = true;
            bgmSource.mute = isMuted;

            if (!isMuted && !bgmSource.isPlaying) //if bgm playing, dont stop
            {
                bgmSource.Play();
            }
        }
    }
    void Start()
    {
        UpdateButtonImage();
    }

    public void ToggleMusic()
    {
        isMuted = !isMuted;
        bgmSource.mute = isMuted;

        if (!isMuted && !bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
        //save status to prevent when change scene restart the music
        PlayerPrefs.SetInt("MusicMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        UpdateButtonImage();
    }

    private void UpdateButtonImage()
    {
        if (buttonImage != null)
            buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
    }
}
