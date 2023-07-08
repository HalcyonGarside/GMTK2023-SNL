using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardController : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform _cam;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        int numTiles = _width * _height;

        for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                var newTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                newTile.name = $"Tile {x} {y}";

                float progress = 0;

                if(y % 2 == 1)
                {
                    progress = y * _width + (_width - x);
                }
                else
                {
                    progress = y * _width + x;
                }

                newTile.Init((float)progress / numTiles);
            }
        }

        _cam.transform.position = new Vector3((float)_width/2-0.5f, (float)_height/2-0.5f, -10);
    }
}
