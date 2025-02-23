using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class ElectricSystem
    {
        private float m_CurrentEnergy;
        private float m_MaxEnergy;

        public ElectricSystem()
        {
            m_CurrentEnergy = 0;
            m_MaxEnergy = 0;
        }

        public float MaxEnergy
        {
            get => m_MaxEnergy;
            set => m_MaxEnergy = value;
        }

        public float CurrentEnergy
        {
            get => m_CurrentEnergy;
        }

        public void Recharge(float i_Amount)
        {
            if (m_CurrentEnergy + i_Amount > m_MaxEnergy)
            {
                throw new ValueOutOfRangeException(0, m_MaxEnergy - m_CurrentEnergy);
            }

            m_CurrentEnergy += i_Amount;
        }

        public void SetFieldValue(string i_FieldName, object i_Value)
        {
            switch (i_FieldName)
            {
                case "MaxBatteryTime":
                    MaxEnergy = (float)i_Value;
                    break;
                case "BatteryTime":
                    Recharge((float)i_Value - CurrentEnergy);
                    break;
                default:
                    throw new ArgumentException($"Field '{i_FieldName}' is not recognized in ElectricSystem.");
            }
        }

        public void GetRequiredFields(Dictionary<string, FieldInfo> io_Fields)
        {
            io_Fields.Add("MaxBatteryTime", new FieldInfo("Enter the maximum battery time:", typeof(float)));
            io_Fields.Add("BatteryTime", new FieldInfo("Enter current battery time:", typeof(float)));
        }

        public void GetDetails(System.Text.StringBuilder io_Details)
        {
            io_Details.AppendLine($"Battery Level: {m_CurrentEnergy}/{m_MaxEnergy}");
        }

        public void ValidateBattery(float i_MaxBatteryTime)
        {
            if (m_MaxEnergy > i_MaxBatteryTime)
            {
                throw new ValueOutOfRangeException(0, i_MaxBatteryTime);
            }
        }
    }
}