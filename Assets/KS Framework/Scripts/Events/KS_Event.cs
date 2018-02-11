using UnityEngine;
using System.Collections;

namespace KS_Events
{
    public interface KS_Event
    {

    }

    public interface KS_EventListener
    {
        void eventReceived(KS_Event e);
    }
}