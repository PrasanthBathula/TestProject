using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerScript : MonoBehaviour
{
    public List<int> orderList;
    public GameObject[] orderStatusPos;
    public Material[] statusMaterials;
    public Renderer moodRenderer;
    public TextMeshProUGUI timerText;
    public int timePerItem = 50;

    private List<int> vegList;
    private CustomerController custController;
    private bool orderTimeUp = false;
    private PlayerControl pc1, pc2;
    private GameController gc;
    private UIHandler uiHandler;
    private IEnumerator timerInstance = null;
    private float currentOrderTime;
    private float orderPeriod;



    // Start is called before the first frame update
    void Start()
    {
        custController = GameObject.Find("GameManager").GetComponent<CustomerController>();
        gc = GameObject.Find("GameManager").GetComponent<GameController>();
        pc1 = GameObject.Find("Player1").GetComponent<PlayerControl>();
        pc2 = GameObject.Find("Player2").GetComponent<PlayerControl>();
        uiHandler = GameObject.Find("GameManager").GetComponent<UIHandler>();
        orderList = new List<int>();
        vegList = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            vegList.Add(i);
        }
        Invoke("NewOrder", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void NewOrder()
    {
        ClearLastOrder();
        StartNewOrder();
    }

    void StartNewOrder()
    {
        orderTimeUp = false;
        orderList = GetRandomOrder();
        ResetOrderTimer(orderList.Count);
        Debug.Log("Orders: " + orderList[0] + orderList[1]);
        ShowOrderStatus();

    }

    void ShowOrderStatus()
    {
        int i = 0;
        foreach (int order in orderList)
        {
            GameObject veg = Instantiate(custController.vegPrefabs[order], orderStatusPos[i].transform, false);
            i++;
        }
    }

    List<int> GetRandomOrder()
    {
        List<int> orders = new List<int>();
        vegList = vegList.OrderBy(x => Random.value).ToList();

        int i = Random.Range(2, 4);

        Debug.Log("Veg List:" + vegList[0] + vegList[1] + vegList[2] + vegList[3] + vegList[4] + vegList[5]);
        Debug.Log("Random value:" + i);
        for (int j = 0; j < i; j++)
        {
            orders.Add(vegList[j]);
        }
        

        return orders;

    }

    void ClearLastOrder()
    {

        foreach (GameObject order in orderStatusPos)
        {
            if (order.transform.childCount == 1)
            {
                Destroy(order.transform.GetChild(0).gameObject);
            }
                
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player") && !orderTimeUp)
        {
            PlayerControl pc = other.gameObject.GetComponent<PlayerControl>();

            if (pc.canServeSalad)
            {
                StopCoroutine(OrderTimer(0.2f, 5));
                pc.canServeSalad = false;
                if(pc.platePos.transform.childCount > 0)
                    Destroy(pc.platePos.transform.GetChild(0).gameObject);
                StartCoroutine(CheckTheOrder(pc.saladIngredients, pc.playerID));
                ClearLastOrder();
                pc.ResetOrder();
                pc.CleanUpAfterOrder();
            }

            
            
            
        }
        
    }

    IEnumerator CheckTheOrder(List<int> saladIngredients, PlayerControl.PlayerID id)
    {

        if (orderList.Count == saladIngredients.Count && orderList.All(saladIngredients.Contains))
        {
            moodRenderer.material = statusMaterials[0];

            bool goldenFactor = (currentOrderTime / orderPeriod) > 0.7f;
            

            if (id == PlayerControl.PlayerID.player1)
            {

                if (goldenFactor)
                {
                    gc.AwardPickUp(1);
                }

                IncreaseScore(1, 100);
                
            }

            else if (id == PlayerControl.PlayerID.player2)
            {

                if (goldenFactor)
                {
                    gc.AwardPickUp(2);
                }

                IncreaseScore(2, 100);
            }

            

        }

        else
            moodRenderer.material = statusMaterials[1];

        StopCoroutine(timerInstance);

        yield return new WaitForSecondsRealtime(5f);

        moodRenderer.material = statusMaterials[2];
        
        NewOrder();

    }

    IEnumerator OrderTimer(float freq, float totalOrderTime)
    {
        currentOrderTime = totalOrderTime;
        while ((currentOrderTime - freq) > 0f)
        {
            yield return new WaitForSecondsRealtime(freq);
            currentOrderTime -= freq;
            int textInt = (int)currentOrderTime;
            timerText.text = textInt.ToString();
        }

        StartCoroutine(OnOrderTimerEnd());

        
        yield return null;
    }

    

    void ResetOrderTimer(int orderCount)
    {
        if (timerInstance != null)
            StopCoroutine(timerInstance);
        orderPeriod = orderCount * timePerItem;
        timerInstance = OrderTimer(0.2f, orderPeriod);
        StartCoroutine(timerInstance);
    }

    void TakeAway()
    {
        
    }

    IEnumerator OnOrderTimerEnd()
    {
        orderTimeUp = true;
        ReduceScores();
        ClearPlates();
        moodRenderer.material = statusMaterials[1];

        yield return new WaitForSecondsRealtime(3f);

        moodRenderer.material = statusMaterials[2];
        NewOrder();
        yield return null;
    }

    void ReduceScores()
    {
        pc1.score -= 20f;
        uiHandler.UpdateScore(1, pc1.score.ToString());
        pc2.score -= 20f;
        uiHandler.UpdateScore(2, pc2.score.ToString());
    }

    void ResetScores()
    {
        pc1.score = 0f;
        uiHandler.UpdateScore(1, pc1.score.ToString());
        pc2.score = 0f;
        uiHandler.UpdateScore(2, pc2.score.ToString());
    }

    void IncreaseScore(int player, int score)
    {
        if (player == 1)
        {
            pc1.score += score;
            uiHandler.UpdateScore(1, pc1.score.ToString());
        }

        else if (player == 2)
        {
            pc2.score += score;
            uiHandler.UpdateScore(2, pc2.score.ToString());
        }
    }

    void ClearPlates()
    {
        if (pc1.platePos.transform.childCount > 0)
            Destroy(pc1.platePos.transform.GetChild(0).gameObject);
        if (pc2.platePos.transform.childCount > 0)
            Destroy(pc2.platePos.transform.GetChild(0).gameObject);
    }

    /// <summary>
    /// Instantiate pick up with in the walkable area with player ID in it's script. When the pick up is triggered with the corresponding player, match the ID and award that player with Speed, Time and Score
    /// </summary>
    /// <param name="playerID"></param>

    

    public void ResetCustomer()
    {
        ClearPlates();
        orderList.Clear();
        NewOrder();
        ResetScores();
    }
}
