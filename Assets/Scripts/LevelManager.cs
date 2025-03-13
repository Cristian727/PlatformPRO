using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levelButtons;


    private void Awake()
    {
        //PlayerPrefs.SetInt("lvl2", 0);
        //PlayerPrefs.SetInt("lvl1", 1);
        //print(PlayerPrefs.GetInt("lvl1"));
        //print(PlayerPrefs.GetInt("lvl2"));

        //if (PlayerPrefs.GetInt("lvl1") == 0)
        //{
        //    level1.SetActive(false);
        //}
        //if (PlayerPrefs.GetInt("lvl2") == 0)
        //{
        //    level2.SetActive(false);
        //}

        for (int i = 1; i <= levelButtons.Length; i++)
            if (PlayerPrefs.GetInt("lvl" + i) == 0)
                levelButtons[i].SetActive(false);
    }


    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
