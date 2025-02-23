using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class GasCar : Vehicle
    {
        private GasSystem m_FuelTank;
        private CarProperties m_CarProperties;

        public GasCar(string i_LicenseNumber) : base(i_LicenseNumber, true)
        {
            m_FuelTank = new GasSystem();
            m_CarProperties = new CarProperties();
        }

        public override void Validate()
        {
            ValidateWheels(Wheels, 34);
            m_FuelTank.ValidateFuelTank(52f, eFuelType.Octan95);
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
                io_ErrorMessages.Add($"Error: A GasCar must have exactly 5 wheels. Entered: {wheels.Count}.");
            }

            foreach (var wheel in wheels)
            {
                if (wheel.MaxAirPressure > 34)
                {
                    io_ErrorMessages.Add($"Error: A GasCar's wheels must have a maximum air pressure of 34. Entered: {wheel.MaxAirPressure}.");
                    break;
                }
            }

            if (m_FuelTank.FuelType != eFuelType.Octan95)
            {
                io_ErrorMessages.Add($"Error: A GasCar must use fuel type Octan95. Entered: {m_FuelTank.FuelType}.");
            }

            if (m_FuelTank.MaxFuelAmount > 52f)
            {
                io_ErrorMessages.Add($"Error: A GasCar's fuel tank must have a maximum capacity of 52 liters. Entered: {m_FuelTank.MaxFuelAmount}.");
            }

            m_CarProperties.ValidateCarProperties(io_ErrorMessages);
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
                    if (!m_CarProperties.SetFieldValue(i_FieldName, i_Value))
                    {
                        base.SetFieldValue(i_FieldName, i_Value);
                    }
                    break;
            }
        }

        public override string GetVehicleDetails()
        {
            var details = new System.Text.StringBuilder();
            details.AppendLine("Gas Car Details:");
            details.AppendLine(base.GetVehicleDetails()); 
            m_FuelTank.GetDetails(details);
            details.AppendLine(m_CarProperties.GetCarDetails());
            return details.ToString();
        }
    }
}