using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnRoadEnterTrigger : MonoBehaviour
{
    CrosswalkManager crosswalkManager;
    void Start()
    {
        crosswalkManager = FindAnyObjectByType<CrosswalkManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        int NPC = Int32.Parse(other.transform.ToString().Substring(0,1));
        Debug.Log("Spawning NPC: "+NPC);
        crosswalkManager.EnableCrosswalk(NPC);
    }
}
