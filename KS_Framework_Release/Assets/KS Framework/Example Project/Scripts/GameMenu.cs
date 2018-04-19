using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core;
using KS_Core.IO;
using System;

public class GameMenu : KS_Behaviour {

    bool visible = false;

    public GameObject menuContainer;

    protected override void Awake()
    {
        base.Awake();

        KS_SaveLoad.OnLoad += OnLoad;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (visible)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }

    private void Show()
    {
        Manager.SetGameState(KS_Manager.GameState.GameMenu);
        menuContainer.SetActive(true);
        visible = true;
        Cursor.visible = true;
    }

    private void Hide()
    {
        Manager.SetGameState(KS_Manager.GameState.Playing);
        menuContainer.SetActive(false);
        visible = false;
        Cursor.visible = false;
    }

    public void SaveGame()
    {
        KS_Manager.Instance.SaveGame("save");
        Hide();
    }

    public void LoadGame()
    {
        KS_SaveLoad.Load("save");
    }

    private void OnLoad(KS_SaveGame savegame)
    {
        Manager.SetGameState(KS_Manager.GameState.LoadScreen);

        clearlevel();

        for (int i = 0; i < savegame.gameObjects.Count; i++)
        {
            GameObject obj = KS_SaveLoad.RestoreGameObject(savegame.gameObjects[i]);

            if (obj)
                obj.SendMessage("OnLoad", savegame, SendMessageOptions.DontRequireReceiver);
        }

        Hide();
        Manager.SetGameState(KS_Manager.GameState.Playing);
    }

    void clearlevel()
    {
        KS_SaveableObject[] goa = GameObject.FindObjectsOfType<KS_SaveableObject>();

        foreach (KS_SaveableObject so in goa)
        {
            Destroy(so.gameObject);
        }
    }


    public void Continue()
    {
        Hide();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
