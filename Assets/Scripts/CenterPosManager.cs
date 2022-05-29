using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPosManager : MonoBehaviour
{
    public List<string> cards;
    public Sprite cardFace;
    public Sprite defaultFace;

    void Start()
    {
        
    }

    
    void Update()
    {
        cardRenderer();
    }

    void cardRenderer()
    {
        if(cards.Count == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = defaultFace;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = cardFace;
        }
    }

}
