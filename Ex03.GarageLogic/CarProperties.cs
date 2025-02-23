using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    public class CarProperties
    {
        private eCarColor m_Color;
        private int m_NumberOfDoors;

        public eCarColor Color
        {
            get => m_Color;
            set
            {
                if (!Enum.IsDefined(typeof(eCarColor), value))
                {
                    throw new ArgumentException($"Invalid color. Allowed colors are: {string.Join(", ", Enum.GetNames(typeof(eCarColor)))}");
                }
                m_Color = value;
            }
        }

        public int NumberOfDoors
        {
            get => m_NumberOfDoors;
            set
            {
                if (value < 2 || value > 5)
                {
                    throw new ArgumentException("Number of doors must be between 2 and 5.");
                }
                m_NumberOfDoors = value;
            }
        }

        public string GetDetails()
        {
            return $"Color: {m_Color}, Number of Doors: {m_NumberOfDoors}";
        }

        public Dictionary<string, FieldInfo> GetRequiredFields()
        {
            return new Dictionary<string, FieldInfo>
            {
                { "Color", new FieldInfo("Enter car color (Blue, Black, White, Gray):", typeof(string)) },
                { "NumberOfDoors", new FieldInfo("Enter the number of doors (2-5):", typeof(int)) }
            };
        }

        public bool SetFieldValue(string i_FieldName, object i_Value)
        {
            bool isSucceeded = true;

            switch (i_FieldName)
            {
                case "Color":
                    if (i_Value is string colorString)
                    {
                        if (Enum.TryParse(colorString, true, out eCarColor parsedColor))
                        {
                            Color = parsedColor;
                        }
                        else
                        {
                            throw new ArgumentException($"Invalid color '{colorString}'. Please choose from: {string.Join(", ", Enum.GetNames(typeof(eCarColor)))}.");
                        }
                    }
                    break;
                case "NumberOfDoors":
                    NumberOfDoors = (int)i_Value;
                    break;
                default:
                    isSucceeded = false;
                    break;
            }

            return isSucceeded;
        }

        public string GetCarDetails()
        {
            var details = new StringBuilder();
            details.AppendLine($"Color: {m_Color}");
            details.AppendLine($"Number of Doors: {m_NumberOfDoors}");
            return details.ToString();
        }

        public void ValidateCarProperties(List<string> io_ErrorMessages)
        {
            if (!Enum.IsDefined(typeof(eCarColor), m_Color))
            {
                io_ErrorMessages.Add($"Error: Invalid car color '{m_Color}'. Allowed colors are: {string.Join(", ", Enum.GetNames(typeof(eCarColor)))}.");
            }

            if (m_NumberOfDoors < 2 || m_NumberOfDoors > 5)
            {
                io_ErrorMessages.Add($"Error: Invalid number of doors '{m_NumberOfDoors}'. Must be between 2 and 5.");
            }
        }
    }
}