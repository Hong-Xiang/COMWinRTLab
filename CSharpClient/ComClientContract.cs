using System.Runtime.InteropServices;

namespace CSharpClient;


[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("2E55DE61-D7ED-44BE-9B62-F340AF70721D")]
interface IHelloInterface
{
    void Hello(int data);
}

