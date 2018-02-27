using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KS_LoadScreen : MonoBehaviour {

    public GameObject LoadScreenContainer;

    private bool loaded = false;
    public Text clickToContinue;
    
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
        LoadScreenContainer.gameObject.SetActive(false);
        clickToContinue.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(LoadScene(1));
        }

        if (loaded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                clickToContinue.gameObject.SetActive(false);
                LoadScreenContainer.SetActive(false);
                loaded = false;
            }
        }
	}

    private IEnumerator LoadScene(int scene)
    {
        LoadScreenContainer.SetActive(true);

        yield return new WaitForSeconds(3f);

        AsyncOperation async = SceneManager.LoadSceneAsync(scene);

        while (async.isDone)
        {
            yield return null;
        }

        loaded = true;
        clickToContinue.gameObject.SetActive(true);
    }
}
