using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinder : MonoBehaviour
{
    Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector2[] FindPath(Vector2 pathFrom, Vector2 pathTo)
    {
        // Pathfinding by https://github.com/SebLague/Pathfinding-2D
        return Pathfinding.RequestPath(pathFrom, pathTo);
    }

}
