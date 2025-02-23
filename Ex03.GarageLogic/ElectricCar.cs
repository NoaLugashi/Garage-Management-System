using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class ElectricCar : Vehicle
    {
        private ElectricSystem m_Battery;
        private CarProperties m_CarProperties;

        public ElectricCar(string i_LicenseNumber) : base(i_LicenseNumber, false)
        {
            m_Battery = new ElectricSystem();
            m_CarProperties = new CarProperties();
        }

        public override void Validate()
        {
            ValidateWheels(Wheels, 34);
            m_Battery.ValidateBattery(5.4f);
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
            foreach (var carField in m_CarProperties.GetRequiredFields())
            {
                fields.Add(carField.Key, carField.Value);
            }

            return fields;
        }

        public override void ValidateSpecificRequirements(List<string> io_ErrorMessages)
        {
            List<Wheel> wheels = Wheels;

            if (wheels.Count != 5)
            {
                io_ErrorMessages.Add($"Error: An ElectricCar must have exactly 5 wheels. Entered: {wheels.Count}.");
            }

            foreach (var wheel in wheels)
            {
                if (wheel.MaxAirPressure > 34)
                {
                    io_ErrorMessages.Add($"Error: An ElectricCar's wheels must have a maximum air pressure of 34. Entered: {wheel.MaxAirPressure}.");
                    break;
                }
            }

            if (m_Battery.MaxEnergy > 5.4f)
            {
                io_ErrorMessages.Add($"Error: An ElectricCar's battery must have a maximum capacity of 5.4 hours. Entered: {m_Battery.MaxEnergy}.");
            }

            m_CarProperties.ValidateCarProperties(io_ErrorMessages);
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

                case "Color":
                case "NumberOfDoors":
                    if (!m_CarProperties.SetFieldValue(i_FieldName, i_Value))
                    {
                        throw new ArgumentException($"Field '{i_FieldName}' is not recognized in CarProperties.");
                    }
                    break;

                default:
                    base.SetFieldValue(i_FieldName, i_Value);
                    break;
            }
        }

        public override string GetVehicleDetails()
        {
            var details = new System.Text.StringBuilder();
            details.AppendLine("Electric Car Details:");
            details.AppendLine(base.GetVehicleDetails());
            m_Battery.GetDetails(details);
            details.AppendLine(m_CarProperties.GetCarDetails());
            return details.ToString();
        }
    }
}