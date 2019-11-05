using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    public float score = 0;
    public float moveSpeed = 2f;
    public GameObject[] veggies;
    public GameObject[] vegHandlePos;
    public GameObject[] chopPositions;
    public GameObject platePrefab;
    public GameObject platePos;
    public enum PlayerID { player1 = 1, player2 = 2 };
    public PlayerID playerID;
    public Slider chopTimerSlider;
    public CustomerController customerController;
    public List<int> saladIngredients;
    public bool canServeSalad = false;
    public float gameTime = 200f;
    public float currentPlayTime;
    public TextMeshProUGUI timerText;


    private int pickedItems = 0;
    private bool pick = false;
    public bool drop = false;
    private bool playerCanMove = true;
    private bool canPickOrDrop = false;
    
    private List<int> vegList;
    private List<int> pickedList;
    private int chopPosition = 0;
    private int vegPlaced = 0;

    private Vector3 playerInitialPos;
    private Quaternion playerInitialRotation;
    private UIHandler uiHandler;
    private GameController gc;
    private IEnumerator playTimerInstance;
    

    void Start()
    {


        uiHandler = GameObject.Find("GameManager").GetComponent<UIHandler>();
        gc = GameObject.Find("GameManager").GetComponent<GameController>();

        playerInitialPos = this.transform.position;
        playerInitialRotation = this.transform.rotation;

        chopTimerSlider.gameObject.SetActive(false);
        saladIngredients = new List<int>();
        vegList = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            vegList.Add(i);
        }

        pickedList = new List<int>();
        
        Invoke("StartPlayTime", 1f);
    }

    void Update()
    {
        if (playerCanMove && canPickOrDrop)
        {
            if (playerID == PlayerID.player1)
            {
                if (Input.GetKeyUp(KeyCode.Q))
                {
                    pick = true;
                    drop = false;
                }

                else if (Input.GetKeyUp(KeyCode.E))
                {
                    drop = true;
                    pick = false;
                }
            }

            else if (playerID == PlayerID.player2)
            {
                if (Input.GetKeyUp(KeyCode.U))
                {
                    pick = true;
                    drop = false;
                }

                else if (Input.GetKeyUp(KeyCode.O))
                {
                    drop = true;
                    pick = false;
                }
            }

        }
        

    }


    /// <summary>
    /// Player Movement - Input and Implementation.
    /// </summary>
    void FixedUpdate()
    {
        if (playerCanMove)
        {
            if (playerID == PlayerID.player1)
            {
                transform.Translate(0f, 0f, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
                transform.Rotate(0f, Input.GetAxis("Horizontal") * moveSpeed, 0f);
            }

            else if (playerID == PlayerID.player2)
            {
                transform.Translate(0f, 0f, Input.GetAxis("Vertical2") * moveSpeed * Time.deltaTime);
                transform.Rotate(0f, Input.GetAxis("Horizontal2") * moveSpeed, 0f);
            }
            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        canPickOrDrop = false;
        pick = false;
        drop = false;
    }



    /// <summary>
    /// Player actions based on the three different trigger it activates. Different types of stocks of vegetables, chopboard, trashcan and the Customer.
    /// </summary>
    /// <param name="col"></param>

    private void OnTriggerStay(Collider col)
    {
        canPickOrDrop = true;
        int pickItemIndex = 0;

        if (col.gameObject.tag == "Veg0")
        {
            pickItemIndex = 0;
        }

        else if (col.gameObject.tag == "Veg1")
        {
            pickItemIndex = 1;
        }

        else if (col.gameObject.tag == "Veg2")
        {
            pickItemIndex = 2;
        }

        else if (col.gameObject.tag == "Veg3")
        {
            pickItemIndex = 3;
        }

        else if (col.gameObject.tag == "Veg4")
        {
            pickItemIndex = 4;
        }

        else if (col.gameObject.tag == "Veg5")
        {
            pickItemIndex = 5;
        }

        else if (this.playerID == PlayerID.player1 && col.gameObject.tag == "ChopBoard1")
        {
            if (drop)
            {
                PlaceVeggiesToChop();
            }

            else if (pick && !IsChopBoardEmpty() && !IsPlayerCarryingVeg())
            {
                pick = false;
                ServeTheSalad();
            }

        }

        else if (this.playerID == PlayerID.player2 && col.gameObject.tag == "ChopBoard2")
        {
            if (drop)
            {
                PlaceVeggiesToChop();
            }

            else if (pick && !IsChopBoardEmpty() && !IsPlayerCarryingVeg())
            {
                pick = false;
                ServeTheSalad();
            }
        }

       

        

        if (!col.tag.Contains("ChopBoard") && !col.tag.Contains("Trash"))
        {
            if (pick && pickedItems < 2)
            {
                pick = false;
                PickItem(pickItemIndex);
            }

            else if (drop)
            {
                drop = false;
                DropItem();
            }
        }
        

    }

    void PickItem(int itemIndex)
    {

        GameObject veg = Instantiate(veggies[itemIndex], vegHandlePos[pickedItems].transform, false);
        veg.transform.localPosition = new Vector3(0f, 0f, 0f);

        Debug.Log("Picking Item" + itemIndex);
        pickedItems++;
        pickedList.Add(itemIndex);
    }

    void DropItem()
    {
        Debug.Log("Picked Items:" + pickedItems);
        if (pickedItems <= 0)
        {
            return;
        }
        else if (pickedItems >= 2)
        {
            Destroy(vegHandlePos[1].transform.GetChild(0).gameObject);
        }

        else if (pickedItems == 1)
        {
            Destroy(vegHandlePos[0].transform.GetChild(0).gameObject);
        }

        pickedItems--;
        pickedList.Remove(pickedList.Count - 1);
        Debug.Log("Dropping Item");
    }

    void PlaceVeggiesToChop()
    {
        if (pickedItems <= 0)
        {
            return;
        }
        else if (pickedItems >= 2)
        {
            GameObject veg = vegHandlePos[0].transform.GetChild(0).gameObject;
            VegItemScript vegItem = veg.GetComponent<VegItemScript>();
            int vegID = vegItem.vegID;

            Destroy(vegHandlePos[0].transform.GetChild(0).gameObject);

            PlaceVeg(veggies[vegID], chopPosition, vegID);
        }

        else if (pickedItems == 1)
        {
            

            if (vegHandlePos[1].transform.childCount == 1)
            {
                GameObject veg = vegHandlePos[1].transform.GetChild(0).gameObject;
                VegItemScript vegItem = veg.GetComponent<VegItemScript>();
                int vegID = vegItem.vegID;

                Destroy(vegHandlePos[1].transform.GetChild(0).gameObject);
                PlaceVeg(veggies[vegID], chopPosition, vegID);
            }

            else
            {
                GameObject veg = vegHandlePos[0].transform.GetChild(0).gameObject;
                VegItemScript vegItem = veg.GetComponent<VegItemScript>();
                int vegID = vegItem.vegID;

                Destroy(vegHandlePos[0].transform.GetChild(0).gameObject);
                PlaceVeg(veggies[vegID], chopPosition, vegID);
            }
                

        }
        drop = false;
        pickedItems--;
    }

    void PlaceVeg(GameObject vegItem, int chopPos, int iD)
    {
        if (chopPosition < 3)
        {
            
            playerCanMove = false;
            GameObject go = Instantiate(vegItem, chopPositions[chopPos].transform);
            go.transform.localPosition = new Vector3(0f, 0f, 0f);
            saladIngredients.Add(iD);
            vegPlaced++;
            StartCoroutine(chopTimer(0.2f, 5f));
            
        }
        
    }


    IEnumerator chopTimer(float freq, float totalChopTime)
    {
        chopTimerSlider.gameObject.SetActive(true);
        float chopTime = 0f;
        chopTimerSlider.value = 0f;
        while ((chopTime+freq) < totalChopTime)
        {
            yield return new WaitForSecondsRealtime(freq);
            chopTime += freq;

            chopTimerSlider.value += freq/totalChopTime;
            Debug.Log("ChopTimer:" + chopTimerSlider.value);
        }


        chopTimerSlider.gameObject.SetActive(false);
        playerCanMove = true;
        chopPosition++;
        yield return null;
    }

    void ServeTheSalad()
    {
        canServeSalad = true;
        Debug.Log("Picking Salad");
        foreach (GameObject item in chopPositions)
        {
            if(item.transform.childCount == 1)
                Destroy(item.transform.GetChild(0).gameObject);
        }

        GameObject plate = Instantiate(platePrefab, platePos.transform, false);

        for (int i = 0; i < vegPlaced; i++)
        {

            Instantiate(veggies[saladIngredients[i]], plate.transform.GetChild(i), false);
        }
        
        
    }

    public void ResetOrder()
    {
        vegPlaced = 0;
        chopPosition = 0;
        pickedItems = 0;
        pickedList.Clear();
        saladIngredients.Clear();
    }

    public void ResetPlayer()
    {
        this.transform.position = playerInitialPos;
        this.transform.rotation = playerInitialRotation;
        score = 0;
        moveSpeed = 2f;
        canServeSalad = false;

        pickedItems = 0;
        pick = false;
        drop = false;
        playerCanMove = true;
        canPickOrDrop = false;

        vegList.Clear();
        pickedList.Clear();
        chopPosition = 0;
        vegPlaced = 0;

        score = 0f;
        uiHandler.UpdateScore(1, score.ToString());
        CleanUpAfterOrder();
    }

    public void CleanUpAfterOrder()
    {
        Debug.Log("Cleaning Up");
        foreach (GameObject item in chopPositions) //Clean chopboard
        {
            if (item.transform.childCount == 1)
                Destroy(item.transform.GetChild(0).gameObject);
        }

        foreach (GameObject item in vegHandlePos) //Clean Player Hands
        {
            if (item.transform.childCount == 1)
                Destroy(item.transform.GetChild(0).gameObject);
        }


    }

    bool IsChopBoardEmpty()
    {
        bool isEmpty = true;
        foreach (GameObject item in chopPositions) 
        {
            if (item.transform.childCount == 1)
                isEmpty = false;
        }

        


        return isEmpty;
    }

    bool IsPlayerCarryingVeg()
    {
        bool isCarrying = false;
        foreach (GameObject item in vegHandlePos)
        {
            if (item.transform.childCount == 1)
                isCarrying = true;
        }

        return isCarrying;
    }

    public void ClearPlate()
    {
        if (platePos.transform.childCount > 0)
            Destroy(platePos.transform.GetChild(0).gameObject);

    }

    public void AwardSpeed()
    {
        moveSpeed += 2f;
        Invoke("ResetSpeedAfterTime", 10f);
    }

    public void AwardTime()
    {
        currentPlayTime += 50f;
    }

    public void AwardScore()
    {
        score += 50f;
    }

    void StartPlayTime()
    {

        playTimerInstance = PlayTimer(0.2f, gameTime);
        StartCoroutine(playTimerInstance);

    }

    IEnumerator PlayTimer(float freq, float totalPlayTime)
    {
        currentPlayTime = totalPlayTime;
        while ((currentPlayTime - freq) > 0f)
        {
            yield return new WaitForSecondsRealtime(freq);
            currentPlayTime -= freq;
            int textInt = (int)currentPlayTime;
            timerText.text = textInt.ToString();
        }

        OnPlayTimerEnd();

        yield return null;
    }

    void OnPlayTimerEnd()
    {
        currentPlayTime = 0f;
        gc.OnGameEnd();
    }


    void ResetSpeedAfterTime()
    {
        moveSpeed -= 2f;
    }

}
