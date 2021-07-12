using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    public List<GameObject> defaultPlanets;
    
    //Order is important
    public List<GameObject> premiumPlanets;

    private Dictionary<string, float> fuelChance = new Dictionary<string, float>
    {
        {"Amerald", 1.0f},
        {"Arkas", 0.4f},
        {"Bistar", 0.33f},
        {"Dragon", 0.33f},
        {"Gataca", 0.33f},
        {"Gerta", 0.8f},
        {"Hoth", 0.33f},
        {"Huazes", 0.4f},
        {"Hypatia", 0.6f},
        {"Jaspet", 0.4f},
        {"Manthra", 0.4f},
        {"Netou", 0.33f},
        {"Sugah", 1.0f},
        {"Tadmor", 0.8f},
        {"Teratoma", 0.6f},
        {"Titawin", 1.0f},
        {"Tockitee", 0.6f},
        {"Vendara", 1.0f},
        {"Wadow", 0.8f},
        {"Xagobah", 0.6f},
    };

    void Start()
    {
        List<GameObject> allAvaliable = new List<GameObject>();
        allAvaliable.AddRange(defaultPlanets);
        
        foreach (GameObject premiumPlanet in premiumPlanets)
        {
            if (PlayerPrefs.GetInt(premiumPlanet.tag, 0) == 1)
            {
                allAvaliable.Add(premiumPlanet);
            }
        }
        
        int randomIndex = Random.Range(0, allAvaliable.Count);
        GameObject planet = Instantiate(
            allAvaliable[randomIndex], 
            new Vector3(transform.position.x,transform.position.y, transform.position.z), 
            Quaternion.identity);
        
        planet.transform.parent = transform;
        planet.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);

        // set fuel chance
        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            float fuel = fuelChance[planet.tag];
            float rand = Random.value;
            if (rand <= fuel)
            {
                transform.parent.GetComponent<Planet>().fuel = true;
            }
            else
            {
                transform.parent.GetComponent<Planet>().fuel = false;
            }
        }
        else
        {
            transform.parent.GetComponent<Planet>().fuel = true;
        }
        
    }
}
