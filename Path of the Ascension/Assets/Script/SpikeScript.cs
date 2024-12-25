using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {   
        if(other.gameObject.layer == 3)
        {
            if (player != null)
            {
                player.SetActive(false);
            }
            else
            {
                Debug.LogError("You Died");
            }
        }
    }
}
