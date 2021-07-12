using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private Vector2 screenBounds;
    private Vector2 widthBounds;
    private Vector2 heightBounds;
    public bool fuel = true;
    private GameObject moon;
    private GameObject player;


    void Start()
    {
        //camera height and width
        screenBounds = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        player = GameObject.FindGameObjectWithTag("Player");

        //fuel is set in PlanetSpawner
        
        //additional effects
        float rand = Random.value;
        bool moonK = false;
        if (rand <= .4f && SceneManager.GetActiveScene().name != "Tutorial")
        {
            moonK = true;
        }

        //moon size
        float moonSize = Random.Range(0.2f, 0.4f);
        moon = transform.GetChild(0).gameObject;
        moon.transform.localScale = new Vector3(moonSize, moonSize, moonSize);

        if (!moonK)
        {
           Destroy(transform.GetChild(0).gameObject);
        }
    }


    void Update()
    {
        //deleting
        widthBounds = new Vector2(Camera.main.transform.position.x - screenBounds.x * 5, Camera.main.transform.position.x + screenBounds.x * 5);
        heightBounds = new Vector2(Camera.main.transform.position.y - screenBounds.y * 7, Camera.main.transform.position.y + screenBounds.y * 7);

        if (transform.position.x < widthBounds.x || transform.position.x > widthBounds.y || transform.position.y < heightBounds.x || transform.position.y > heightBounds.y)
        {
            Destroy(this.gameObject);
        }
    }
}