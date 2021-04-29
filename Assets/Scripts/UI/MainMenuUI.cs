using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuUI : MonoBehaviour
{
    private void Awake() => Time.timeScale = 1f;

    private void Start()
    {
        Cursor.visible = true;
        AudioManager.PlayMainMusic();
        AudioManager.PlayAmbientAudio();
    }

    public void ExitGame() => Application.Quit();
}
