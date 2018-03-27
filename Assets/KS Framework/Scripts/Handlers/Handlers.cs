using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.Handlers
{
    public delegate void GameStateHandler(KS_Manager.GameState state);
    public delegate void VoidHandler();
    public delegate void IntHandler(int index);
    public delegate void FloatHandler(float value);
    public delegate void BoolHandler(bool value);
}
