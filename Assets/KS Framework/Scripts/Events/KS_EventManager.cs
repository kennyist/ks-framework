using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;


namespace KS_Events{
	public class KS_EventManager
	{
		
		static List<WeakReference> listeners = new List<WeakReference>();
		
		public static void registerListener(KS_EventListener evlst)
		{
			listeners.Add(new WeakReference(evlst));
		}
		
		public static void unregisterListener(KS_EventListener evlst){
			listeners.RemoveAll(x => x.Target == evlst);
		}
		
		public static void post(KS_Event ev)
		{
			// Remove all dead listners 
			var working = (from evlst in listeners where evlst.IsAlive select evlst);
			listeners = working.ToList();
			
			foreach(WeakReference wref in working)
			{
				// Call post event to the listener
				(wref.Target as KS_EventListener).eventReceived(ev);
			}
		}
	}
}
