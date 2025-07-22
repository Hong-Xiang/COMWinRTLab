#pragma once
#include "Square.g.h"

namespace winrt::WinRTCppRuntimeComponent::implementation
{
    struct Square : SquareT<Square>
    {
        float m_size;
        Square() = default;

        Square(float size);
        float Size();
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
