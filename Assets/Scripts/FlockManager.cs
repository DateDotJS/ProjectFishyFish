using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    private List<Flock> flocks;
    private GameObject player;

    [SerializeField] private float drawDistance = 10f;

    private void Awake()
    {
        this.flocks = new List<Flock>();
        this.player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        foreach (var flock in this.flocks) {
            if (flock == null) {
                flocks.RemoveAll(f => f == null);
                break;
            }

            CheckFlockDrawDistance(flock);
        }
            
    }

    private void CheckFlockDrawDistance(Flock flock)
    {
        var distance = Vector3.Distance(flock.transform.position, player.transform.position);

        if (distance > this.drawDistance && flock.gameObject.activeSelf)
            flock.gameObject.SetActive(false);

        if (distance < this.drawDistance && !flock.gameObject.activeSelf)
            flock.gameObject.SetActive(true);
    }

    internal void AddFlock(Flock flock)
    {
        this.flocks.Add(flock);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.player.transform.position, this.drawDistance);
    }
}
