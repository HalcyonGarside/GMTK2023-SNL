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

    public Snake[] generateSnakes(int numSnakes)
    {
        var retSnakes = new Snake[numSnakes];
        var region = _renderer.bounds;

        for(int snake = 0; snake < retSnakes.Length; snake++)
        {
            var snakeChoice = Random.Range(0, _snakeTypes.Length);
            retSnakes[snake] = Instantiate(_snakeTypes[snakeChoice], 
                new Vector3((region.min.x + region.max.x)/2, Random.Range(region.min.y, region.max.y), -9),
                _snakeTypes[snakeChoice].transform.rotation);
            retSnakes[snake].transform.Rotate(0, 0, 90);
        }

        return retSnakes;
    }
}
