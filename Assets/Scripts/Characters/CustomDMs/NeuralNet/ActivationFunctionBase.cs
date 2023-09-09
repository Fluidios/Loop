
namespace Game.NeuralNet
{
    public abstract class ActivationFunctionBase
    {
        protected abstract float Run(float input);
        public ActivationFunctionDelegate Function
        {
            get { return Run; }
        }
    }
    public delegate float ActivationFunctionDelegate(float neuronOutputSignal);
}
