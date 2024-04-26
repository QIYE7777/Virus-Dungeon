using UnityEngine;
using UnityEngine.UI;

public class HudBehaviour : MonoBehaviour
{
    public Slider healthSlider;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    public static HudBehaviour instance;
    bool damaged;

    private void Awake()
    {
        instance = this;
    }

    public void OnDamaged()
    {
        damaged = true;
    }

    public void SetHpBar(float v)
    {
        healthSlider.value = v;
    }

    void Update()
    {
        if (damageImage != null)
        {
            if (damaged)
                damageImage.color = flashColour;
            else
                damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        damaged = false;
    }
}
