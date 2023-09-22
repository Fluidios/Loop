using NaughtyAttributes;
using UnityEngine;

namespace Game.NeuralNet
{
    [System.Serializable]
    public class Step : ActivationFunctionBase
    {
        [SerializeField] private string _name;
        private const float c_floatingPointEpsilon = 0.00001f;
        private float _treshold;
        private float _delta;

        public Step(float treshold) 
        {
            _name = string.Format("Step({0})", treshold);
            _treshold = treshold;
        }

        protected override float Run(float input)
        {
            _delta = _treshold - input;
            if(_delta >  c_floatingPointEpsilon)
            {
                return 0;
            } 
            else
            {
                return 1;
            }
        }

        protected override float Derivative(float input)
        {
            throw new System.NotImplementedException();
        }
        public override string ToString()
        {
            return _name;
        }
    }
}
