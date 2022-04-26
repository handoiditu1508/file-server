using Fries.Helpers.Extensions;
using Fries.Models.Common;

namespace Fries.Helpers
{
    public class CustomException : Exception
    {
        public string Group { get; set; }

        public string Code { get; set; }

        public CustomException(EnumExceptionGroup group, string code, string message) : base(message)
        {
            Group = group.GetDescription();
            Code = code;
        }

        public static class System
        {
            public static readonly CustomException UnexpectedError = new(EnumExceptionGroup.System, "SYSTEM_001", "Unexpected error.");
        }

        public static class Validation
        {
            public static CustomException PropertyIsNullOrEmpty(string propertyName) => new CustomException(EnumExceptionGroup.Validation, "VALIDATION_001", $"Property {propertyName} is null or empty.");
            public static CustomException PropertyIsInvalid(string propertyName) => new CustomException(EnumExceptionGroup.Validation, "VALIDATION_002", $"Property {propertyName} is invalid.");
        }

        public static class FilesStorage
        {
            public static CustomException FileNotFound(string file) => new(EnumExceptionGroup.FilesStorage, "FILES_STORAGE_001", $"File \"{file}\" not found.");
        }
    }
}
