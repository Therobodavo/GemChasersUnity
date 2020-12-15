using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * FacePlayer Class
 * Programmed by David Knolls
 * 
 * Forces Object to look at player
 */

public class FacePlayer : MonoBehaviour
{
    GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (player) 
        {
            transform.rotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
        }
        
    }
}
