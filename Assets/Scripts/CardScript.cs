using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public string cardName;
    public bool faceCard;
    public List<Sprite> cardFaces;
    public Sprite emptypos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cardRender();
    }

    void cardRender()
    {
        if (cardName == "")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = emptypos;
        }
        else
        {
            if (faceCard == true)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = cardFaces[0];
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = cardFaces[1];
            }
        }
    }
}
