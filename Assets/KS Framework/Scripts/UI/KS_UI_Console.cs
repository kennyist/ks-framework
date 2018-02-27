using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class KS_UI_Console : MonoBehaviour {

    public GameObject container;
    public Text logTextBox;
    public InputField inputText;
    public bool isEnabled = false;

	// Use this for initialization
	void Start () {
        KS_Console.Instance.OnLogUpdate += OnLogUpdate;
        inputText.onEndEdit.AddListener(delegate { OnEnter(inputText.text); });
    }

    private void OnLogUpdate(string text)
    {
        logTextBox.text = KS_Console.Instance.GetLog;

        if(logTextBox.text.Length > 3000)
        {
            logTextBox.text = logTextBox.text.Remove(logTextBox.text.LastIndexOf(Environment.NewLine));
        }
    }
    
    // Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (isEnabled)
            {
                container.SetActive(false);
                isEnabled = false;
            }
            else
            {
                container.SetActive(true);
                isEnabled = true;
                inputText.Select();
                inputText.ActivateInputField();
            }
        }

        if (isEnabled)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                inputText.text = KS_Console.Instance.PreviousCommand;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                inputText.text = KS_Console.Instance.NextUsedCommand;
            }
        }
    }

    public void OnEnter(string input)
    {
        Debug.Log("Entered: " + input);
        inputText.text = "";

        KS_Console.Instance.RunCommandString(input.ToLower());

        inputText.Select();
        inputText.ActivateInputField();
    }
}
