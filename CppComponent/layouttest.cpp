#include <cstdio> // for printf

struct IDraw
{
    virtual void __stdcall Draw() = 0;
};

struct IShape : IDraw
{
    virtual double __stdcall Area() = 0;
    virtual double __stdcall Perimeter() = 0;
};

struct IHasCorner
{
    virtual int __stdcall Corners() = 0;
};

struct Circle : IShape
{
    double m_radius;
    Circle(double radius) : m_radius(radius) {}
    double __stdcall Area() override
    {
        return 3.14 * m_radius * m_radius;
    }
    double __stdcall Perimeter() override
    {
        return 2 * 3.14 * m_radius;
    }
    void __stdcall Draw() override
    {
        printf("Circle\n");
    }
};

struct Square : IShape, IHasCorner
{
    double m_side;
    Square(double side) : m_side(side) {}
    double __stdcall Area() override
    {
        return m_side * m_side;
    }
    double __stdcall Perimeter() override
    {
        return 4 * m_side;
    }
    void __stdcall Draw() override
    {
        printf("Square\n");
    }
    int __stdcall Corners() override
    {
        return 4;
    }
};

template <typename T>
void dump_layout()
{
    volatile size_t s = sizeof(T);
}

// Explicit instantiations
template void dump_layout<Circle>();
template void dump_layout<Square>();
template void dump_layout<IDraw>();
template void dump_layout<IShape>();
template void dump_layout<IHasCorner>();