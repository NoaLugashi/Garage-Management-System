using System;

namespace Ex03.GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        private readonly float r_MinValue;
        private readonly float r_MaxValue;

        public ValueOutOfRangeException(float i_MinValue, float i_MaxValue)
            : base($"Value is out of range. It should be between {i_MinValue} and {i_MaxValue}.")
        {
            r_MinValue = i_MinValue;
            r_MaxValue = i_MaxValue;
        }

        public float GetMinValue()
        {
            return r_MinValue;
        }

        public float GetMaxValue()
        {
            return r_MaxValue;
        }
    }
}