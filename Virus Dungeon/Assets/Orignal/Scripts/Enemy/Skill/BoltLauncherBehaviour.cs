using UnityEngine;

public class BoltLauncherBehaviour : MonoBehaviour
{
    public BoltBehaviour boltPrefab;

    BoltBehaviour[] bolts;

    public float deltaUpward = 2;
    public float deltaRadian = 2;
    public float deltaOutside = 3;

    public float durationBeforeRelease = 2;
    public AnimationCurve ac;
    float _releaseTimestamp;
    bool _released;

    private void Start()
    {
        bolts = new BoltBehaviour[3] { null, null, null };
        for (int i = 0; i < 3; i++)
        {
            bolts[i] = Instantiate(boltPrefab, transform.position, Quaternion.identity, transform.parent);
        }

        _releaseTimestamp = com.GameTime.time + durationBeforeRelease;
    }

    private void Update()
    {
        SetPositionOfBolts();
    }

    void SetPositionOfBolts()
    {
        if (_released)
            return;

        var passedTime = _releaseTimestamp - com.GameTime.time;
        var timeRatio = 1 - passedTime / durationBeforeRelease;
        var acRatio = ac.Evaluate(timeRatio);
        if (timeRatio >= 1)
        {
            ReleaseBolts();
            return;
        }

        Vector3 centerPosition = transform.position;
        for (int i = 0; i < bolts.Length; i++)
        {
            var bolt = bolts[i];
            var intialAngle = Mathf.PI * 2f / 3f * i;

            var idealRadian = intialAngle + deltaRadian * acRatio;
            var idealHeight = deltaUpward * acRatio;
            var idealOutSideDistance = deltaOutside * acRatio;

            bolt.transform.position = centerPosition + Vector3.up * idealHeight +
                Vector3.right * Mathf.Sin(idealRadian) * idealOutSideDistance +
                  Vector3.forward * Mathf.Cos(idealRadian) * idealOutSideDistance;
        }
    }

    void ReleaseBolts()
    {
        foreach (var bolt in bolts)
            bolt.Release();

        _released = true;
        Destroy(gameObject);
    }
}