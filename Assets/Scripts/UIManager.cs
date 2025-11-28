using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManager:MonoBehaviour 
{
    public static UIManager instance;
    public GameObject handCursor;
    public GameObject BackImage;
    public TextMeshProUGUI captionText;
    public Image interacactionImage; 
    private void Awake()
    {
        
        instance = this;
    }
    public void SetCaptionText(string text)
    {
        captionText.text = text;
    }

    public void SetHandCursor (bool state)
    {
        handCursor.SetActive(state);
    }

    public void SetBackImage(bool state)
    {
        BackImage.SetActive(state);
        if (!state) {
            interacactionImage.enabled = false;
                
        }
    }

    public void SetImage(Sprite sprite)
    {
        interacactionImage.sprite = sprite;
        interacactionImage.enabled = true;
    }
}
