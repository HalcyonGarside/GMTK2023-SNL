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

    [SerializeField] private Player[] _players;



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

        for(int i = 0; i < _players.Length; i++)
        {
            _players[i].transform.position = new Vector3(_tiles[0].transform.position.x, _tiles[0].transform.position.y, _players[i].transform.position.z);
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

            Vector2 targetPosition = endTile.transform.position - _ladderObjects[i].transform.position;
            _ladderObjects[i].transform.rotation = Quaternion.FromToRotation(Vector3.up, targetPosition);
            //_ladderObjects[i].transform.localScale = new Vector3(1, Vector2.SqrMagnitude((endTile.transform.position - startTile.transform.position)) / _ladderObjects[i]., 1);

            var lsr = _ladderObjects[i].GetComponent<SpriteRenderer>();
            lsr.size = new Vector2(lsr.size.x, Mathf.Sqrt(Vector2.SqrMagnitude((endTile.transform.position - startTile.transform.position))));

            _ladders[i] = newLadder;
        }
    }

    public void AddSnake(Snake snek)
    {
        _snakes.Add(snek);
    }

    public void RemoveSnake(Snake snek)
    {
        _snakes.Remove(snek);
    }

    public void doRound()
    {
        for(int player = 0; player < _players.Length; player++)
        {
            int newPos = Mathf.Min(_players[player].GetBoardPosition() + Random.Range(1, 7), 99);
            int ladderEnd = -1;
            int snakeEnd = -1;
            
            for(int lad = 0; lad < _ladders.Length; lad++)
            {
                int ladderBase = Mathf.Min(_ladders[lad].x, _ladders[lad].y);
                int ladderTop = Mathf.Max(_ladders[lad].x, _ladders[lad].y);
                if(newPos == ladderBase && ladderTop > ladderEnd)
                {
                    ladderEnd = ladderTop;
                }
            }

            if(ladderEnd >= 0)
            {
                newPos = ladderEnd;
            }

            if(newPos >= 99)
            {
                _players[player].transform.position = new Vector3(_tiles[newPos].transform.position.x, _tiles[newPos].transform.position.y, _players[player].transform.position.z);
                _players[player].SetBoardPosition(newPos);
                Debug.Log("YYYYOU LOOOSEEEEE");
            }
            else
            {
                _players[player].transform.position = new Vector3(_tiles[newPos].transform.position.x, _tiles[newPos].transform.position.y, _players[player].transform.position.z);
                _players[player].SetBoardPosition(newPos);
            }
        }
    }
}
