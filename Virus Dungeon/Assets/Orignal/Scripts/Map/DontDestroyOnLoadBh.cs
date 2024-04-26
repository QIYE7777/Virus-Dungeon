using UnityEngine;

public class DontDestroyOnLoadBh : MonoBehaviour
{
    public static DontDestroyOnLoadBh uniqueInstance;

    private void Awake()
    {
        uniqueInstance = this;
        DontDestroyOnLoad(this);

        //  if (uniqueInstance == null)
        //  {
        //      DontDestroyOnLoad(this);
        //      uniqueInstance = this;
        //  }
        //  else
        //  {
        //      Destroy(uniqueInstance.gameObject);
        //      DontDestroyOnLoad(this);
        //      uniqueInstance = this;
        //  }

    }
}