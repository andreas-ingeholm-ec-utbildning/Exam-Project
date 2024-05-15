namespace App.Services;

public static class StringExtensions
{
    public static string RemoveWhitespace(this string str)
    {
        str = str.ReplaceLineEndings("");

        while (str.Contains("  "))
            str = str.Replace("  ", "");
        return str;
    }
}
