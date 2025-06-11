using System.Runtime.InteropServices;

namespace CSharpConsumer;

public static partial class ComWrapperConsumer
{
    public static ICalculator CreateCalculator()
    {
        var ptr = NativeMethods.CreateCalculator();
        return (ICalculator)Marshal.GetObjectForIUnknown(ptr);
    }

    public static ICalculator2 CastAsCalculator2(ICalculator c)
    {
        return (ICalculator2)c;
    }
}

