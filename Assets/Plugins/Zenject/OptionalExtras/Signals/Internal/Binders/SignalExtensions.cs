using System;
using System.Collections.Generic;
using ModestTree;
using System.Linq;
using ModestTree.Util;

namespace Zenject
{
    public static class SignalExtensions
    {
        public static DeclareSignalRequireHandlerAsyncTickPriorityCopyBinder DeclareSignal<TSignal>(this DiContainer container)
        {
            var signalBindInfo = new SignalDeclarationBindInfo(typeof(TSignal));

            signalBindInfo.RunAsync = container.Settings.Signals.DefaultSyncMode == SignalDefaultSyncModes.Asynchronous;
            signalBindInfo.MissingHandlerResponse = container.Settings.Signals.MissingHandlerDefaultResponse;
            signalBindInfo.TickPriority = container.Settings.Signals.DefaultAsyncTickPriority;

            var bindInfo = container.Bind<SignalDeclaration>().AsCached()
                .WithArguments(typeof(TSignal), signalBindInfo).WhenInjectedInto(typeof(SignalBus), typeof(SignalDeclarationAsyncInitializer)).BindInfo;

            var signalBinder = new DeclareSignalRequireHandlerAsyncTickPriorityCopyBinder(signalBindInfo);
            signalBinder.AddCopyBindInfo(bindInfo);
            return signalBinder;
        }

        public static BindSignalToBinder<TSignal> BindSignal<TSignal>(this DiContainer container)
        {
            return new BindSignalToBinder<TSignal>(container);
        }
    }
}
