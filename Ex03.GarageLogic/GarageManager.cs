using System;
using System.Collections.Generic;
using System.Linq;

namespace Ex03.GarageLogic
{
    public class GarageManager
    {
        private static readonly Dictionary<string, Vehicle> r_Vehicles = new Dictionary<string, Vehicle>();

        public bool IsVehicleInGarage(string i_LicenseNumber)
        {
            if (r_Vehicles.ContainsKey(i_LicenseNumber))
            {
                return true;
            }

            return false;
        }

        public void SetVehicleToInRepair(string i_LicenseNumber)
        {
            ChangeVehicleStatus(i_LicenseNumber, eGarageStatus.InRepair.ToString());
        }

        public void AddVehicleToGarage(string i_LicenseNumber, Vehicle i_Vehicle)
        {
            i_Vehicle.Validate();

            if (!r_Vehicles.ContainsKey(i_Vehicle.LicenseNumber))
            {
                r_Vehicles.Add(i_Vehicle.LicenseNumber, i_Vehicle);
            }
            else
            {
                throw new ArgumentException($"Vehicle with license number {i_Vehicle.LicenseNumber} already exists in the garage.");
            }
        }

        public IEnumerable<string> DisplayLicenseNumbers(eGarageStatus? i_FilterStatus = null)
        {
            return i_FilterStatus.HasValue
                ? r_Vehicles.Where(vehicle => vehicle.Value.VehicleStatus == i_FilterStatus.Value)
                            .Select(vehicle => vehicle.Key)
                : r_Vehicles.Keys;
        }

        public void ChangeVehicleStatus(string i_LicenseNumber, string i_NewStatus)
        {
            if (!r_Vehicles.ContainsKey(i_LicenseNumber))
            {
                throw new ArgumentException($"Vehicle with license number '{i_LicenseNumber}' does not exist in the garage.");
            }

            eGarageStatus newStatus;

            try
            {
                newStatus = (eGarageStatus)Enum.Parse(typeof(eGarageStatus), i_NewStatus, true);
            }
            catch
            {
                throw new ArgumentException($"Invalid status: '{i_NewStatus}'. Please choose a valid status.");
            }

            r_Vehicles[i_LicenseNumber].VehicleStatus = newStatus;
        }

        public Vehicle GetVehicleByLicenseNumber(string i_LicenseNumber)
        {
            if (!r_Vehicles.ContainsKey(i_LicenseNumber))
            {
                throw new ArgumentException($"Vehicle with license number {i_LicenseNumber} does not exist in the garage.");
            }

            return r_Vehicles[i_LicenseNumber];
        }

        public void InflateWheelsToMax(string i_LicenseNumber)
        {
            if (r_Vehicles.ContainsKey(i_LicenseNumber))
            {
                Vehicle vehicle = r_Vehicles[i_LicenseNumber];
                foreach (Wheel wheel in vehicle.Wheels)
                {
                    float airToAdd = wheel.MaxAirPressure - wheel.CurrentAirPressure;
                    wheel.Inflate(airToAdd);
                }
            }
            else
            {
                throw new ArgumentException($"Vehicle with license number {i_LicenseNumber} does not exist in the garage.");
            }
        }

        public void RefuelVehicle(string i_LicenseNumber, float i_Amount, string i_FuelType)
        {
            if (!r_Vehicles.ContainsKey(i_LicenseNumber))
            {
                throw new ArgumentException($"Vehicle with license number {i_LicenseNumber} does not exist in the garage.");
            }

            if (!Enum.TryParse(i_FuelType, true, out eFuelType fuelType))
            {
                throw new ArgumentException($"Invalid fuel type: {i_FuelType}.");
            }

            r_Vehicles[i_LicenseNumber].Refuel(i_Amount, fuelType);
        }

        public void RechargeVehicle(string i_LicenseNumber, float i_Minutes)
        {
            if (r_Vehicles.ContainsKey(i_LicenseNumber))
            {
                Vehicle vehicle = r_Vehicles[i_LicenseNumber];
                vehicle.Recharge(i_Minutes);
            }
            else
            {
                throw new ArgumentException($"Vehicle with license number {i_LicenseNumber} does not exist in the garage.");
            }
        }

        public List<string> GetVehicleStatuses()
        {
            return Enum.GetNames(typeof(eGarageStatus)).ToList();
        }

        public List<string> GetFuelTypes()
        {
            return Enum.GetNames(typeof(eFuelType)).ToList();
        }

        public List<string> GetVehicleTypes()
        {
            return Factory.GetVehicleTypeNames();
        }
    }
}