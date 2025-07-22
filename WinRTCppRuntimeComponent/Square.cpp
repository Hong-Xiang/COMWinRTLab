#include "pch.h"
#include "Square.h"
#include "Square.g.cpp"
#include <iostream>

namespace winrt::WinRTCppRuntimeComponent::implementation {
	Square::Square(float size) : m_size(size) {}

	float Square::Size() { return m_size; }

	void Square::Show() { std::cout << "Square with size " << m_size << std::endl; }
	float Square::Area() { return m_size * m_size; }
} // namespace winrt::WinRTCppRuntimeComponent::implementation
