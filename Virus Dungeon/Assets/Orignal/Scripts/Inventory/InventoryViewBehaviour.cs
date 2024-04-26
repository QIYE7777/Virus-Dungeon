using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class InventoryViewBehaviour : MonoBehaviour
{
    public static InventoryViewBehaviour instance;

    public Text goldText;

    private void Awake()
    {
        instance = this;
    }

    public void Sync()
    {
        goldText.text = "Gold " + InventorySystem.instance.gold;
    }
}
