using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuUI : MonoBehaviour
{
    private void Awake() => Time.timeScale = 1f;

    private void Start() => AudioManager.PlayMainMusic();

    public void ExitGame() => Application.Quit();
}
