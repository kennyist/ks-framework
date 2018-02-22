using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poolItemTest : KS_poolObject {

    public PoolObjectSettings settings { get; set; }

    public override PoolObjectSettings PoolSettings()
    {
        return settings;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddToPoolAndDissable();
        }
    }
}
