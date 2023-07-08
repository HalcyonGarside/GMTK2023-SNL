using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardController : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Vector2 _positionOnScreen;

    [SerializeField] private Transform _cam;

    [SerializeField] private GameObject[] _ladderObjects;

    private List<Snake> _snakes = new List<Snake>();
    private Vector2Int[] _ladders;
    private List<Tile> _tiles = new List<Tile>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
        _ladders = new Vector2Int[_ladderObjects.Length];
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

                _tiles.Add(newTile);
            }
        }

        _cam.transform.position = new Vector3((float)_width/2-0.5f, (float)_height/2-0.5f, -10) - new Vector3(_positionOnScreen.x, _positionOnScreen.y, 0.0f);
    }

    void ScrambleLadders()
    {
        for(int i = 0; i < _ladders.Length; i++)
        {
            var newLadder = new Vector2Int(Random.Range(0, _width), Random.Range(0, _height));
            var startTile = _tiles[Mathf.Min(newLadder.x, newLadder.y)];
            var endTile = _tiles[Mathf.Max(newLadder.x, newLadder.y)];

            //Calculate ladder rotation and distance
            _ladderObjects[i].transform.position = (endTile.transform.position - startTile.transform.position) / 2.0f + startTile.transform.position;
            _ladderObjects[i].transform.rotation = Quaternion.FromToRotation(Vector3.up, endTile.transform.position - startTile.transform.position);
            _ladderObjects[i].transform.localScale = new Vector3(1, Vector2.SqrMagnitude((endTile.transform.position - startTile.transform.position)), 1);

            _ladders[i] = newLadder;
        }
    }

    void AddSnake(Snake snek)
    {
        _snakes.Add(snek);
    }

    void RemoveSnake(Snake snek)
    {
        _snakes.Remove(snek);
    }
}
