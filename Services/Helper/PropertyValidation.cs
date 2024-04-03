using System.Text.RegularExpressions;

namespace Application.Helper;

public static class PropertyValidation
{
    public static bool IsValidNumber(this string number)
    {
        var regex = new Regex("^[0-9]{10}$");
        return regex.IsMatch(number);
    }
}