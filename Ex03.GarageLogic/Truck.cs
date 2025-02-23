using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class Truck : Vehicle
    {
        private GasSystem m_FuelTank;
        private bool m_IsCoolingCargo;
        private float m_CargoVolume;

        public Truck(string i_LicenseNumber) : base(i_LicenseNumber, true)
        {
            m_FuelTank = new GasSystem();
        }

        public override void Validate()
        {
            ValidateWheels(Wheels, 29);
            m_FuelTank.ValidateFuelTank(125f, eFuelType.Soler);
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
            fields.Add("IsCoolingCargo", new FieldInfo("Is the truck carrying cooling cargo? (true/false):", typeof(bool)));
            fields.Add("CargoVolume", new FieldInfo("Enter cargo volume:", typeof(float)));

            return fields;
        }

        public override void ValidateSpecificRequirements(List<string> io_ErrorMessages)
        {
            List<Wheel> wheels = Wheels;

            if (wheels.Count != 14)
            {
                io_ErrorMessages.Add($"Error: A Truck must have exactly 14 wheels. Entered: {wheels.Count}.");
            }

            foreach (var wheel in wheels)
            {
                if (wheel.MaxAirPressure > 29)
                {
                    io_ErrorMessages.Add($"Error: A Truck's wheels must have a maximum air pressure of 29. Entered: {wheel.MaxAirPressure}.");
                    break;
                }
            }

            if (m_FuelTank.FuelType != eFuelType.Soler)
            {
                io_ErrorMessages.Add($"Error: A Truck must use fuel type Soler. Entered: {m_FuelTank.FuelType}.");
            }

            if (m_FuelTank.MaxFuelAmount > 125f)
            {
                io_ErrorMessages.Add($"Error: A Truck's fuel tank must have a maximum capacity of 125 liters. Entered: {m_FuelTank.MaxFuelAmount}.");
            }
        }

        public override void SetFieldValue(string i_FieldName, object i_Value)
        {
            switch (i_FieldName)
            {
                case "FuelMax":
                    m_FuelTank.MaxFuelAmount = (float)i_Value;
                    break;
                case "FuelAmount":
                    m_FuelTank.Refuel((float)i_Value, m_FuelTank.FuelType);
                    EnergyPercentage = (m_FuelTank.CurrentFuelAmount / m_FuelTank.MaxFuelAmount) * 100;
                    break;
                case "FuelType":
                    m_FuelTank.FuelType = (eFuelType)i_Value;
                    break;

                case "IsCoolingCargo":
                    if (i_Value is bool isCoolingCargo)
                    {
                        m_IsCoolingCargo = isCoolingCargo;
                    }
                    else
                    {
                        throw new ArgumentException("Invalid input for 'IsCoolingCargo'. Expected a Boolean value.");
                    }
                    break;
                case "CargoVolume":
                    m_CargoVolume = (float)i_Value;
                    break;
                default:
                    base.SetFieldValue(i_FieldName, i_Value);
                    break;
            }
        }

        public override string GetVehicleDetails()
        {
            var details = new System.Text.StringBuilder();
            details.AppendLine("Truck Details:");
            details.AppendLine(base.GetVehicleDetails());
            m_FuelTank.GetDetails(details);
            details.AppendLine($"Cargo Volume: {m_CargoVolume} m³");
            details.AppendLine($"Is Carrying Refrigerated Cargo: {m_IsCoolingCargo}");
            return details.ToString();
        }
    }
}