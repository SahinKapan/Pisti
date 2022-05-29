using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReset : MonoBehaviour
{
    public GameObject game;
    public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunGame()
    {
        DeskManager.GetSingleton();
        button.transform.position = new Vector3(10000, 0, 0);
    }

}
