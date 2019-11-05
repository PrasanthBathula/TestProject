using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashScript : MonoBehaviour
{
    private PlayerControl pc1, pc2;
    private UIHandler uiHandler;
    // Start is called before the first frame update
    void Start()
    {
        pc1 = GameObject.Find("Player1").GetComponent<PlayerControl>();
        pc2 = GameObject.Find("Player2").GetComponent<PlayerControl>();
        uiHandler = GameObject.Find("UIHandler").GetComponent<UIHandler>();
    }


    private void OnTriggerStay(Collider other)
    {
       
        if (other.tag.Contains("Player"))
        {
            
            PlayerControl pc = other.gameObject.GetComponent<PlayerControl>();

            if (pc.canServeSalad && pc.drop)
            {
                Debug.Log("Trash Trigger");
                pc.ClearPlate();
                pc.CleanUpAfterOrder();
                pc.ResetOrder();
                pc.score -= 20f;
                uiHandler.UpdateScore((int)pc.playerID, pc.score.ToString());
                
            }
            

        }

    }

    
}
