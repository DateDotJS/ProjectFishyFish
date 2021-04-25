using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTag : MonoBehaviour
{
    public bool HideDuringPlay = true;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        this.meshRenderer = GetComponent<MeshRenderer>();

        if (HideDuringPlay)
            this.meshRenderer.enabled = false;
    }

}
