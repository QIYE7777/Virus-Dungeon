using UnityEngine;

public class ExplodeOnDeath : MonoBehaviour
{
    public GameObject prefab;

    public void Spawn()
    {
        var explode = Instantiate(prefab, transform.parent);
        explode.transform.position = transform.position;
        Destroy(explode, 2);
    }
}
