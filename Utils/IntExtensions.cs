namespace Utils
{
    public static class IntExtensions
    {
        public static int Mod(this int x, int m) {
            var r = x%m;
            return r<0 ? r+m : r;
        }
    }
}