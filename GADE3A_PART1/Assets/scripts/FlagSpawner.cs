using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSpawner : MonoBehaviour
{
    public GameObject flagPrefab; 
    public void SpawnFlag()
    {
        Instantiate(flagPrefab, transform.position, Quaternion.identity);
    }
}
