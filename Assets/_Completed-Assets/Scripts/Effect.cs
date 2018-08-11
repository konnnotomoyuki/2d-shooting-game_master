using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class Effect : MonoBehaviour
{
    Image image;

    bool blnColorChange = true;

    private int intWaveLength;

    private float timeout = 60f;

    private float timeelapsed = 0;

    // Use this for initialization
    void Start ()
    {
        image = GetComponent<Image>();
        image.color = Color.clear;

        intWaveLength = (FindObjectOfType<Emitter>().waves.Length - 1);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //StartCoroutine(ColorChange(3f));

        if (blnColorChange == true)
        {
            this.image.color = new Color(0.5f, 0f, 0f, 0.5f);
            blnColorChange = false;
        }
        else
        {
            this.image.color = Color.Lerp(this.image.color, Color.clear, Time.deltaTime); ;
            timeelapsed += 1f;

            if (timeelapsed > timeout)
            {
                blnColorChange = true;
                timeelapsed = 0;
            }
        }
    }
    
}
