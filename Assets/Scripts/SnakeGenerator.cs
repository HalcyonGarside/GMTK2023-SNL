using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGenerator : MonoBehaviour
{
    [SerializeField] private Snake[] _snakeTypes;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameBoardController _board;
    [SerializeField] private SpriteRenderer _scrambleRegion;
    [SerializeField] private SpriteRenderer _predictRegion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Snake> generateSnakes(int numSnakes)
    {
        var retSnakes = new List<Snake>();
        var region = _renderer.bounds;

        for(int snake = 0; snake < numSnakes; snake++)
        {
            var snakeChoice = Random.Range(0, _snakeTypes.Length);
            retSnakes.Add(Instantiate(_snakeTypes[snakeChoice], 
                new Vector3((region.min.x + region.max.x)/2, Random.Range(region.min.y, region.max.y), -9),
                _snakeTypes[snakeChoice].transform.rotation));
            retSnakes[snake].transform.Rotate(0, 0, Random.Range(90.0f, 270.0f));
            retSnakes[snake].Init(_board, _predictRegion.bounds, _scrambleRegion.bounds);
        }

        return retSnakes;
    }

    public Snake generateSnake()
    {
        var region = _renderer.bounds;
        var snakeChoice = Random.Range(0, _snakeTypes.Length);
        var retSnake = Instantiate(_snakeTypes[snakeChoice], 
            new Vector3((region.min.x + region.max.x)/2, Random.Range(region.min.y, region.max.y), -9),
            _snakeTypes[snakeChoice].transform.rotation);
        retSnake.transform.Rotate(0, 0, Random.Range(90.0f, 270.0f));
        retSnake.Init(_board, _predictRegion.bounds, _scrambleRegion.bounds);

        return retSnake;
    }
}
