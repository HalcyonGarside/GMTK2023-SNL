using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGenerator : MonoBehaviour
{
    [SerializeField] private Snake[] _snakeTypes;

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
        // var retSnakes = new Snake[numSnakes];

        // for(int snake = 0; snake < retSnakes.Length; snake++)
        // {
        //     //retSnakes[snake] = Instantiate(_snakeTypes[Random.Range(0, _snakeTypes.Length)], this.gameObject.transform.)
        // }
        return new Snake[numSnakes];
    }
}
