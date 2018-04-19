using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.Handlers
{
    /// <summary>
    /// On game state change delegate 
    /// </summary>
    /// <param name="state">Current Game State</param>
    public delegate void GameStateHandler(KS_Manager.GameState state);
    /// <summary>
    /// Generic handler 
    /// </summary>
    public delegate void VoidHandler();
    /// <summary>
    /// Generic Int parameter handler
    /// </summary>
    /// <param name="value">Int Value</param>
    public delegate void IntHandler(int value);
    /// <summary>
    /// Generic Float Handler
    /// </summary>
    /// <param name="value">Float value</param>
    public delegate void FloatHandler(float value);
    /// <summary>
    /// Generic Bool Handler
    /// </summary>
    /// <param name="value">Bool value</param>
    public delegate void BoolHandler(bool value);
}
