namespace CSharpConsumerTest;

using CSharpConsumer;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

public partial class ComWrapperUsageTest
{
    [LibraryImport("CppComponent.dll")]
    public static partial int AddC(int a, int b);

    [LibraryImport("CppComponent.dll")]
    public static partial nint CreateCalculator();

    [Fact]
    public void BasicUsageShouldWork()
    {
        var c = ComWrapperConsumer.CreateCalculator();
        Console.WriteLine($"Add 40 2 = {c.Add(40, 2)}");

        ICalculator2 c2 = (ICalculator2)c;
        Assert.Equal(80, c2.Mul(40, 2));
        Console.WriteLine($"40 * 2 = {c2.Mul(40, 2)}");
    }
}
