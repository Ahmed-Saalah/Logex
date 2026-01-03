namespace Logex.API.Helpers
{
    public static class WaybillNumberGenerator
    {
        public static string Generate()
        {
            var year = DateTime.UtcNow.ToString("yy");
            var uniquePart = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
            return $"LOGX-{year}-{uniquePart}";
        }
    }
}
