using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public int customerLimit = 5;
    public int vegLimit = 6;

    public GameObject[] vegPrefabs;
    public List<Customer> customers;
    public List<int> vegList;
    
    // Start is called before the first frame update
    void Start()
    {
        customers = new List<Customer>();
        vegList = new List<int>();
        for (int i = 0; i < vegLimit; i++)
        {
            vegList.Add(i);
        }

        for (int i = 0; i < customerLimit; i++)
        {
            Customer customer = new Customer();
            customer.orderList = new List<int>();
            customers.Add(customer);
        }


        InvokeRepeating("NewOrder", 1f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void NewOrder()
    {
        for (int i = 0; i < customerLimit; i++)
        {
            customers[i].orderList = GetRandomOrder();
        }


    }

    List<int> GetRandomOrder()
    {
        List<int> orderList = new List<int>();
        int i = Random.Range(2, 3);

        vegList = vegList.OrderBy(x => Random.value).ToList();

        for (int j = 0; j < i; j++)
        {
            orderList.Add(vegList[j]);
        }

        return orderList;

    }



}

public class Customer
{
    public List<int> orderList;

    
}
