using Assets.Scripts.Enums;

namespace Assets.Scripts
{
    internal sealed class LabParameters
    {
        private static LabParameters _instance;

        public delegate void ValueChanged(float value);

        public event ValueChanged EyeSeparationValueChanged;

        public event ValueChanged FieldOfViewValueChanged;

        public event ValueChanged NearClippingDistanceValueChanged;

        public event ValueChanged ConvergenceDistanceValueChanged;

        public static LabParameters GetInstance()
        {
            if(_instance == null)
                _instance = new LabParameters();
            return _instance;
        }

        public LabParameters()
        {
            
        }

        public void ChangeLabValue(float value, ValueType valueType)
        {
           switch(valueType) 
           {
                case ValueType.EyeSeparation:
                    EyeSeparationValueChanged?.Invoke(value);
                    break;
                case ValueType.FildOfView:
                    FieldOfViewValueChanged?.Invoke(value);
                    break;
                case ValueType.NearClipDistance:
                    NearClippingDistanceValueChanged?.Invoke(value);
                    break;
                case ValueType.ConvergenceDistance:
                    ConvergenceDistanceValueChanged?.Invoke(value);
                    break;
           }
        }
    }
}
