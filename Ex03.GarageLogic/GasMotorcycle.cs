using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class GasMotorcycle : Vehicle
    {
        private GasSystem m_FuelTank;
        private MotorcycleProperties m_MotorcycleProperties;

        public GasMotorcycle(string i_LicenseNumber) : base(i_LicenseNumber, true)
        {
            m_FuelTank = new GasSystem();
            m_MotorcycleProperties = new MotorcycleProperties();
        }

        public override void Validate()
        {
            ValidateWheels(Wheels, 32);
            m_FuelTank.ValidateFuelTank(6.2f, eFuelType.Octan98);
        }

        public override void Refuel(float i_Amount, eFuelType i_FuelType)
        {
            m_FuelTank.Refuel(i_Amount, i_FuelType);
            EnergyPercentage = (m_FuelTank.CurrentFuelAmount / m_FuelTank.MaxFuelAmount) * 100;
        }

        public override Dictionary<string, FieldInfo> GetRequiredFields()
        {
            var fields = base.GetRequiredFields();

            m_FuelTank.GetRequiredFields(fields);
            foreach (var motorcycleField in m_MotorcycleProperties.GetRequiredFields())
            {
                fields.Add(motorcycleField.Key, motorcycleField.Value);
            }

            return fields;
        }

        public override void ValidateSpecificRequirements(List<string> io_ErrorMessages)
        {
            List<Wheel> wheels = Wheels;

            if (wheels.Count != 2)
            {
                io_ErrorMessages.Add($"Error: A GasMotorCycle must have exactly 2 wheels. Entered: {wheels.Count}.");
            }

            foreach (var wheel in wheels)
            {
                if (wheel.MaxAirPressure > 32)
                {
                    io_ErrorMessages.Add($"Error: A GasMotorCycle's wheels must have a maximum air pressure of 32. Entered: {wheel.MaxAirPressure}.");
                    break;
                }
            }

            if (m_FuelTank.FuelType != eFuelType.Octan98)
            {
                io_ErrorMessages.Add($"Error: A GasMotorCycle must use fuel type Octan98. Entered: {m_FuelTank.FuelType}.");
            }
            if (m_FuelTank.MaxFuelAmount > 6.2f)
            {
                io_ErrorMessages.Add($"Error: A GasMotorCycle's fuel tank must have a maximum capacity of 6.2 liters. Entered: {m_FuelTank.MaxFuelAmount}.");
            }

            m_MotorcycleProperties.ValidateMotorcycleProperties(io_ErrorMessages);
        }

        public override void SetFieldValue(string i_FieldName, object i_Value)
        {
            switch (i_FieldName)
            {
                case "FuelMax":
                    m_FuelTank.MaxFuelAmount = (float)i_Value;
                    break;
                case "FuelAmount":
                    m_FuelTank.CurrentFuelAmount = (float)i_Value;
                    EnergyPercentage = (m_FuelTank.CurrentFuelAmount / m_FuelTank.MaxFuelAmount) * 100;
                    break;
                case "FuelType":
                    m_FuelTank.FuelType = (eFuelType)i_Value;
                    break;
                default:
                    if (!m_MotorcycleProperties.SetFieldValue(i_FieldName, i_Value))
                    {
                        base.SetFieldValue(i_FieldName, i_Value);
                    }
                    break;
            }
        }

        public override string GetVehicleDetails()
        {
            var details = new System.Text.StringBuilder();
            details.AppendLine("Gas Motorcycle Details:");
            details.AppendLine(base.GetVehicleDetails());
            m_FuelTank.GetDetails(details);
            details.AppendLine(m_MotorcycleProperties.GetMotorcycleDetails());
            return details.ToString();
        }
    }
}