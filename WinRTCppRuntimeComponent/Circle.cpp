#include "pch.h"
#include "Circle.h"
#include "Circle.g.cpp"
#include <iostream>

namespace winrt::WinRTCppRuntimeComponent::implementation {
	Circle::Circle(float size) : m_size(size) {}
	float Circle::Radius() { return m_size; }

	void Circle::Show() {
		std::cout << "Circle with size " << m_size << std::endl;
	}
	float Circle::Area() { return 3.14159 * m_size * m_size; }
} // namespace winrt::WinRTCppRuntimeComponent::implementation
