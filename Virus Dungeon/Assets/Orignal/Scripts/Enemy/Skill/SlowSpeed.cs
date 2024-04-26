using UnityEngine;

public class SlowSpeed : MonoBehaviour
{
    public float slowDown = 3;
    public float duration = 1.5f;

    public void Slow(PlayerBehaviour player)
    {
        player.move.SlowSpeed(duration, slowDown);
    }
}
