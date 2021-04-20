using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void SetScene(int indexScene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(indexScene);
    }
}
