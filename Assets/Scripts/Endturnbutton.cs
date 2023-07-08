using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endturnbutton : MonoBehaviour
{
    [SerializeField] private GameObject _EndTurnButton;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameBoardController _board;

    public void Init()
    {
        _highlight.SetActive(false);
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && _highlight.activeSelf == true)
        {
            _board.doRound();
        }
    }
}
