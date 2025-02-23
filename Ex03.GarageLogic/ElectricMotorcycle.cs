using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class ElectricMotorcycle : Vehicle
    {
        private ElectricSystem m_Battery;
        private MotorcycleProperties m_MotorcycleProperties;

        public ElectricMotorcycle(string i_LicenseNumber) : base(i_LicenseNumber, false)
        {
            m_Battery = new ElectricSystem();
            m_MotorcycleProperties = new MotorcycleProperties();
        }

        public override void Validate()
        {
            ValidateWheels(Wheels, 32);
            m_Battery.ValidateBattery(2.9f);
        }

        public override void Recharge(float i_Minutes)
        {
            float hoursToCharge = i_Minutes / 60;
            m_Battery.Recharge(hoursToCharge);
            EnergyPercentage = (m_Battery.CurrentEnergy / m_Battery.MaxEnergy) * 100;
        }

        public override Dictionary<string, FieldInfo> GetRequiredFields()
        {
            var fields = base.GetRequiredFields();

            m_Battery.GetRequiredFields(fields);
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
                io_ErrorMessages.Add($"Error: An ElectricMotorCycle must have exactly 2 wheels. Entered: {wheels.Count}.");
            }

            foreach (var wheel in wheels)
            {
                if (wheel.MaxAirPressure > 32)
                {
                    io_ErrorMessages.Add($"Error: An ElectricMotorCycle's wheels must have a maximum air pressure of 32. Entered: {wheel.MaxAirPressure}.");
                    break;
                }
            }

            if (m_Battery.MaxEnergy > 2.9f)
            {
                io_ErrorMessages.Add($"Error: An ElectricMotorCycle's battery must have a maximum capacity of 2.9 hours. Entered: {m_Battery.MaxEnergy}.");
            }

            m_MotorcycleProperties.ValidateMotorcycleProperties(io_ErrorMessages);
        }

        public override void SetFieldValue(string i_FieldName, object i_Value)
        {
            switch (i_FieldName)
            {
                case "MaxBatteryTime":
                case "BatteryTime":
                    m_Battery.SetFieldValue(i_FieldName, i_Value);
                    EnergyPercentage = (m_Battery.CurrentEnergy / m_Battery.MaxEnergy) * 100;
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

            details.AppendLine("Electric Motorcycle Details:");
            details.AppendLine(base.GetVehicleDetails());
            m_Battery.GetDetails(details);
            details.AppendLine(m_MotorcycleProperties.GetMotorcycleDetails());
            return details.ToString();
        }
    }
}