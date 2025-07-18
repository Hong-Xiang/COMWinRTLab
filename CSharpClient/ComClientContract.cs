using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace CSharpClient;


[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("2E55DE61-D7ED-44BE-9B62-F340AF70721D")]
partial interface IHello
{
    void Hello(int data);
}

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("EA71AB58-8226-4FCB-A156-DC53E1EE91A9")]
interface IAdderInterface
{
    int Add(int a, int b);
}
