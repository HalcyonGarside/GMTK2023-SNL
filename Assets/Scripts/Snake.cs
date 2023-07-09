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

    private GameBoardController _board;

    private Bounds _predictReg;
    private Bounds _scrambReg;

    public void Init(GameBoardController board, Bounds predictReg, Bounds scrambReg)
    {
        _board = board;
        _predictReg = predictReg;
        _scrambReg = scrambReg;
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

        float x = transform.position.x;
        float y = transform.position.y;

        Vector3 srMax = _scrambReg.max, srMin = _scrambReg.min;
        Vector3 pMax = _predictReg.max, pMin = _predictReg.min;

        if(x < srMax.x && x > srMin.x && y < srMax.y && y > srMin.y)
        {
            _board.ScrambleLadders(this);
        }
        else if(x < pMax.x && x > pMin.x && y < pMax.y && y > pMin.y)
        {
            _board.predictMove(this);
        }
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
