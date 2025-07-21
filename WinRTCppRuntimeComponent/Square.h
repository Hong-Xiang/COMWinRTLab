#pragma once
#include "Square.g.h"

namespace winrt::WinRTCppRuntimeComponent::implementation
{
    struct Square : SquareT<Square>
    {
        Square() = default;

        Square(float size);
        void Show();
        float Area();
    };
}
namespace winrt::WinRTCppRuntimeComponent::factory_implementation
{
    struct Square : SquareT<Square, implementation::Square>
    {
    };
}
