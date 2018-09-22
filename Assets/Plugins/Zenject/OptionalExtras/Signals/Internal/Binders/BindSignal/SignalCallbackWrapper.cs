using System;
using ModestTree;

namespace Zenject
{
    // Note that there's a reason we don't just have a generic
    // argument for signal type - because when using struct type signals it throws
    // exceptions on AOT platforms
    public class SignalCallbackWrapper : IDisposable
    {
        readonly SignalBus _signalBus;
        readonly Action<object> _action;
        readonly Type _signalType;

        public SignalCallbackWrapper(
            Type signalType,
            Action<object> action,
            SignalBus signalBus)
        {
            _signalType = signalType;
            _signalBus = signalBus;
            _action = action;

            signalBus.Subscribe(signalType, OnSignalFired);
        }

        void OnSignalFired(object signal)
        {
            _action(signal);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe(_signalType, OnSignalFired);
        }
    }
}
