#include <iostream>
#include <cstdlib>
#include <cstring>

// Linux-compatible GUID and COM-like structures
typedef struct _GUID {
    unsigned int   Data1;
    unsigned short Data2;
    unsigned short Data3;
    unsigned char  Data4[8];
} GUID;

typedef int HRESULT;
typedef wchar_t OLECHAR;

// HRESULT values
#define S_OK                    ((HRESULT)0L)
#define E_NOINTERFACE           ((HRESULT)0x80004002L)

// Linux-compatible function calling convention (no __stdcall)
#define STDCALL

namespace LinuxCOMServer
{
    // Forward declarations
    struct IUnknownVTbl;
    struct IHelloVTbl;
    struct IAdderVTbl;

    struct MySimpleObject {
        IHelloVTbl* VTable;
        int RefCount;
    };

    // Function pointer typedefs for COM IUnknownVTbl interface
    typedef HRESULT(STDCALL* QueryInterfaceFunc)(void* This, const GUID* riid, void** ppvObject);
    typedef unsigned long(STDCALL* AddRefFunc)(void* This);
    typedef unsigned long(STDCALL* ReleaseFunc)(void* This);
    typedef HRESULT(STDCALL* HelloFunc)(void* This, int data);
    typedef int(STDCALL* AddFunc)(void* This, int a, int b);

    struct IUnknownVTbl
    {
        QueryInterfaceFunc QueryInterface;
        AddRefFunc AddRef;
        ReleaseFunc Release;
    };

    struct IHelloVTbl {
        QueryInterfaceFunc QueryInterface;
        AddRefFunc AddRef;
        ReleaseFunc Release;
        HelloFunc Hello;
    };

    struct IAdderVTbl {
        QueryInterfaceFunc QueryInterface;
        AddRefFunc AddRef;
        ReleaseFunc Release;
        AddFunc Add;
    };

    // Combined object that supports both interfaces
    struct MyCombinedObject {
        IHelloVTbl* HelloVTable;
        IAdderVTbl* AdderVTable;
        int RefCount;
    };

    // GUID definitions
    static GUID IUnknownGUID{ 0x00000000, 0x0000, 0x0000, {0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46} };
    static GUID IHelloInterface{ 0x2E55DE61, 0xD7ED, 0x44BE, {0x9B, 0x62, 0xF3, 0x40, 0xAF, 0x70, 0x72, 0x1D} };
    static GUID IAdderInterface{ 0xEA71AB58, 0x8226, 0x4FCB, {0xA1, 0x56, 0xDC, 0x53, 0xE1, 0xEE, 0x91, 0xA9} };

    // Helper function to compare GUIDs
    bool IsEqualGUID(const GUID& guid1, const GUID& guid2) {
        return memcmp(&guid1, &guid2, sizeof(GUID)) == 0;
    }

    // Helper function to print GUID
    void PrintGUID(const GUID* guid) {
        printf("{%08X-%04X-%04X-%02X%02X-%02X%02X%02X%02X%02X%02X}",
               guid->Data1, guid->Data2, guid->Data3,
               guid->Data4[0], guid->Data4[1], guid->Data4[2], guid->Data4[3],
               guid->Data4[4], guid->Data4[5], guid->Data4[6], guid->Data4[7]);
    }

    HRESULT STDCALL QueryInterfaceImpl(MySimpleObject* self, const GUID* riid, void** ppvObject)
    {
        printf("QueryInterface called with self %p and riid ", self);
        PrintGUID(riid);
        printf("\n");
        
        if (IsEqualGUID(*riid, IUnknownGUID) || IsEqualGUID(*riid, IHelloInterface)) {
            *ppvObject = self;
            self->RefCount++;
            return S_OK;
        }
        return E_NOINTERFACE;
    }

    HRESULT STDCALL QueryInterfaceCombinedImpl(MyCombinedObject* self, const GUID* riid, void** ppvObject)
    {
        printf("QueryInterface (Combined) called with self %p and riid ", self);
        PrintGUID(riid);
        printf("\n");
        
        if (IsEqualGUID(*riid, IUnknownGUID) || IsEqualGUID(*riid, IHelloInterface)) {
            *ppvObject = self; // Return the object starting with HelloVTable
            self->RefCount++;
            return S_OK;
        }
        else if (IsEqualGUID(*riid, IAdderInterface)) {
            // Return pointer adjusted to AdderVTable
            *ppvObject = &(self->AdderVTable);
            self->RefCount++;
            return S_OK;
        }
        return E_NOINTERFACE;
    }

    unsigned long STDCALL AddRefImpl(MySimpleObject* self)
    {
        printf("AddRef called\n");
        self->RefCount++;
        return self->RefCount;
    }

    unsigned long STDCALL AddRefCombinedImpl(MyCombinedObject* self)
    {
        printf("AddRef (Combined) called\n");
        self->RefCount++;
        return self->RefCount;
    }

    unsigned long STDCALL ReleaseImpl(MySimpleObject* self)
    {
        printf("Release called\n");
        self->RefCount--;
        if (self->RefCount == 0)
        {
            printf("Freeing memory\n");
            free(self);
            return 0;
        }
        return self->RefCount;
    }

    unsigned long STDCALL ReleaseCombinedImpl(MyCombinedObject* self)
    {
        printf("Release (Combined) called\n");
        self->RefCount--;
        if (self->RefCount == 0)
        {
            printf("Freeing combined memory\n");
            free(self);
            return 0;
        }
        return self->RefCount;
    }

    // Helper to get the correct object pointer from AdderVTable pointer
    MyCombinedObject* GetCombinedObjectFromAdder(void* adderPtr)
    {
        // Calculate offset from AdderVTable to start of object
        return reinterpret_cast<MyCombinedObject*>(
            reinterpret_cast<char*>(adderPtr) - offsetof(MyCombinedObject, AdderVTable)
        );
    }

    unsigned long STDCALL AddRefAdderImpl(void* self)
    {
        MyCombinedObject* obj = GetCombinedObjectFromAdder(self);
        return AddRefCombinedImpl(obj);
    }

    unsigned long STDCALL ReleaseAdderImpl(void* self)
    {
        MyCombinedObject* obj = GetCombinedObjectFromAdder(self);
        return ReleaseCombinedImpl(obj);
    }

    HRESULT STDCALL QueryInterfaceAdderImpl(void* self, const GUID* riid, void** ppvObject)
    {
        MyCombinedObject* obj = GetCombinedObjectFromAdder(self);
        return QueryInterfaceCombinedImpl(obj, riid, ppvObject);
    }

    HRESULT STDCALL HelloImpl(MySimpleObject* self, int data) {
        printf("Hello with data %d from COM, with ref count %d\n", data, self->RefCount);
        return S_OK;
    }

    HRESULT STDCALL HelloCombinedImpl(MyCombinedObject* self, int data) {
        printf("Hello (Combined) with data %d from COM, with ref count %d\n", data, self->RefCount);
        return S_OK;
    }

    int STDCALL AddImpl(void* self, int a, int b) {
        MyCombinedObject* obj = GetCombinedObjectFromAdder(self);
        printf("Add called: %d + %d = %d (ref count: %d)\n", a, b, a + b, obj->RefCount);
        int result = a + b;
        return result;
    }

    // VTables
    static IHelloVTbl MySimpleObjectHelloVTable{
        .QueryInterface = (QueryInterfaceFunc)QueryInterfaceImpl,
        .AddRef = (AddRefFunc)AddRefImpl,
        .Release = (ReleaseFunc)ReleaseImpl,
        .Hello = (HelloFunc)HelloImpl
    };

    static IHelloVTbl MyCombinedObjectHelloVTable{
        .QueryInterface = (QueryInterfaceFunc)QueryInterfaceCombinedImpl,
        .AddRef = (AddRefFunc)AddRefCombinedImpl,
        .Release = (ReleaseFunc)ReleaseCombinedImpl,
        .Hello = (HelloFunc)HelloCombinedImpl
    };

    static IAdderVTbl MyCombinedObjectAdderVTable{
        .QueryInterface = (QueryInterfaceFunc)QueryInterfaceAdderImpl,
        .AddRef = (AddRefFunc)AddRefAdderImpl,
        .Release = (ReleaseFunc)ReleaseAdderImpl,
        .Add = AddImpl
    };

    MySimpleObject* CreateSimpleObject() {
        MySimpleObject* result = (MySimpleObject*)malloc(sizeof(MySimpleObject));
        result->VTable = &MySimpleObjectHelloVTable;
        result->RefCount = 1;
        return result;
    }

    MyCombinedObject* CreateCombinedObject() {
        MyCombinedObject* result = (MyCombinedObject*)malloc(sizeof(MyCombinedObject));
        result->HelloVTable = &MyCombinedObjectHelloVTable;
        result->AdderVTable = &MyCombinedObjectAdderVTable;
        result->RefCount = 1;
        return result;
    }
}

// Export functions for C#
extern "C" {
    void* CreateMinimumCOMObject()
    {
        printf("CreateMinimumCOMObject called\n");
        return LinuxCOMServer::CreateSimpleObject();
    }

    void* CreateCppCOMObject()
    {
        printf("CreateCppCOMObject called\n");
        return LinuxCOMServer::CreateCombinedObject();
    }

    void HelloFromClientExport()
    {
        printf("Hello from Linux C++ server!\n");
    }

    // Direct function call helpers for Linux interop
    int CallHelloDirectly(void* obj, int data)
    {
        printf("CallHelloDirectly called with obj %p and data %d\n", obj, data);
        
        // Try to call as combined object first
        LinuxCOMServer::MyCombinedObject* combined = static_cast<LinuxCOMServer::MyCombinedObject*>(obj);
        if (combined && combined->HelloVTable) {
            auto helloFunc = combined->HelloVTable->Hello;
            if (helloFunc) {
                return helloFunc(combined, data);
            }
        }
        
        // Fallback to simple object
        LinuxCOMServer::MySimpleObject* simple = static_cast<LinuxCOMServer::MySimpleObject*>(obj);
        if (simple && simple->VTable) {
            auto helloFunc = simple->VTable->Hello;
            if (helloFunc) {
                return helloFunc(simple, data);
            }
        }
        
        printf("Failed to call Hello function\n");
        return -1;
    }

    int CallAddDirectly(void* obj, int a, int b)
    {
        printf("CallAddDirectly called with obj %p, a=%d, b=%d\n", obj, a, b);
        
        // Check if this is a valid pointer
        if (!obj) {
            printf("Error: null object pointer\n");
            return -1;
        }
        
        // Try to determine object type by checking if it has the combined structure
        // This is a heuristic - in a real implementation you'd have better type checking
        LinuxCOMServer::MyCombinedObject* combined = static_cast<LinuxCOMServer::MyCombinedObject*>(obj);
        
        // Check if the object has a valid AdderVTable pointer
        // This is unsafe but necessary for our demo
        try {
            if (combined && combined->AdderVTable) {
                auto addFunc = combined->AdderVTable->Add;
                if (addFunc) {
                    return addFunc(&combined->AdderVTable, a, b);
                }
            }
        } catch (...) {
            // Catch any potential crashes from invalid memory access
            printf("Exception occurred while trying to call Add - object doesn't support IAdder\n");
        }
        
        printf("Failed to call Add function - object doesn't support IAdder\n");
        return -1;
    }

    // Helper to get reference count for debugging
    int GetRefCount(void* obj)
    {
        // Try as combined object first
        LinuxCOMServer::MyCombinedObject* combined = static_cast<LinuxCOMServer::MyCombinedObject*>(obj);
        if (combined) {
            return combined->RefCount;
        }
        
        // Try as simple object
        LinuxCOMServer::MySimpleObject* simple = static_cast<LinuxCOMServer::MySimpleObject*>(obj);
        if (simple) {
            return simple->RefCount;
        }
        
        return -1;
    }

    // Helper to check if object supports a specific interface
    int SupportsInterface(void* obj, const char* interfaceName)
    {
        printf("SupportsInterface called with obj %p for interface %s\n", obj, interfaceName);
        
        if (!obj || !interfaceName) {
            return 0;
        }
        
        // For our demo, we'll do a simple check based on interface name
        if (strcmp(interfaceName, "IHello") == 0) {
            // Both simple and combined objects support IHello
            return 1;
        }
        else if (strcmp(interfaceName, "IAdder") == 0) {
            // Only combined objects support IAdder
            // This is a heuristic - check if the object has both VTables
            LinuxCOMServer::MyCombinedObject* combined = static_cast<LinuxCOMServer::MyCombinedObject*>(obj);
            return (combined && combined->HelloVTable && combined->AdderVTable) ? 1 : 0;
        }
        
        return 0;
    }
}
