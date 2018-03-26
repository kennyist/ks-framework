using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using KS_Core;

public class KS_LoadScreen : KS_Behaviour {

    public GameObject LoadScreenContainer;

    private bool loaded = false;
    public Text clickToContinue;
    public Text progress;

    public override void OnLoadLevel(int index)
    {
        Debug.Log("Loading level: " + index);
        StartCoroutine(LoadScene(index));
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this.gameObject);
        LoadScreenContainer.gameObject.SetActive(false);
        clickToContinue.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        if (loaded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                clickToContinue.gameObject.SetActive(false);
                LoadScreenContainer.SetActive(false);
                loaded = false;

                KS_Manager.Instance.SetGameState(KS_Manager.GameState.Playing);
                KS_Manager.Instance.LevelLoaded();
            }
        }

        if (loading)
        {
            progress.text = loadProgress.ToString("00%");
        }
	}

    private float loadProgress = 0f;
    private bool loading = false;

    private IEnumerator LoadScene(int scene)
    {
        LoadScreenContainer.SetActive(true);
        loading = true;

        yield return new WaitForSeconds(3f);

        AsyncOperation async = SceneManager.LoadSceneAsync(scene);

        while (async.isDone)
        {
            loadProgress = async.progress;
            yield return null;
        }

        loading = false;
        loaded = true;
        clickToContinue.gameObject.SetActive(true);
    }
}
