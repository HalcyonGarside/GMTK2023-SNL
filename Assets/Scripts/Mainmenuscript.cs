using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenuscript : MonoBehaviour
{
   
   
   
   
   
   //hopefully loads the game
   private void LoadGame(){
        //Application.LoadLevel("SampleScene");
         SceneManager.LoadScene(1);
   }
   
   
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey){
            LoadGame();
        }
    }
}
