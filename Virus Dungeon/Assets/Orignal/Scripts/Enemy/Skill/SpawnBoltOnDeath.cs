using UnityEngine;

public class SpawnBoltOnDeath : MonoBehaviour
{
    public BoltLauncherBehaviour boltLauncherPrefab;
    
    public void Spawn()
    {
       var launcher =Instantiate(boltLauncherPrefab, transform.parent);
        launcher.transform.position = transform.position;
    }
}
