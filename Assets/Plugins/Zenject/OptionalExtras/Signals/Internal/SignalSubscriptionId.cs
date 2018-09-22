using System;
using ModestTree;

namespace Zenject
{
    [System.Diagnostics.DebuggerStepThrough]
    public struct SignalSubscriptionId : IEquatable<SignalSubscriptionId>
    {
        Type _signalType;
        object _callback;

        public SignalSubscriptionId(Type signalType, object callback)
        {
            _signalType = signalType;
            _callback = callback;
        }

        public Type SignalType
        {
            get { return _signalType; }
        }

        public object Callback
        {
            get { return _callback; }
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + _signalType.GetHashCode();
                hash = hash * 29 + _callback.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object that)
        {
            if (that is SignalSubscriptionId)
            {
                return this.Equals((SignalSubscriptionId)that);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(SignalSubscriptionId that)
        {
            return object.Equals(this.SignalType, that.SignalType)
                && object.Equals(this.Callback, that.Callback);
        }

        public static bool operator == (SignalSubscriptionId left, SignalSubscriptionId right)
        {
            return left.Equals(right);
        }

        public static bool operator != (SignalSubscriptionId left, SignalSubscriptionId right)
        {
            return !left.Equals(right);
        }
    }
}
