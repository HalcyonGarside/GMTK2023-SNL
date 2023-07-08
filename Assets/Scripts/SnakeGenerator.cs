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
            retSnakes[snake] = Instantiate(_snakeTypes[Random.Range(0, _snakeTypes.Length)], 
                new Vector3(Random.Range(region.min.x, region.max.x), Random.Range(region.min.y, region.max.y), -4), 
                Quaternion.identity);
        }

        return new Snake[numSnakes];
    }
}
