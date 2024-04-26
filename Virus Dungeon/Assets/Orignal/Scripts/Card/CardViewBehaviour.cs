using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class CardViewBehaviour : MonoBehaviour
{
    Image _img;
    public Text title;
    public Text price;
    public ParticleSystem ps;

    private void Awake()
    {
        _img = GetComponent<Image>();
    }

    public void Setup(CardConfig cfg)
    {
        _img.sprite = cfg.sp;
        title.text = cfg.title;
        price.text = cfg.price + "";
    }

    public void SetTransparent()
    {
        _img.color = new Color(1, 1, 1, 0.25f);
    }

    public void SetNonTransparent()
    {
        _img.color = new Color(1, 1, 1, 1f);
    }

    public void OnStartDrag()
    {
        ps.Play();
    }

    public void OnStopDrag()
    {
        ps.Stop();
    }
}