using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    public PlayerBehaviour host;

    private void Awake()
    {
        host = GetComponentInParent<PlayerBehaviour>();
    }
}
