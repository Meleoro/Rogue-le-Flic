using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpawn : MonoBehaviour
{

    public AudioSource sonSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SonSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator SonSpawn()

    {
        yield return new WaitForSeconds(2.1f);
        sonSpawn.Play();
    }
}
