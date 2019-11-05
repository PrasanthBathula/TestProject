using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    
    public float gameTime = 600f;
    public GameObject endScreen;
    public TextMeshProUGUI summaryText;
    public GameObject pickUpPrefab;

    private IEnumerator timerInstance = null;
    private PlayerControl pc1, pc2;
    private GameObject[] custControllers;
    
    // Start is called before the first frame update
    void Start()
    {
        custControllers = GameObject.FindGameObjectsWithTag("Customer");

        pc1 = GameObject.Find("Player1").GetComponent<PlayerControl>();
        pc2 = GameObject.Find("Player2").GetComponent<PlayerControl>();

      
    }

   

   

    public void OnGameEnd()
    {
        if ((pc1.currentPlayTime == 0) && (pc2.currentPlayTime == 0))
        {
            ShowSummary();
            endScreen.SetActive(true);
        }
        
    }

    void ShowSummary()
    {
        if (pc1.score > pc2.score)
        {
            summaryText.text = "Player1 has Won";
        }

        else if (pc2.score > pc1.score)
        {
            summaryText.text = "Player2 has won";
        }

        else
        {
            summaryText.text = "No Contest";
        }

    }

    public void ResetLevel()
    {
        foreach (GameObject go in custControllers)
        {
            CustomerScript cc = go.GetComponent<CustomerScript>();
            cc.ResetCustomer();

        }
        pc1.ResetPlayer();
        pc2.ResetPlayer();
        endScreen.SetActive(false);
        Invoke("StartTimer", 1f);
    }

    public void AwardPickUp(int playerID)
    {
        float x = Random.Range(-7f, 7f);
        float y = 0.7f;
        float z = Random.Range(-3f, 3f);

        GameObject pickUp = Instantiate(pickUpPrefab);
        pickUp.transform.position = new Vector3(x, y, z);
        var pu = pickUp.GetComponent<PickUpItem>();
        pu.playerID = playerID;
        
        Debug.Log("$$$$$$$$$$$$$$$$$$$$$$$Pick UP for Player " + playerID);
    }


}
