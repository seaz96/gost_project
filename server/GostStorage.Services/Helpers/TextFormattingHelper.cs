namespace GostStorage.Services.Helpers;

public static class TextFormattingHelper
{
    public static string FormatDesignation(string text)
    {
        return text.Replace('–', '-').Trim();
    }
}