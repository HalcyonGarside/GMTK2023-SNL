using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGenerator : MonoBehaviour
{
    [SerializeField] private Snake[] _snakeTypes;
    [SerializeField] private SpriteRenderer _renderer;

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
            retSnakes[snake].transform.Rotate(0, 0, 90);
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
        retSnake.transform.Rotate(0, 0, 90);

        return retSnake;
    }
}
