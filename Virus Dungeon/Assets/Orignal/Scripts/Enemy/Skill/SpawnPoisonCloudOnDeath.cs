using UnityEngine;

public class SpawnPoisonCloudOnDeath : MonoBehaviour
{
    public GameObject prefab;

    public void Spawn()
    {
        var cloud = Instantiate(prefab, transform.parent);
        cloud.transform.position = transform.position;
        Destroy(cloud, 6);
    }
}
