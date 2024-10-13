namespace GostStorage.Helpers;

public static class TextFormattingHelper
{
    public static string FormatDesignation(string text)
    {
        return text.Replace('–', '-').Trim();
    }
}