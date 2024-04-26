using UnityEngine;

public class Ticker : MonoBehaviour
{
    public float TickTime;
    float _nextTs;

    protected virtual void Update()
    {
        if (Time.time >= _nextTs)
        {
            _nextTs = Time.time + TickTime;
            Tick();
        }
    }

    protected virtual void Tick()
    {
    }
}