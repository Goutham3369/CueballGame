using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DebriHandler : MonoBehaviour
{
    public List<GameObject> Debris = new List<GameObject>();
    // Start is called before the first frame update
    public void ActivateDebris()
    {
        gameObject.SetActive(true);
        for (int i = 0; i < Debris.Count; i++)
        {
            if (Debris[i] == null) return;
            Debris[i].GetComponent<Rigidbody>().AddForce(new Vector3(0,Random.Range(20000,100000),0) * Time.deltaTime);
            Destroy(Debris[i],1.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
