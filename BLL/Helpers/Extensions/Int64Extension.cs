namespace BLL.Helpers.Extensions
{
    public static class Int64Extension
    {
        public static bool IsZero(this long value)
        {
            if (value == 0)
                return true;
            else return false;
        }
    }
}