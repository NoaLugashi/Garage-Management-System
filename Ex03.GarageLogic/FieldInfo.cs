using System;

namespace Ex03.GarageLogic
{
    public class FieldInfo
    {
        public string Prompt;
        public Type DataType;

        public FieldInfo(string prompt, Type dataType)
        {
            Prompt = prompt;
            DataType = dataType;
        }
    }
}