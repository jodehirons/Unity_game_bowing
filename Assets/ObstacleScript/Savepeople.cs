using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savepeople : MonoBehaviour
{
    public float controlTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        controlTime += Time.deltaTime;
        if(controlTime> 3)
        {
            Destroy(gameObject);
        }
    }
}
