using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CVRecognizingService.Domain.Enums
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
            => enumValue.GetType()?
            .GetMember(enumValue.ToString())?
            .FirstOrDefault()?
            .GetCustomAttribute<DisplayAttribute>()?
            .Name ?? string.Empty;
    }
}
