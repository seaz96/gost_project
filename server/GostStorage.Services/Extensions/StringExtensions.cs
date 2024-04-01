namespace GostStorage.Services.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharToUpperNextToLower(this string input)
        {
            return input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => string.Concat(input[0].ToString().ToUpper(), input[1..].ToString().ToLower())
            };
        }
    }
}
