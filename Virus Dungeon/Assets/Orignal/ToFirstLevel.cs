using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToFirstLevel : MonoBehaviour
{
    public void ToLevel_1()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit ();
    }
}
