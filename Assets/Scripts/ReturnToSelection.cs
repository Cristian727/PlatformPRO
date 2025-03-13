using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToSelection : MonoBehaviour
{
    public void GoToLevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
