#pragma once
#include "Circle.g.h"

namespace winrt::WinRTCppRuntimeComponent::implementation
{
    struct Circle : CircleT<Circle>
    {
        float m_size;
        Circle() = default;

        Circle(float size);
        float Radius();
        void Show();
        float Area();
    };
}
namespace winrt::WinRTCppRuntimeComponent::factory_implementation
{
    struct Circle : CircleT<Circle, implementation::Circle>
    {
    };
}
