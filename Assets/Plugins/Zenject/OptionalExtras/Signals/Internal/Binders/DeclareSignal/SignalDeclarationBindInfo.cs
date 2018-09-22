using System;
using ModestTree;

namespace Zenject
{
    public class SignalDeclarationBindInfo
    {
        public SignalDeclarationBindInfo(Type signalType)
        {
            SignalType = signalType;
        }

        public Type SignalType
        {
            get; private set;
        }

        public bool RunAsync
        {
            get; set;
        }

        public int TickPriority
        {
            get; set;
        }

        public SignalMissingHandlerResponses MissingHandlerResponse
        {
            get; set;
        }
    }
}
