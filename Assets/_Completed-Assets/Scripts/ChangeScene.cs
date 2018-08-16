using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeScene : MonoBehaviour {

	// Use this for initialization
	void Update ()
    {
        StartCoroutine(ChangeStage());
    }

    IEnumerator ChangeStage()
    {
        yield return new WaitForSeconds(5f);
        Object.DontDestroyOnLoad(FindObjectOfType<Score>());
        SceneManager.LoadScene("Secondstage");
    }
}
