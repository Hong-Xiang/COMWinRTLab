using System.Runtime.InteropServices;

namespace CSharpConsumer;


[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
//[GeneratedComInterface]
[Guid("0D65A935-2857-4487-9CCD-633D40563C4E")]
public partial interface ICalculator
{
    void Hello();
    [PreserveSig]
    int Add(int a, int b);
}

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
//[GeneratedComInterface]
[Guid("497A7BFE-BB9C-4DDF-914E-5DE5602325FF")]
public partial interface ICalculator2
{
    int Mul(int a, int b);
}


