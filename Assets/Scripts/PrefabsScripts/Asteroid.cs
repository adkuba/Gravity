﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Vector2 screenBounds;
    private Vector2 widthBounds;
    private Vector2 heightBounds;

    private int rotateX;
    private int rotateY;

    void Start()
    {
        screenBounds = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        rotateX = Random.Range(-1, 2);
        rotateY = Random.Range(-1, 2);
    }


    void Update()
    {
        transform.Rotate(new Vector3(rotateX, rotateY), 45 * Time.deltaTime);

        //deleting
        widthBounds = new Vector2(Camera.main.transform.position.x - screenBounds.x * 4, Camera.main.transform.position.x + screenBounds.x * 4);
        heightBounds = new Vector2(Camera.main.transform.position.y - screenBounds.y * 5, Camera.main.transform.position.y + screenBounds.y * 5); 

        if (transform.position.x < widthBounds.x || transform.position.x > widthBounds.y || transform.position.y < heightBounds.x || transform.position.y > heightBounds.y)
        {
            Destroy(this.gameObject);
        }
    }
}
