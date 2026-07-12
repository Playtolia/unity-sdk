using System;
using System.Collections.Generic;

namespace PlaytoliaSDK.Runtime.Common.Core
{
    public abstract class VirtualStateful
    {
        private List<Action> _stateChangeListeners = new List<Action>();
            
        public void NotifyStateChanged()
        {
            // Notify all registered state change listeners.
            foreach (var listener in _stateChangeListeners)
            {
                listener.Invoke();
            }
        }
        
        public void AddStateChangedListener(Action listener)
        {
            // Add a listener to the state change listeners list.
            if (listener != null && !_stateChangeListeners.Contains(listener))
            {
                _stateChangeListeners.Add(listener);
            }
        }
        
        public void RemoveStateChangedListener(Action listener)
        {
            // Remove a listener from the state change listeners list.
            if (listener != null && _stateChangeListeners.Contains(listener))
            {
                _stateChangeListeners.Remove(listener);
            }
        }
    }
}