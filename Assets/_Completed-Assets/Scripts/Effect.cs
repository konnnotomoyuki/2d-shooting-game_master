using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class Effect : MonoBehaviour
{
    Image image;

    private int intWaveLength;   

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
        if (FindObjectOfType<Emitter>().currentWave == 0)
        {
            this.image.color = new Color(20f, 0, 0, 20f);
        }
        else
        {
            this.image.color = Color.Lerp(this.image.color, Color.clear, Time.deltaTime);
        }
    }
}
