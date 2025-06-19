---
mode: 'agent'
description: 'General pourpose project intro prompt for working with this project with copilot'
tools: ['codebase', 'fetch', 'githubRepo']
---
This project is a learning puprose project to learn how the COM/WinRT works,
by implementing a mini-COM/mini-WinRT component/comsumer by hand.

To check our implementation actually works,
we use C# project to use a C++ component to ensure ABI the COM/WinRT works.
We use different language for implementation and comsumer to ensure that we are not using any language specific feature,
and we are using the ABI only.

To check our implementation actually works as standard COM/WinRT spec,
we use C#'s existing COM/WinRT support to use a C++ component to ensure ABI the COM/WinRT works,
thus if C#'s standard COM/WinRT support can use our C++ component,
then our implementation is compliant with the COM/WinRT spec.

When working on this project with some large tasks,
please consider using [memory](./memory.prompt.md) to split the task into smaller tasks and persist the plan into file so another round of chat/agent session could pick them up.

When performing changes, please try to come up with proper way to validate those changes first,
one common way is to write a test case to ensure the changes are correct.

There is a roadmap/plan for this project, please check [README](./README.md).