using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameBoardController : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private GameObject _ladderPrefab;

    [SerializeField] private Vector2 _positionOnScreen;

    [SerializeField] private Transform _cam;

    [SerializeField] private int _numLadders;

    [SerializeField] private Player[] _players;

    [SerializeField] private SnakeGenerator _snakeGenerator;



    private List<Snake> _snakes;
    private Vector2Int[] _ladders;
    private GameObject[] _ladderObjects;
    private Tile[] _tiles;
    public static int turnCounter;
    [SerializeField] private Text[] _playerPred;

    // Start is called before the first frame update
    void Start()
    {
        _ladders = new Vector2Int[_numLadders];
        _ladderObjects = new GameObject[_numLadders];
        _tiles = new Tile[_width * _height];
        _snakes = _snakeGenerator.generateSnakes(3);
        turnCounter = 0;
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
            _players[i].SetNextRoll(Random.Range(1, 7));
            _playerPred[i].text = "Roll: " + _players[i].GetNextRoll();
        }

        ScrambleLadders(null);

        _cam.transform.position = new Vector3((float)_width/2-0.5f, (float)_height/2-0.5f, -10) - new Vector3(_positionOnScreen.x, _positionOnScreen.y, 0.0f);
    }

    public void ScrambleLadders(Snake sacrifice)
    {
        if(sacrifice != null)
        {
            _snakes.Remove(sacrifice);
            Destroy(sacrifice.gameObject);
        }
        
        for(int i = 0; i < _ladders.Length; i++)
        {
            var newLadder = new Vector2Int(Random.Range(0, _width * _height), Random.Range(0, _width * _height));
            newLadder.x = Random.Range(0, (_width * _height) - _width);
            newLadder.y = Random.Range(newLadder.x + _width, _width * _height);
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

    public void predictMove(Snake sacrifice)
    {
        var preds = new List<Text>();
        for(int i = 0; i < _playerPred.Length; i++)
        {
            if(!_playerPred[i].gameObject.activeSelf)
            {
                preds.Add(_playerPred[i]);
            }
        }

        if(preds.Count == 0)
        {
            return;
        }
        if(sacrifice != null)
        {
            _snakes.Remove(sacrifice);
            Destroy(sacrifice.gameObject);
        }

        int playerIdx = Random.Range(0, preds.Count);

        Debug.Log("Roll" + _players[playerIdx].GetNextRoll());

        preds[playerIdx].gameObject.SetActive(true);
    }

    public void doRound()
    {
        
        turnCounter++;
        int bonusSnakes = 0;
        List<Snake> snakesToDestroy = new List<Snake>();
        //increments through the players
        for(int player = 0; player < _players.Length; player++)
        {
            int newPos = Mathf.Min(_players[player].GetBoardPosition() + _players[player].GetNextRoll(), 99);
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

            for(int snek = 0; snek < _snakes.Count; snek++)
            {
                Collider2D head = _snakes[snek].getHead();
                Collider2D tail = _snakes[snek].getTail();
                int xh = Mathf.RoundToInt(head.transform.position.x);
                int yh = Mathf.RoundToInt(head.transform.position.y);
                int xt = Mathf.RoundToInt(tail.transform.position.x);
                int yt = Mathf.RoundToInt(tail.transform.position.y);

                if(xh > 10 || xh < 0 || yh > 10 || yh < 0 || xt > 10 || xt < 0 || yt > 10 || yt < 0)
                {
                    continue;
                }
                snakesToDestroy.Add(_snakes[snek]);

                int headProg = -1;
                int tailProg = -1;

                if(yh % 2 == 1)
                {
                    headProg = yh * _width + (_width - xh - 1);
                }
                else
                {
                    headProg = yh * _width + xh;
                }

                if(yt % 2 == 1)
                {
                    tailProg = yt * _width + (_width - xt - 1);
                }
                else
                {
                    tailProg = yt * _width + xt;
                }

                if(newPos == headProg && tailProg > snakeEnd)
                {
                    snakeEnd = tailProg;
                }
            }

            if(ladderEnd >= 0 && snakeEnd < 0)
            {
                newPos = ladderEnd;
                bonusSnakes++;
            }    
            else if(snakeEnd >= 0)
            {
                newPos = snakeEnd;
                Debug.Log("Uh oh you ran into a snake");
            }

            if(newPos >= 99)
            {
                _players[player].transform.position = new Vector3(_tiles[newPos].transform.position.x, _tiles[newPos].transform.position.y, _players[player].transform.position.z);
                _players[player].SetBoardPosition(newPos);
                Debug.Log("YYYYOU LOOOSEEEEE");
                SceneManager.LoadScene(2);
            }
            else
            {
                _players[player].transform.position = new Vector3(_tiles[newPos].transform.position.x, _tiles[newPos].transform.position.y, _players[player].transform.position.z);
                _players[player].SetBoardPosition(newPos);
                _players[player].SetNextRoll(Random.Range(1, 7));

                _playerPred[player].gameObject.SetActive(false);
                _playerPred[player].text = "Roll: " + _players[player].GetNextRoll();
            }
        }

        Debug.Log("Score: " + turnCounter);

        //Regen snakes
        foreach(Snake snek in snakesToDestroy)
        {
            _snakes.Remove(snek);
            Destroy(snek.gameObject);
        }
       // mathf.Min
        
        _snakes.AddRange(_snakeGenerator.generateSnakes(Mathf.Min((5-_snakes.Count), 1+bonusSnakes)));
       Debug.Log("Snake Count " + _snakes.Count + "Snek to be kill " + snakesToDestroy.Count);
    }
}
