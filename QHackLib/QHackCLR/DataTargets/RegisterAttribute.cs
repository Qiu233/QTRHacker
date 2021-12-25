using System;

namespace QHackCLR.DataTargets
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class RegisterAttribute : Attribute
    {
        public string Name;
        public RegisterType RegisterType;

        public RegisterAttribute(RegisterType registerType)
        {
            RegisterType = registerType;
        }
    }
}
