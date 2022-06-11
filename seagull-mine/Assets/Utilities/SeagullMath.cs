namespace Utilities
{
    public static class SeagullMath
    {
        public static float EaseInQuart(float x)
        {
            return x * x * x * x;
        }
        
        public static float EaseInQuint(float x)
        {
            return x * x * x * x * x;
        }
    }
}
