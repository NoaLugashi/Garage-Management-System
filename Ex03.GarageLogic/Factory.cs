using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public static class Factory
    {
        private static readonly Dictionary<string, eVehicleType> s_VehicleTypeMap = new Dictionary<string, eVehicleType>
        {
            { "GasMotorcycle", eVehicleType.GasMotorcycle },
            { "ElectricMotorcycle", eVehicleType.ElectricMotorcycle },
            { "GasCar", eVehicleType.GasCar },
            { "ElectricCar", eVehicleType.ElectricCar },
            { "Truck", eVehicleType.Truck }
        };

        public static Vehicle CreateVehicle(string i_vehicleTypeName, string i_LicenseNumber)
        {
            switch (i_vehicleTypeName)
            {
                case "GasMotorcycle":
                    return new GasMotorcycle(i_LicenseNumber);
                case "ElectricMotorcycle":
                    return new ElectricMotorcycle(i_LicenseNumber);
                case "GasCar":
                    return new GasCar(i_LicenseNumber);
                case "ElectricCar":
                    return new ElectricCar(i_LicenseNumber);
                case "Truck":
                    return new Truck(i_LicenseNumber);
                default:
                    throw new ArgumentException($"Invalid vehicle type: {i_vehicleTypeName}");
            }
        }

        public static List<string> GetVehicleTypeNames()
        {
            return new List<string>(s_VehicleTypeMap.Keys);
        }
    }
}