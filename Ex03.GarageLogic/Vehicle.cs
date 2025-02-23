using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex03.GarageLogic
{
    public abstract class Vehicle
    {
        private string m_ModelName;
        private readonly string r_LicenseNumber;
        private float m_EnergyPercentage;
        private string m_OwnerName;
        private string m_PhoneNumber;
        private eGarageStatus m_VehicleStatus;
        private readonly bool r_IsGas;
        private List<Wheel> m_Wheels;
        private int m_NumberOfWheels;

        protected Vehicle(string i_LicenseNumber, bool i_IsGas)
        {
            if (string.IsNullOrWhiteSpace(i_LicenseNumber) || i_LicenseNumber.Length < 4 || !i_LicenseNumber.All(char.IsLetterOrDigit))
            {
                throw new ArgumentException("License number must be at least 4 characters long and contain only letters and digits.");
            }
            r_LicenseNumber = i_LicenseNumber;
            r_IsGas = i_IsGas;
            m_Wheels = new List<Wheel>();
        }

        public eGarageStatus VehicleStatus
        {
            get => m_VehicleStatus;
            set => m_VehicleStatus = value;
        }

        public float EnergyPercentage
        {
            get => m_EnergyPercentage;
            set => m_EnergyPercentage = value;
        }

        public string LicenseNumber => r_LicenseNumber;

        public bool IsGas => r_IsGas;

        public List<Wheel> Wheels => m_Wheels;

        public int NumberOfWheels
        {
            get => m_Wheels.Count;
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Number of wheels must be at least 1.");
                }

                float defaultMaxAirPressure = m_Wheels.Count > 0 ? m_Wheels[0].MaxAirPressure : 0f;

                m_NumberOfWheels = value;
                m_Wheels = createWheels(m_NumberOfWheels, defaultMaxAirPressure);
            }
        }

        public string PhoneNumber
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length != 10 || !value.All(char.IsDigit))
                {
                    throw new ArgumentException("Phone number must be exactly 10 digits.");
                }
                m_PhoneNumber = value;
            }
        }

        public abstract void ValidateSpecificRequirements(List<string> io_ErrorMessages);

        public abstract void Validate();

        public virtual string GetVehicleDetails()
        {
            var details = new System.Text.StringBuilder();
            details.AppendLine($"Owner: {m_OwnerName} ({m_PhoneNumber})");
            details.AppendLine($"Model Name: {m_ModelName}");
            details.AppendLine($"Vehicle Status: {m_VehicleStatus}");
            details.AppendLine($"Energy Percentage: {m_EnergyPercentage}%");
            details.AppendLine("Wheels:");
            foreach (var wheel in m_Wheels.Select((wheel, index) => new { Wheel = wheel, Index = index + 1 }))
            {
                details.AppendLine($"\tWheel {wheel.Index}: Manufacturer: {wheel.Wheel.ManufacturerName}, Current Pressure: {wheel.Wheel.CurrentAirPressure}, Max Pressure: {wheel.Wheel.MaxAirPressure}");
            }
            return details.ToString();
        }

        public void InflateWheel(int i_WheelIndex, float i_Pressure)
        {
            if (i_WheelIndex < 0 || i_WheelIndex >= m_Wheels.Count)
            {
                throw new ArgumentOutOfRangeException($"Invalid wheel index: {i_WheelIndex}. Must be between 0 and {m_Wheels.Count - 1}.");
            }

            m_Wheels[i_WheelIndex].Inflate(i_Pressure);
        }

        public void InflateAllWheels(float i_Pressure)
        {
            foreach (Wheel wheel in m_Wheels)
            {
                wheel.Inflate(i_Pressure);
            }
        }

        private List<Wheel> createWheels(int i_NumberOfWheels, float i_MaxPressure)
        {
            for (int index = 0; index < i_NumberOfWheels; index++)
            {
                m_Wheels.Add(new Wheel("Default Manufacturer", 0, i_MaxPressure));
            }

            return m_Wheels;
        }

        public virtual Dictionary<string, FieldInfo> GetRequiredFields()
        {
            return new Dictionary<string, FieldInfo>
            {
                { "OwnerName", new FieldInfo("Enter the owner's name:", typeof(string)) },
                { "PhoneNumber", new FieldInfo("Enter the owner's phone number (10 digits):", typeof(string)) },
                { "ModelName", new FieldInfo("Enter the model name:", typeof(string)) },
                { "NumberOfWheels", new FieldInfo("Enter the number of wheels:", typeof(int)) },
                { "MaxWheelPressure", new FieldInfo("Enter the maximum pressure for the wheels:", typeof(float)) },
                { "WheelPressure", new FieldInfo("Enter the desired pressure for the wheels:", typeof(float)) },
                { "WheelManufacturer", new FieldInfo("Enter the manufacturer name for the wheels:", typeof(string)) }
            };
        }

        public virtual void SetFieldValue(string i_FieldName, object i_Value)
        {
            if (i_FieldName == "ModelName")
            {
                m_ModelName = (string)i_Value;
            }
            else if (i_FieldName == "WheelPressure")
            {
                float pressure = (float)i_Value;
                foreach (var wheel in m_Wheels)
                {
                    wheel.Inflate(pressure - wheel.CurrentAirPressure);
                }
            }
            else if (i_FieldName == "NumberOfWheels")
            {
                m_NumberOfWheels = (int)i_Value;
                m_Wheels = createWheels(m_NumberOfWheels, m_Wheels[0].MaxAirPressure);
            }
            else if (i_FieldName == "MaxWheelPressure")
            {
                foreach (var wheel in m_Wheels)
                {
                    wheel.MaxAirPressure = (float)i_Value;
                }
            }
            else if(i_FieldName == "PhoneNumber")
            {
                PhoneNumber = (string)i_Value;
            }
            else if(i_FieldName == "OwnerName")
            {
                if (string.IsNullOrWhiteSpace((string)i_Value))
                {
                    throw new ArgumentException("Owner's name cannot be empty or null.");
                }
                m_OwnerName = (string)i_Value;
            }
            else
            {
                throw new ArgumentException($"Field '{i_FieldName}' is not recognized in the base class.");
            }
        }
        
        protected void ValidateWheels(List<Wheel> i_Wheels, float i_MaxPressure)
        {
            foreach (Wheel wheel in i_Wheels)
            {
                if (wheel.MaxAirPressure > i_MaxPressure)
                {
                    throw new ValueOutOfRangeException(0, i_MaxPressure);
                }

                if (wheel.MaxAirPressure < 0 || wheel.CurrentAirPressure > wheel.MaxAirPressure)
                {
                    throw new ValueOutOfRangeException(0, wheel.MaxAirPressure);
                }
            }
        }

        public virtual void Recharge(float i_Minutes)
        {
            throw new InvalidOperationException("This vehicle type does not support recharging.");
        }

        public virtual void Refuel(float i_Amount, eFuelType i_FuelType)
        {
            throw new InvalidOperationException("This vehicle type does not support refueling.");
        }
    }
}