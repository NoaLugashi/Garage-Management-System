using System;

namespace Ex03.GarageLogic
{
    public class Wheel
    {
        private string m_ManufacturerName;
        private float m_CurrentAirPressure;
        private float m_MaxAirPressure;

        public Wheel(string i_ManufacturerName, float i_CurrentAirPressure, float i_MaxAirPressure)
        {
            m_ManufacturerName = i_ManufacturerName;
            m_CurrentAirPressure = i_CurrentAirPressure;
            m_MaxAirPressure = i_MaxAirPressure;
        }

        public void Inflate(float i_AirToAdd)
        {
            if (m_CurrentAirPressure + i_AirToAdd > m_MaxAirPressure)
            {
                throw new ValueOutOfRangeException(0, m_MaxAirPressure - m_CurrentAirPressure);
            }

            m_CurrentAirPressure += i_AirToAdd;
        }

        public string ManufacturerName
        {
            get => m_ManufacturerName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Manufacturer name cannot be empty or null.");
                }
                m_ManufacturerName = value;
            }
        }

        public float CurrentAirPressure
        {
            get => m_CurrentAirPressure;
            set
            {
                if (value < 0 || value > MaxAirPressure)
                {
                    throw new ArgumentException($"Air pressure must be between 0 and {MaxAirPressure}.");
                }
                m_CurrentAirPressure = value;
            }
        }

        public float MaxAirPressure
        {
            get => m_MaxAirPressure;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Max air pressure must be greater than 0.");
                }
                m_MaxAirPressure = value;
            }
        }
    }
}