using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//The deck will be created

//The decks will be dealt and the remaining cards will be calculated.


public class DeskManager : MonoBehaviour
{
    public static DeskManager singleton;
    //DEFINITION OF LIST HOLDING THE DECK
    public GameObject cardFaceObject;
    public List<string> deck = new List<string>();
    public Sprite[] cardFace = new Sprite[52];
    public Sprite backCard;
    //WHAT YOU NEED TO KNOW TO BUILD THE DECK
    public static string[] type = new string[] { "C", "S", "H", "D" };
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    //TO KEEP TRACK OF THE BLOCKS WHERE THE CARDS ARE PLACED
    public GameObject[] BottomPosition = new GameObject[4];
    public GameObject[] TopPosition = new GameObject[4];
    public GameObject[] MiddlePosition = new GameObject[4]; //This place will be empty when cards are taken from the middle position. It will fill up as the card is put in.
    public GameObject Arena;//This place will be empty when cards are taken from the middle position. It will fill up as the card is put in.
    //THE ELEMENT THAT HOLDS THE TURN OF THE GAME
    public int PlayOrder = -1;//If 0,user plays,if 1, bot plays.
    //SCORE CALCULATION PART
    public List<string> BottomPosCollection = new List<string>();
    public List<string> TopPosCollection = new List<string>();
    public int BottomPosPisti;
    public int TopPosPisti;
    //NUMBER OF TURNS
    public int turnNumber = 0;
    //SCORE
    public int scorePlayer;
    public int scoreBot;
    //TEXT
    public Text scoreBoard;
    public Text scoreRobot;
    public Text scoreUs;
    //RESET COMMAND
    int endGame;
    public Button button;
    


    // Start is called before the first frame update
    void Start()
    {
        cardFaceObject = GameObject.FindGameObjectWithTag("Deck");
        for (int i = 0; i < 52; i++)
        {
            cardFace[i] = cardFaceObject.GetComponent<CardFace>().cardFrontFaces[i];
            Debug.Log(cardFace[i]);
        }
        backCard = cardFaceObject.GetComponent<CardFace>().cardBackFace;

        //To avoid bugs //Calculating points gives an error because BottomPosCollection and TopPosCollection give null refs
        BottomPosCollection.Add("Z0");//I'm giving a card name that won't affect the game
        TopPosCollection.Add("Z0");//I'm giving a card name that won't affect the game


        FirstStage();
        scoreBoard = GameObject.FindGameObjectWithTag("scoreboard").GetComponent<Text>();
        scoreRobot = GameObject.FindGameObjectWithTag("scoreRobot").GetComponent<Text>();
        scoreUs = GameObject.FindGameObjectWithTag("scorePlayer").GetComponent<Text>();
        button = GameObject.FindGameObjectWithTag("canvas").GetComponentInChildren<Button>();
        scoreBoard.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        if (singleton != this)
        {
            Destroy(gameObject);
        }

        Play();
        calculateScore();
    }

    public static DeskManager GetSingleton()
    {
        if (singleton == null)
        {
            if (GameObject.Find("deskManager") != null)
            {
                singleton = GameObject.Find("deskManager").GetComponent<DeskManager>();
            }
            else
            {
                GameObject Desk_Manager = new GameObject("deskManager");
                singleton = Desk_Manager.AddComponent<DeskManager>();
            }

        }
        return singleton;
    }

    void FirstStage()
    {
        CreateDeck();
        MixDeck();
        LocationObject();
        DistributeDeck();
    }

    void CreateDeck()
    {
        deck.Clear();
        foreach (string t in type)
        {
            foreach (string d in values)
            {
                deck.Add(t + d); // Collect strings like C2, C3, C4 in the list
            }
        }

    }

    void MixDeck()
    {
        System.Random random = new System.Random();
        int n = deck.Count;
        string temp;
        Sprite temp2;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            temp = deck[k];
            temp2 = cardFace[k];
            deck[k] = deck[n];
            cardFace[k] = cardFace[n];
            deck[n] = temp;
            cardFace[n] = temp2;
        }

       for(int i = 0; i < deck.Count; i++)
        {
            print(deck[i]);
        }
    }

    void FinishGame()
    {
        if (PlayOrder == 1 && DeskControl())
        {


            foreach (string item in Arena.GetComponent<CenterPosManager>().cards)
            {
                BottomPosCollection.Add(item);

            }
            Arena.GetComponent<CenterPosManager>().cards.Clear();

        }
        else if (PlayOrder == 0 && DeskControl())
        {

            foreach (string item in Arena.GetComponent<CenterPosManager>().cards)
            {
                TopPosCollection.Add(item);

            }
            Arena.GetComponent<CenterPosManager>().cards.Clear();

        }
        return;
    }

    void DistributeDeck()
    {
        if (PlayOrder == -1)
        {
            PlayOrder = Random.Range(0, 2);
            

        }
        
        if(turnNumber == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                BottomPosition[i].GetComponent<CardScript>().cardName = deck[0];
                if (BottomPosition[i].GetComponent<CardScript>().cardFaces.Count > 0)
                {
                    BottomPosition[i].GetComponent<CardScript>().cardFaces.RemoveAt(0);
                    BottomPosition[i].GetComponent<CardScript>().cardFaces.RemoveAt(0);
                }

                BottomPosition[i].GetComponent<CardScript>().cardFaces.Add(cardFace[turnNumber * 8 + i]);
                BottomPosition[i].GetComponent<CardScript>().cardFaces.Add(backCard);
                deck.RemoveAt(0);

            }

            for (int i = 0; i < 4; i++)
            {
                TopPosition[i].GetComponent<CardScript>().cardName = deck[0];
                if (TopPosition[i].GetComponent<CardScript>().cardFaces.Count > 0)
                {
                    TopPosition[i].GetComponent<CardScript>().cardFaces.RemoveAt(0);
                    TopPosition[i].GetComponent<CardScript>().cardFaces.RemoveAt(0);
                }
                TopPosition[i].GetComponent<CardScript>().cardFaces.Add(cardFace[turnNumber * 8 + i + 4]);
                TopPosition[i].GetComponent<CardScript>().cardFaces.Add(backCard);
                deck.RemoveAt(0);
            }

            for (int i = 0; i < 4; i++)
            {
                MiddlePosition[i].GetComponent<CardScript>().cardName = deck[0];
                if (MiddlePosition[i].GetComponent<CardScript>().cardFaces.Count > 0)
                {
                    MiddlePosition[i].GetComponent<CardScript>().cardFaces.RemoveAt(0);
                    MiddlePosition[i].GetComponent<CardScript>().cardFaces.RemoveAt(0);
                }
                MiddlePosition[i].GetComponent<CardScript>().cardFaces.Add(cardFace[turnNumber * 8 + i + 8]);
                MiddlePosition[i].GetComponent<CardScript>().cardFaces.Add(backCard);
                deck.RemoveAt(0);


            }
            Arena.GetComponent<CenterPosManager>().cardFace = MiddlePosition[3].GetComponent<CardScript>().cardFaces[0];
            Arena.GetComponent<CenterPosManager>().cards.Add(MiddlePosition[3].GetComponent<CardScript>().cardName);
        }


        else
        {
            for (int i = 0; i < 4; i++)
            {
                BottomPosition[i].GetComponent<CardScript>().cardName = deck[0];
                if (BottomPosition[i].GetComponent<CardScript>().cardFaces.Count > 0)
                {
                    BottomPosition[i].GetComponent<CardScript>().cardFaces.RemoveAt(0);
                    BottomPosition[i].GetComponent<CardScript>().cardFaces.RemoveAt(0);
                }

                BottomPosition[i].GetComponent<CardScript>().cardFaces.Add(cardFace[turnNumber * 8 + i + 4]);
                BottomPosition[i].GetComponent<CardScript>().cardFaces.Add(backCard);
                deck.RemoveAt(0);

            }

            for (int i = 0; i < 4; i++)
            {
                TopPosition[i].GetComponent<CardScript>().cardName = deck[0];
                if (TopPosition[i].GetComponent<CardScript>().cardFaces.Count > 0)
                {
                    TopPosition[i].GetComponent<CardScript>().cardFaces.RemoveAt(0);
                    TopPosition[i].GetComponent<CardScript>().cardFaces.RemoveAt(0);
                }
                TopPosition[i].GetComponent<CardScript>().cardFaces.Add(cardFace[turnNumber * 8 + i + 8]);
                TopPosition[i].GetComponent<CardScript>().cardFaces.Add(backCard);
                deck.RemoveAt(0);
            }
        }

        turnNumber++;
    }

   

    void LocationObject()
    {
        BottomPosition[0] = GameObject.Find("bottom0");
        BottomPosition[1] = GameObject.Find("bottom1");
        BottomPosition[2] = GameObject.Find("bottom2");
        BottomPosition[3] = GameObject.Find("bottom3");
        TopPosition[0] = GameObject.Find("top0");
        TopPosition[1] = GameObject.Find("top1");
        TopPosition[2] = GameObject.Find("top2");
        TopPosition[3] = GameObject.Find("top3");
        MiddlePosition[0] = GameObject.FindWithTag("mid0");
        MiddlePosition[1] = GameObject.FindWithTag("mid1");
        MiddlePosition[2] = GameObject.FindWithTag("mid2");
        MiddlePosition[3] = GameObject.FindWithTag("mid3");
        Arena = GameObject.FindWithTag("mid4");

    }

    bool DeskControl()
    {
        if (BottomPosition[0].GetComponent<CardScript>().cardName == "" && BottomPosition[1].GetComponent<CardScript>().cardName == ""
            && BottomPosition[2].GetComponent<CardScript>().cardName == "" && BottomPosition[3].GetComponent<CardScript>().cardName
            == "" && TopPosition[0].GetComponent<CardScript>().cardName == "" && TopPosition[1].GetComponent<CardScript>().cardName
            == "" && TopPosition[2].GetComponent<CardScript>().cardName == "" && TopPosition[3].GetComponent<CardScript>().cardName == "")
        {
            //PULLING CARD
            return true;
        }
        else
        {
            //YOU CAN'T PULL CARD
            return false;
        }
    }

    void calculateScore()
    {
        int temp_Bottom = 0;
        int temp_Top = 0;
        foreach (string item in BottomPosCollection)
        {
            if (item[1] == 'A' || item[1] == 'J')
            {
                temp_Bottom += 1;
            }
            else if (item == "C2")
            {
                temp_Bottom += 2;
            }
            else if (item == "D10")
            {
                temp_Bottom += 3;
            }
        }
        foreach (string item in TopPosCollection)
        {
            if (item[1] == 'A' || item[1] == 'J')
            {
                temp_Top += 1;
            }
            else if (item == "C2")
            {
                temp_Top += 2;
            }
            else if (item == "D10")
            {
                temp_Top += 3;
            }
        }
        temp_Bottom += (BottomPosPisti * 10);
        temp_Top += (TopPosPisti * 10);
        
        scoreRobot.text = "Robot: " + temp_Top;
        scoreUs.text = "You :" + temp_Bottom;
        if (BottomPosCollection.Count > TopPosCollection.Count)
        {
            temp_Bottom += 3;
        }
        else if (BottomPosCollection.Count < TopPosCollection.Count)
        {
            temp_Top += 3;
        }

        scoreBot = temp_Top;
        scorePlayer = temp_Bottom;
    }

    void GameFinishCalculate()
    {

        button.GetComponentInChildren<Text>().text = "Robot: " + scoreBot + " PLAYER: " + scorePlayer;

        scoreRobot.text = "Robot: " + scoreBot;
        scoreUs.text = "You :" + scorePlayer;

        if(scoreBot < scorePlayer)
        {
            scoreBoard.text = "YOU WIN";
        }
        else if(scoreBot < scorePlayer)
        {
            scoreBoard.text = "It's a draw.";
        }
        else
        {
            scoreBoard.text = "YOU LOST.";
        }


        button.transform.position = new Vector3(0, 2.2f, 0);
        //resetButton.transform.position = new Vector3(360, 236, 0);
    }

    void Play()
    {
        if (deck.Count == 0 && DeskControl() && Arena.GetComponent<CenterPosManager>().cards.Count == 0)
        {
            GameFinishCalculate();
        }
        if (deck.Count == 0 && DeskControl())
        {
            FinishGame();
        }
        else if (DeskControl())
        {
            DistributeDeck();
        }
        else
        {
            if (PlayOrder == 0)
            {
                PlayPlayer();

            }
            else if (PlayOrder == 1)
            {
                PlayBot();
            }
        }
    }

    void PlayPlayer()
    {
        GetMouseClick();
    }

    void PlayBot()
    {
        bool didItPlay = false;
        char middleCard;
        CenterPosManager centerscript = Arena.GetComponent<CenterPosManager>();
        if (centerscript.cards.Count == 0)
        {
            middleCard = 'x';
        }
        else
        {
            middleCard = centerscript.cards[centerscript.cards.Count - 1][1];
            print(centerscript.cards[centerscript.cards.Count - 1][1]);
        }

        if (TopPosition[0].GetComponent<CardScript>().cardName != "")
        {
            //Debug.Log("Kart ismi0 " + UstKonum[0].GetComponent<CardScript>().kartismi[1]);
            if (TopPosition[0].GetComponent<CardScript>().cardName[1] == middleCard)
            {
                //Debug.Log("AYNI ELINDE VAR");
                PlayCard(TopPosition[0]);
                didItPlay = true;
                return;
            }
        }
        if (TopPosition[1].GetComponent<CardScript>().cardName != "")
        {
            //Debug.Log("Kart ismi1 " + UstKonum[1].GetComponent<CardScript>().kartismi[1]);
            if (TopPosition[1].GetComponent<CardScript>().cardName[1] == middleCard)
            {
                //Debug.Log("AYNI ELINDE VAR");
                PlayCard(TopPosition[1]);
                didItPlay = true;
                return;
            }
        }
        if (TopPosition[2].GetComponent<CardScript>().cardName != "")
        {
            //Debug.Log("Kart ismi2 " + UstKonum[2].GetComponent<CardScript>().kartismi[1]);
            if (TopPosition[2].GetComponent<CardScript>().cardName[1] == middleCard)
            {
                //Debug.Log("AYNI ELINDE VAR");
                PlayCard(TopPosition[2]);
                didItPlay = true;
                return;
            }
        }
        if (TopPosition[3].GetComponent<CardScript>().cardName != "")
        {
            //Debug.Log("Kart ismi3 " + UstKonum[3].GetComponent<CardScript>().kartismi[1]);
            if (TopPosition[3].GetComponent<CardScript>().cardName[1] == middleCard)
            {
                //Debug.Log("AYNI ELINDE VAR");
                PlayCard(TopPosition[3]);
                didItPlay = true;
                return;
            }
        }

        if (TopPosition[0].GetComponent<CardScript>().cardName != "")
        {
            if (TopPosition[0].GetComponent<CardScript>().cardName[1] == 'J')
            {
                //Debug.Log("JOKER ELINDE VAR");
                PlayCard(TopPosition[0]);
                didItPlay = true;
                return;
            }
        }
        if (TopPosition[1].GetComponent<CardScript>().cardName != "")
        {
            if (TopPosition[1].GetComponent<CardScript>().cardName[1] == 'J')
            {
                //Debug.Log("JOKER ELINDE VAR");
                PlayCard(TopPosition[1]);
                didItPlay = true;
                return;
            }
        }
        if (TopPosition[2].GetComponent<CardScript>().cardName != "")
        {
            if (TopPosition[2].GetComponent<CardScript>().cardName[1] == 'J')
            {
                //Debug.Log("JOKER ELINDE VAR");
                PlayCard(TopPosition[2]);
                didItPlay = true;
                return;
            }
        }
        if (TopPosition[3].GetComponent<CardScript>().cardName != "")
        {
            if (TopPosition[3].GetComponent<CardScript>().cardName[1] == 'J')
            {
                //Debug.Log("JOKER ELINDE VAR");
                PlayCard(TopPosition[3]);
                didItPlay = true;
                return;
            }
        }

        if (!didItPlay)
        {
            while (true)
            {
                int randPosition = Random.Range(0, 4);
                if (TopPosition[randPosition].GetComponent<CardScript>().cardName == "")
                {
                    continue;
                }
                PlayCard(TopPosition[randPosition]);
                break;

            }
        }
        
    }

    void GetMouseClick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y - 10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                if (hit.collider.GetComponent<CardScript>() && hit.collider.CompareTag("bottom") && hit.collider.GetComponent<CardScript>().cardName != "")
                {
                    PlayCard(hit.collider.gameObject);

                }

            }
        }
    }

    void PlayCard(GameObject card)
    {
        if (canItTake(card))
        {
            if (PlayOrder == 0)
            {

                BottomPosCollection.Add(card.GetComponent<CardScript>().cardName);
                for (int i = 0; i < 4; i++)
                {
                    foreach (string item in MiddlePosition[i].GetComponent<CenterPosManager>().cards)
                    {
                        BottomPosCollection.Add(item);

                    }
                }

                for(int i = 0;i < 4; i++)
                {
                    MiddlePosition[i].GetComponent<CenterPosManager>().cards.Clear();
                    card.GetComponent<CardScript>().cardName = "";
                }

                foreach (string item in Arena.GetComponent<CenterPosManager>().cards)
                {
                    BottomPosCollection.Add(item);

                }

                Arena.GetComponent<CenterPosManager>().cards.Clear();



                card.GetComponent<CardScript>().cardName = "";


            }
            else if (PlayOrder == 1)
            {
                TopPosCollection.Add(card.GetComponent<CardScript>().cardName);

                for (int i = 0; i < 4; i++)
                {
                    foreach (string item in MiddlePosition[i].GetComponent<CenterPosManager>().cards)
                    {
                        TopPosCollection.Add(item);

                    }

                }



                for (int i = 0; i < 4; i++)
                {
                    MiddlePosition[i].GetComponent<CenterPosManager>().cards.Clear();
                    MiddlePosition[i].GetComponent<CardScript>().cardName = "";

                }

                foreach (string item in Arena.GetComponent<CenterPosManager>().cards)
                {
                    TopPosCollection.Add(item);

                }
                Arena.GetComponent<CenterPosManager>().cards.Clear();

                card.GetComponent<CardScript>().cardName = "";
            }
        }
        else
        {
            Arena.GetComponent<CenterPosManager>().cardFace = card.GetComponent<CardScript>().cardFaces[0];
            Arena.GetComponent<CenterPosManager>().cards.Add(card.GetComponent<CardScript>().cardName);
            card.GetComponent<CardScript>().cardName = "";
        }


        ChangeHand();
    }

    bool canItTake(GameObject card)
    {
        char middleCard;
        CenterPosManager centerscript = Arena.GetComponent<CenterPosManager>();
        if (centerscript.cards.Count == 0)
        {
            middleCard = 'x';
        }
        else
        {
            middleCard = centerscript.cards[centerscript.cards.Count - 1][1];

        }

        if (card.GetComponent<CardScript>().cardName[1] == middleCard)
        {
            if (centerscript.cards.Count == 1)
            {
                if (PlayOrder == 0)
                {
                    BottomPosPisti++;
                }
                else if (PlayOrder == 1)
                {
                    TopPosPisti++;
                }
            }
            return true;
        }
        else if (card.GetComponent<CardScript>().cardName[1] == 'J')
        {
            return true;
        }
        return false;
    }

    void ChangeHand()
    {
        if (PlayOrder == 1)
        {
            PlayOrder = 0;
        }
        else
        {
            PlayOrder = 1;
        }
    }

}
