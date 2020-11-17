using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player) 
        {
            transform.rotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
        }
        
    }
}
