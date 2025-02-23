using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
    class ConsoleUI
    {
        private static readonly GarageManager r_GarageManager = new GarageManager();

        public void Run()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Garage Management System:");
                Console.WriteLine("1. Add new vehicle");
                Console.WriteLine("2. Display license numbers (all or filtered)");
                Console.WriteLine("3. Change vehicle status");
                Console.WriteLine("4. Inflate wheels to max");
                Console.WriteLine("5. Refuel vehicle");
                Console.WriteLine("6. Recharge vehicle");
                Console.WriteLine("7. Display full vehicle details");
                Console.WriteLine("8. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();
                try
                {
                    switch (choice)
                    {
                        case "1":
                            addNewVehicleUI();
                            break;
                        case "2":
                            displayLicenseNumbersUI();
                            break;
                        case "3":
                            changeVehicleStatusUI();
                            break;
                        case "4":
                            inflateAllWheelsToMax();
                            break;
                        case "5":
                            refuelVehicleUI();
                            break;
                        case "6":
                            rechargeVehicleUI();
                            break;
                        case "7":
                            displayFullVehicleDetailsUI();
                            break;
                        case "8":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
            }
        }

        private void addNewVehicleUI()
        {
            Console.Write("Enter license number: (at least 4 characters long and contain only letters and digits)");
            string licenseNumber = Console.ReadLine();

            if (r_GarageManager.IsVehicleInGarage(licenseNumber))
            {
                r_GarageManager.SetVehicleToInRepair(licenseNumber);
                Console.WriteLine("The vehicle is already in the garage. Status updated to 'InRepair'.");
                return;
            }
            Console.WriteLine("Please select the vehicle type:");
            List<string> vehicleTypes = r_GarageManager.GetVehicleTypes();

            for (int i = 0; i < vehicleTypes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {vehicleTypes[i]}");
            }
            int vehicleChoice = getUserChoice(1, vehicleTypes.Count);
            string selectedVehicleTypeString = vehicleTypes[vehicleChoice - 1];

            Vehicle newVehicle = Factory.CreateVehicle(selectedVehicleTypeString, licenseNumber);
            collectVehicleDetails(newVehicle);
            r_GarageManager.AddVehicleToGarage(licenseNumber, newVehicle);

            Console.WriteLine("Vehicle added successfully.");
        }

        private void collectVehicleDetails(Vehicle i_Vehicle)
        {
            var requiredFields = i_Vehicle.GetRequiredFields();
            Dictionary<string, object> fieldValues = new Dictionary<string, object>();
            var errorMessages = new List<string>();

            foreach (var field in requiredFields)
            {
                if (field.Key == "WheelPressure")
                {
                    handleWheelAttributes(i_Vehicle, "WheelPressure");
                }
                else if (field.Key == "NumberOfWheels")
                {
                    handleNumberOfWheels(i_Vehicle);
                }
                else if (field.Key == "WheelManufacturer")
                {
                    handleWheelAttributes(i_Vehicle, "WheelManufacturer");
                }
                else if (field.Key == "MaxWheelPressure")
                {
                    handleWheelAttributes(i_Vehicle, "MaxWheelPressure");
                }
                else
                {
                    handleGeneralField(field, i_Vehicle, fieldValues);
                }
            }

            i_Vehicle.ValidateSpecificRequirements(errorMessages);

            displayErrorsIfAny(errorMessages);
        }

        private void handleWheelAttributes(Vehicle i_Vehicle, string i_AttributeName)
        {
            Console.WriteLine($"Do you want to set the same value for all wheels for '{i_AttributeName}'? (yes/no):");
            string userChoice;
            do
            {
                userChoice = Console.ReadLine()?.ToLower();
                if (userChoice != "yes" && userChoice != "no")
                {
                    Console.WriteLine("Invalid input. Please enter 'yes' or 'no':");
                }
            } while (userChoice != "yes" && userChoice != "no");

            if (userChoice == "yes")
            {
                while (true)
                {
                    try
                    {
                        Console.Write($"Enter the desired value for all wheels for '{i_AttributeName}': ");
                        string inputValue = Console.ReadLine();

                        switch (i_AttributeName)
                        {
                            case "WheelPressure":
                                float pressure = float.Parse(inputValue);
                                i_Vehicle.InflateAllWheels(pressure);
                                break;

                            case "MaxWheelPressure":
                                float maxPressure = float.Parse(inputValue);
                                foreach (var wheel in i_Vehicle.Wheels)
                                {
                                    wheel.MaxAirPressure = maxPressure;
                                }
                                break;

                            case "WheelManufacturer":
                                foreach (var wheel in i_Vehicle.Wheels)
                                {
                                    wheel.ManufacturerName = inputValue;
                                }
                                break;

                            default:
                                throw new ArgumentException($"Unsupported attribute: {i_AttributeName}");
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Invalid input: {ex.Message}. Please try again.");
                    }
                }
            }
            else
            {
                int wheelCount = i_Vehicle.NumberOfWheels;

                for (int wheelIndex = 0; wheelIndex < wheelCount; wheelIndex++)
                {
                    while (true)
                    {
                        try
                        {
                            Console.Write($"Enter the desired value for wheel {wheelIndex + 1} for '{i_AttributeName}': ");
                            string inputValue = Console.ReadLine();

                            switch (i_AttributeName)
                            {
                                case "WheelPressure":
                                    float pressure = float.Parse(inputValue);
                                    i_Vehicle.InflateWheel(wheelIndex, pressure);
                                    break;

                                case "MaxWheelPressure":
                                    float maxPressure = float.Parse(inputValue);
                                    i_Vehicle.Wheels[wheelIndex].MaxAirPressure = maxPressure;
                                    break;

                                case "WheelManufacturer":
                                    i_Vehicle.Wheels[wheelIndex].ManufacturerName = inputValue;
                                    break;

                                default:
                                    throw new ArgumentException($"Unsupported attribute: {i_AttributeName}");
                            }
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Invalid input for wheel {wheelIndex + 1}: {ex.Message}. Please try again.");
                        }
                    }
                }
            }
        }

        private void handleNumberOfWheels(Vehicle i_Vehicle)
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter the number of wheels (must be a positive number): ");
                    string input = Console.ReadLine();

                    if (!int.TryParse(input, out int numberOfWheels) || numberOfWheels <= 0)
                    {
                        Console.WriteLine("Error: Number of wheels must be greater than 0. Please try again.");
                        continue;
                    }

                    i_Vehicle.NumberOfWheels = numberOfWheels;
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}. Please try again.");
                }
            }
        }

        private void handleGeneralField(KeyValuePair<string, FieldInfo> i_Field, Vehicle i_Vehicle, Dictionary<string, object> io_FieldValues)
        {
            while (true)
            {
                try
                {
                    Console.Write($"{i_Field.Value.Prompt} ");
                    string userInput = Console.ReadLine();
                    object parsedValue;

                    if (i_Field.Value.DataType.IsEnum)
                    {
                        try
                        {
                            parsedValue = Enum.Parse(i_Field.Value.DataType, userInput, true);
                        }
                        catch
                        {
                            throw new ArgumentException($"Invalid value '{userInput}' for {i_Field.Key}. Expected one of: {string.Join(", ", Enum.GetNames(i_Field.Value.DataType))}");
                        }
                    }
                    else
                    {
                        parsedValue = Convert.ChangeType(userInput, i_Field.Value.DataType);
                    }

                    i_Vehicle.SetFieldValue(i_Field.Key, parsedValue);
                    io_FieldValues.Add(i_Field.Key, parsedValue);
                    break;
                }
                catch (Exception ex)
                {
                    string cleanMessage = ex.Message.Replace("\r", "").Replace(Environment.NewLine, " ").Trim();
                    Console.WriteLine($"Error for field '{i_Field.Key}': {cleanMessage}. Please try again.");
                }
            }
        }

        private void displayErrorsIfAny(List<string> i_ErrorMessages)
        {
            if (i_ErrorMessages.Any())
            {
                Console.WriteLine(Environment.NewLine + "This vehicle does not meet our garage's requirements:");
                foreach (var error in i_ErrorMessages)
                {
                    Console.WriteLine(error);
                }
            }
        }

        private void displayLicenseNumbersUI()
        {
            string userChoice;
            do
            {
                Console.WriteLine("Do you want to filter license numbers by vehicle status? (yes/no)");
                userChoice = Console.ReadLine()?.Trim().ToLower();

                if (userChoice != "yes" && userChoice != "no")
                {
                    Console.WriteLine("Invalid input. Please enter 'yes' or 'no':");
                }
            } while (userChoice != "yes" && userChoice != "no");

            if (userChoice == "yes")
            {
                Console.WriteLine("Select the filter status:");
                List<string> statusOptions = r_GarageManager.GetVehicleStatuses();

                for (int i = 0; i < statusOptions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {statusOptions[i]}");
                }

                int statusChoice = getUserChoice(1, statusOptions.Count);
                string selectedStatusString = statusOptions[statusChoice - 1];
                eGarageStatus selectedStatus = (eGarageStatus)Enum.Parse(typeof(eGarageStatus), selectedStatusString);

                Console.WriteLine("Filtered license numbers:");
                foreach (var licenseNumber in r_GarageManager.DisplayLicenseNumbers(selectedStatus))
                {
                    Console.WriteLine(licenseNumber);
                }
            }
            else if (userChoice == "no")
            {
                Console.WriteLine("All license numbers:");
                foreach (var licenseNumber in r_GarageManager.DisplayLicenseNumbers())
                {
                    Console.WriteLine(licenseNumber);
                }
            }
        }

        private void changeVehicleStatusUI()
        {
            Console.Write("Enter the license number of the vehicle: ");
            string licenseNumber = Console.ReadLine();

            Console.WriteLine("Select the new status for the vehicle:");

            List<string> statusOptions = r_GarageManager.GetVehicleStatuses();
            for (int statusIndex = 0; statusIndex < statusOptions.Count; statusIndex++)
            {
                Console.WriteLine($"{statusIndex + 1}. {statusOptions[statusIndex]}");
            }

            int statusChoice = getUserChoice(1, statusOptions.Count) - 1;
            string selectedStatusString = statusOptions[statusChoice];

            try
            {
                string selectedStatus = statusOptions[statusChoice];
                r_GarageManager.ChangeVehicleStatus(licenseNumber, selectedStatus);
                Console.WriteLine($"Vehicle status updated successfully to '{selectedStatus}'.");
            }            
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void refuelVehicleUI()
        {
            try
            {
                Console.Write("Enter the license number of the vehicle: ");
                string licenseNumber = Console.ReadLine();

                if (!r_GarageManager.IsVehicleInGarage(licenseNumber))
                {
                    Console.WriteLine($"Vehicle with license number {licenseNumber} is not in the garage.");
                    return;
                }

                Vehicle vehicle = r_GarageManager.GetVehicleByLicenseNumber(licenseNumber);
                if (!vehicle.IsGas)
                {
                    Console.WriteLine($"Vehicle with license number {licenseNumber} is not powered by fuel.");
                    return;
                }

                Console.Write("Enter the amount of fuel to refuel: ");
                float fuelAmount = float.Parse(Console.ReadLine());

                Console.WriteLine("Select the fuel type:");
                List<string> fuelTypes = r_GarageManager.GetFuelTypes();
                for (int index = 0; index < fuelTypes.Count; index++)
                {
                    Console.WriteLine($"{index + 1}. {fuelTypes[index]}");
                }

                int fuelTypeChoice = getUserChoice(1, fuelTypes.Count);
                string selectedFuelTypeString = fuelTypes[fuelTypeChoice - 1];

                r_GarageManager.RefuelVehicle(licenseNumber, fuelAmount, selectedFuelTypeString);
                Console.WriteLine("Refueling completed successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (ValueOutOfRangeException ex)
            {
                Console.WriteLine($"Error: {ex.Message} (Valid range: {ex.GetMinValue()} to {ex.GetMaxValue()})");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }

        private void rechargeVehicleUI()
        {
            try
            {
                Console.Write("Enter the license number of the vehicle: ");
                string licenseNumber = Console.ReadLine();

                if (!r_GarageManager.IsVehicleInGarage(licenseNumber))
                {
                    Console.WriteLine($"Vehicle with license number {licenseNumber} is not in the garage.");
                    return;
                }
                Vehicle vehicle = r_GarageManager.GetVehicleByLicenseNumber(licenseNumber);

                if (vehicle.IsGas)
                {
                    Console.WriteLine($"Vehicle with license number {licenseNumber} is not powered by electricity.");
                    return;
                }

                Console.Write("Enter the amount of minutes to recharge: ");
                float rechargeMinutes = float.Parse(Console.ReadLine());

                r_GarageManager.RechargeVehicle(licenseNumber, rechargeMinutes);
                Console.WriteLine("Recharge completed successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (ValueOutOfRangeException ex)
            {
                Console.WriteLine($"Error: {ex.Message} (Valid range: {ex.GetMinValue()} to {ex.GetMaxValue()})");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }

        private int getUserChoice(int i_Min, int i_Max)
        {
            int choice;

            while (true)
            {
                Console.Write($"Enter your choice ({i_Min}-{i_Max}): ");
                if (int.TryParse(Console.ReadLine(), out choice) && choice >= i_Min && choice <= i_Max)
                {
                    break;
                }

                Console.WriteLine("Invalid choice. Please try again.");
            }

            return choice;
        }


        private void inflateAllWheelsToMax()
        {
            Console.Write("Enter the license number of the vehicle: ");
            string licenseNumber = Console.ReadLine();

            try
            {
                r_GarageManager.InflateWheelsToMax(licenseNumber);
                Console.WriteLine("Wheels inflated successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void displayFullVehicleDetailsUI()
        {
            try
            {
                Console.Write("Enter the license number of the vehicle: ");
                string licenseNumber = Console.ReadLine();

                if (!r_GarageManager.IsVehicleInGarage(licenseNumber))
                {
                    Console.WriteLine($"Vehicle with license number {licenseNumber} is not in the garage.");
                    return;
                }

                Vehicle vehicle = r_GarageManager.GetVehicleByLicenseNumber(licenseNumber);
                string details = vehicle.GetVehicleDetails();
                Console.WriteLine(details);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}