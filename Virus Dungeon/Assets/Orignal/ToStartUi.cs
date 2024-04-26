using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToStartUi : MonoBehaviour
{
    public void ReStartGame()
    {
        Destroy(DontDestroyOnLoadBh.uniqueInstance.gameObject);
        EnemyIdentifier.ClearEnemyList();
        SceneManager.LoadScene(0); 
    }


}
