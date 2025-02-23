using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class GasSystem
    {
        private float m_CurrentFuelAmount;
        private float m_MaxFuelAmount;
        private eFuelType m_FuelType;

        public GasSystem()
        {
            m_CurrentFuelAmount = 0;
            m_MaxFuelAmount = 0;
            m_FuelType = eFuelType.Octan95; //ברירת מחדל
        }

        public float MaxFuelAmount
        {
            get => m_MaxFuelAmount;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Maximum fuel amount must be greater than zero.");
                }
                m_MaxFuelAmount = value;
            }
        }

        public float CurrentFuelAmount
        {
            get => m_CurrentFuelAmount;
            set
            {
                if (value < 0 || value > m_MaxFuelAmount)
                {
                    throw new ArgumentException($"Fuel amount must be between 0 and {m_MaxFuelAmount}.");
                }
                m_CurrentFuelAmount = value;
            }
        }

        public eFuelType FuelType
        {
            get => m_FuelType;
            set
            {
                if (!Enum.IsDefined(typeof(eFuelType), value))
                {
                    throw new ArgumentException("Invalid fuel type.");
                }
                m_FuelType = value;
            }
        }

        public void GetRequiredFields(Dictionary<string, FieldInfo> io_Fields)
        {
            io_Fields.Add("FuelMax", new FieldInfo("Enter the maximum fuel amount:", typeof(float)));
            io_Fields.Add("FuelAmount", new FieldInfo("Enter current fuel amount:", typeof(float)));
            io_Fields.Add("FuelType", new FieldInfo("Enter fuel type (Octan95, Octan98, etc.):", typeof(eFuelType)));
        }

        public void GetDetails(System.Text.StringBuilder io_Details)
        {
            io_Details.AppendLine($"Fuel Level: {m_CurrentFuelAmount}/{m_MaxFuelAmount}");
            io_Details.AppendLine($"Fuel Type: {m_FuelType}");
        }

        public void Refuel(float i_Amount, eFuelType i_FuelType)
        {
            if (i_FuelType != m_FuelType)
            {
                throw new ArgumentException($"Incorrect fuel type. Expected: {m_FuelType}, got: {i_FuelType}.");
            }

            if (m_CurrentFuelAmount + i_Amount > m_MaxFuelAmount)
            {
                throw new ValueOutOfRangeException(0, m_MaxFuelAmount - m_CurrentFuelAmount);
            }

            m_CurrentFuelAmount += i_Amount;
        }

        public void ValidateFuelTank(float i_MaxCapacity, eFuelType i_ExpectedFuelType)
        {
            if (m_MaxFuelAmount > i_MaxCapacity)
            {
                throw new ValueOutOfRangeException(0, i_MaxCapacity);
            }

            if (m_FuelType != i_ExpectedFuelType)
            {
                throw new ArgumentException($"Invalid fuel type. Expected: {i_ExpectedFuelType}, got: {m_FuelType}.");
            }
        }
    }
}