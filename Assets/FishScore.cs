using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishScore : MonoBehaviour
{
    public FishType FishType;

    private Text text;

    private void Awake()
    {
        this.text = GetComponent<Text>();
    }

    private void Start()
    {
        SetScore(0);
    }

    public void SetScore(int value)
    {
        this.text.text = $"{value}";
    }
}
