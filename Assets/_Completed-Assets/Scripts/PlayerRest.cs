using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRest : MonoBehaviour
{
    public GameObject Rest1;

    public GameObject Rest2;

    private int intPlayerNum;

    // Use this for initialization
    void Update ()
    {
        try
        {
            intPlayerNum = FindObjectOfType<Player>().intPlayerNum;

            if (intPlayerNum == 3)
            {
                Rest2.SetActive(true);
                Rest1.SetActive(true);
            }
            else if (intPlayerNum == 2)
            {
                Rest2.SetActive(false);
            }
            else if (intPlayerNum == 1)
            {
                Rest1.SetActive(false);
            }
        }
        catch
        {
            intPlayerNum = 3;
        }        
    }	
}
