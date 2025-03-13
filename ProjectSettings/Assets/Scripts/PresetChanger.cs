using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PresetChanger : MonoBehaviour
{

    [SerializeField] List<Stats> profiles;
    int current = 0;

    PlayerMovement player;

    //[SerializeField] CanvasGroup presetInfoUI;
    //[SerializeField] TMP_Text presetInfoText;
    //[SerializeField] float staticTime = 1;
    //[SerializeField] float fadeTime = 3;
    //Coroutine coroutine = null;

    


    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        SetCurrentMovementProfile();
    }

    void SetCurrentMovementProfile()
    {
        player.stats = profiles[current];
        print(profiles[current].name);
    }


    private void Update()
    {
  
        if (Input.GetKeyDown(KeyCode.C))
        {
            current = (current + 1) % profiles.Count;

            SetCurrentMovementProfile();
        }

    }

}
