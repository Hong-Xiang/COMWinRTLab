# Prelude

Let's start journey on learning COM/WinRT by write a mini-COM/WinRT system.

Since COM/WinRT are designed as a application binary interface standard,
it supports dynamic composing components in a language-agnostic way.

As a consequence, to learn it, 
we need to understand the core concepts and gradually build our knowledge through practical implementation,
in both cpp and csharp language.

It is actually a little challenge to check our understanding
since all practical languages provide methods to write program in a modular way,
of course including cpp and csharp.
thus we can easily get confused on the across binary boundaries behavour if we happens to use language's builtin way of linking,
thus to avoid those confusion,
we stick on using explicit dynamic linking methods in both cpp and csharp language,
thus simply the implementation and consumer must stay in different dlls,
no static linking is used.
Another rule we set to avoid accidental coupling is we try to implement and consume interfaces in different languages,
e.g. implement in cpp and consume in csharp, to avoid any language builtin functionalities of ABI.

Our plan is seperated into several steps, and in each step, we would write a md file for its details with its own code.
We try to minimumize tool usage to find the actual essential steps,
but after we figure out the essential steps,
we will use current tools with same functionalities to simplify our code,
and move forward to higher level abstractions.

To simplify discussion, we use term `client` for the code that consumes the interfaces, and `server` for the code that implements the interfaces.

Our plan is:

1. Dynamic Linking - Create separate dlls, and calling functions across them, in both cpp and csharp

