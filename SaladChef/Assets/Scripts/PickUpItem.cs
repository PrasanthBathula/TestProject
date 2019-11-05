using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public int playerID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.tag.Contains("Player"))
        {

            PlayerControl pc = other.gameObject.GetComponent<PlayerControl>();

            if (((int)pc.playerID == playerID ))
            {
                int c = Random.Range(0, 3);

                switch (c)
                {
                    case 0:
                        pc.AwardSpeed();
                        break;

                    case 1:
                        pc.AwardTime();
                        break;
                    case 2:
                        pc.AwardScore();
                        break;

                    default:
                        break;
                }

                Destroy(this.gameObject);

            }


        }

    }
}
