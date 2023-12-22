namespace ChinaCatSunflower.AppHelpers;

public class Fibber
{
    public static long[] Fib(int n) {

        long[] vals = new long[n+1];
        vals[0] = 1;
        vals[1] = 2;
        for(int i=2; i <= n;i++){
            vals[i] = vals[i-1] + vals[i-2];
        }
        return vals;
    }
}