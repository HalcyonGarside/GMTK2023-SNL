using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardController : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private GameObject _ladderPrefab;

    [SerializeField] private Vector2 _positionOnScreen;

    [SerializeField] private Transform _cam;

    [SerializeField] private int _numLadders;



    private List<Snake> _snakes = new List<Snake>();
    private Vector2Int[] _ladders;
    private GameObject[] _ladderObjects;
    private Tile[] _tiles;

    // Start is called before the first frame update
    void Start()
    {
        _ladders = new Vector2Int[_numLadders];
        _ladderObjects = new GameObject[_numLadders];
        _tiles = new Tile[_width * _height];

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

                int progress = 0;

                if(y % 2 == 1)
                {
                    progress = y * _width + (_width - x - 1);
                }
                else
                {
                    progress = y * _width + x;
                }

                newTile.Init((float)progress / numTiles);

                Debug.Log(progress);
                _tiles[progress] = newTile;
            }
        }

        for(int i = 0; i < _numLadders; i++)
        {
            _ladderObjects[i] = Instantiate(_ladderPrefab, new Vector3(0, 0, -4), Quaternion.identity);
        }

        ScrambleLadders();

        _cam.transform.position = new Vector3((float)_width/2-0.5f, (float)_height/2-0.5f, -10) - new Vector3(_positionOnScreen.x, _positionOnScreen.y, 0.0f);
    }

    void ScrambleLadders()
    {
        for(int i = 0; i < _ladders.Length; i++)
        {
            var newLadder = new Vector2Int(Random.Range(0, _width * _height), Random.Range(0, _width * _height));
            var startTile = _tiles[Mathf.Min(newLadder.x, newLadder.y)];
            var endTile = _tiles[Mathf.Max(newLadder.x, newLadder.y)];

            Debug.Log(newLadder.x + ", " + newLadder.y);

            //Calculate ladder rotation and distance
            _ladderObjects[i].transform.position = (endTile.transform.position - startTile.transform.position) / 2.0f + startTile.transform.position - new Vector3(0, 0, 3);
            _ladderObjects[i].transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.up, endTile.transform.position - startTile.transform.position));
            //_ladderObjects[i].transform.localScale = new Vector3(1, Vector2.SqrMagnitude((endTile.transform.position - startTile.transform.position)) / _ladderObjects[i]., 1);

            var lsr = _ladderObjects[i].GetComponent<SpriteRenderer>();
            lsr.size = new Vector2(lsr.size.x, Mathf.Sqrt(Vector2.SqrMagnitude((endTile.transform.position - startTile.transform.position))));

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
