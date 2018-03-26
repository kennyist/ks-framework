using UnityEngine;
using System.Collections;

namespace KS_Core.Events
{
    public interface KS_Event
    {

    }

    public interface KS_EventListener
    {
        void eventReceived(KS_Event e);
    }
}