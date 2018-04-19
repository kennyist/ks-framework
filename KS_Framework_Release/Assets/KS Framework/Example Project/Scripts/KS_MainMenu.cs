using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core;

public class KS_MainMenu : KS_Behaviour {

    public GameObject menuContainer;

    protected override void OnGameStateChange(KS_Manager.GameState state)
    {
        Debug.Log("KS:FW - KS_MainMenu:OnGameStateChange() - change detected: " + state.ToString());

        if(state == KS_Manager.GameState.MainMenu)
        {
            menuContainer.SetActive(true);
        }

        base.OnGameStateChange(state);
    }
}
