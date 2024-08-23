using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactiveMe : MonoBehaviour
{
    public float timeToDeactive;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (timeToDeactive != 0)
        {
            yield return new WaitForSeconds(timeToDeactive);
            gameObject.SetActive(false);
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisableMe()
    {
        gameObject.SetActive(false);
    }
}
