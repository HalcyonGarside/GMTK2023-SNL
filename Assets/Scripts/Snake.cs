using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private GameObject[] _snakePrefabs;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Collider2D _headColl;
    [SerializeField] private Collider2D _tailColl;

    private bool dragging = false;
    
    private Vector3 offset;

    void Init()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging) 
        { 
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset; 
        }
        
    }

    private void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }

    private void OnMouseUp()
    {
        dragging = false;
        Debug.Log(_headColl.transform.position);
    }

    public Collider2D getHead()
    {
        return _headColl;
    }

    public Collider2D getTail()
    {
        return _tailColl;
    }
}
