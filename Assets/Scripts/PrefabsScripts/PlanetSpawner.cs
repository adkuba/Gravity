using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    public List<GameObject> defaultPlanets;
    
    //Order is important
    public List<GameObject> premiumPlanets;

    void Start()
    {
        int randomIndex = Random.Range(0, defaultPlanets.Count);
        GameObject planet = Instantiate(
            defaultPlanets[randomIndex], 
            new Vector3(transform.position.x,transform.position.y, transform.position.z), 
            Quaternion.identity);
        
        planet.transform.parent = transform;
        planet.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
    }
}
