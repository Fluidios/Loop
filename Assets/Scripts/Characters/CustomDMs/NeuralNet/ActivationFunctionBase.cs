
using UnityEngine;

namespace Game.NeuralNet
{
    public abstract class ActivationFunctionBase
    {
        protected abstract float Run(float input);
        protected abstract float Derivative(float input);
        public ActivationFunctionDelegate Function
        {
            get { return Run; }
        }
        public ActivationFunctionDelegate DerivativeOfFunction
        {
            get { return Derivative; }
        }
    }
    public delegate float ActivationFunctionDelegate(float neuronOutputSignal);
}
