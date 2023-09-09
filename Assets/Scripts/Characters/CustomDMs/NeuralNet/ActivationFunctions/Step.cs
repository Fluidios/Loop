using UnityEngine;

namespace Game.NeuralNet
{
    public class Step : ActivationFunctionBase
    {
        private const float c_floatingPointEpsilon = 0.00001f;
        private float _treshold;

        public Step(float treshold) 
        {
            _treshold = treshold;
        }

        protected override float Run(float input)
        {
            if(_treshold - input >  c_floatingPointEpsilon)
            {
                return 0;
            } 
            else
            {
                return 1;
            }
        }
    }
}
