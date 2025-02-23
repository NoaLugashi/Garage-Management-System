using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class MotorcycleProperties
    {
        private eLicenseType m_LicenseType;
        private int m_EngineVolume;

        public eLicenseType LicenseType
        {
            get => m_LicenseType;
            set
            {
                if (!Enum.IsDefined(typeof(eLicenseType), value))
                {
                    throw new ArgumentException($"Invalid license type. Allowed types are: {string.Join(", ", Enum.GetNames(typeof(eLicenseType)))}");
                }

                m_LicenseType = value;
            }
        }

        public int EngineVolume
        {
            get => m_EngineVolume;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Engine volume must be a positive number.");
                }
                m_EngineVolume = value;
            }
        }

        public Dictionary<string, FieldInfo> GetRequiredFields()
        {
            return new Dictionary<string, FieldInfo>
            {
                { "LicenseType", new FieldInfo("Enter license type (A1, A2, B1, B2):", typeof(eLicenseType)) },
                { "EngineVolume", new FieldInfo("Enter engine volume in cc:", typeof(int)) }
            };
        }

        public bool SetFieldValue(string i_FieldName, object i_Value)
        {
            bool isFieldSet = true;

            switch (i_FieldName)
            {
                case "LicenseType":
                    LicenseType = (eLicenseType)i_Value;
                    break;
                case "EngineVolume":
                    EngineVolume = (int)i_Value;
                    break;
                default:
                    isFieldSet = false;
                    break;
            }

            return isFieldSet;
        }

        public void ValidateMotorcycleProperties(List<string> io_ErrorMessages)
        {
            if (!Enum.IsDefined(typeof(eLicenseType), m_LicenseType))
            {
                io_ErrorMessages.Add($"Invalid license type: {m_LicenseType}.");
            }

            if (m_EngineVolume <= 0)
            {
                io_ErrorMessages.Add("Engine volume must be a positive number.");
            }
        }

        public string GetMotorcycleDetails()
        {
            return $"License Type: {m_LicenseType}, Engine Volume: {m_EngineVolume}cc";
        }
    }
}