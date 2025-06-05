using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    public AudioSource bgmSource;        
    public Sprite soundOnSprite;        
    public Sprite soundOffSprite;        
    public Image buttonImage;            

    private bool isMuted = false;

    void Start()
    {
        UpdateButtonImage();
    }

    public void ToggleMusic()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            bgmSource.Stop();  
        }
        else
        {
            bgmSource.Play();  // play again
        }

        UpdateButtonImage();
    }

    private void UpdateButtonImage()
    {
        buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
    }
}
