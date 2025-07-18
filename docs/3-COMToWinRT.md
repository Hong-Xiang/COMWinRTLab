# More Primitive Types

In COM, string is a relative complex topic,
there are different character sets, 
and different string types,
one of the most common string types is BSTR,
which is a wchar_t buffer with a length prefix and a null terminator,
the problem is that BSTR need to be allocated and deallocated using same memory allocator,
thus we have a special allocator for BSTR.