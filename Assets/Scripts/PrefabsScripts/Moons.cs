using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moons : MonoBehaviour
{
    private Vector3 rotation;


    void Start()
    {
        int moon1 = Random.Range(0,2);
        
        if (moon1 == 0)
        {
            Destroy(transform.GetChild(1).gameObject);
        }
        else
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        //values 0 or 1!
        if (Random.Range(0, 2) == 0)
        {
            rotation = new Vector3(0, 0, 1);
        }
        else
        {
            rotation = new Vector3(0, 0, -1);
        }

        transform.localScale = new Vector3(0.083f, 0.083f, 0.083f);
    }


    void Update()
    {
        transform.RotateAround(transform.parent.position, rotation, 30 * Time.deltaTime);
    }
}
