using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGLight : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(new Vector3(1, 0, 0), 10 * Time.deltaTime);
    }
}
