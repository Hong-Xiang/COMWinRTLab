#include <cstdio> // for printf

struct IDraw {
    virtual void Draw() = 0;
};

struct IShape : IDraw {
    virtual double Area() = 0;
    virtual double Perimeter() = 0;
};

struct IHasCorner {
    virtual int Corners() = 0;
};


struct Circle : IShape {
    double m_radius;
    Circle(double radius) : m_radius(radius) {}
    double Area() override {
        return 3.14 * m_radius * m_radius;
    }
    double Perimeter() override {
        return 2 * 3.14 * m_radius;
    }
    void Draw() override {
        printf("Circle\n");
    }
};


struct Square : IShape, IHasCorner {
    double m_side;
    Square(double side) : m_side(side) {}
    double Area() override {
        return m_side * m_side;
    }
    double Perimeter() override {
        return 4 * m_side;
    }
    void Draw() override {
        printf("Square\n");
    }
    int Corners() override {
        return 4;
    }
};

template<typename T>
void dump_layout() {
    volatile size_t s = sizeof(T);
}

// Explicit instantiations
template void dump_layout<Circle>();
template void dump_layout<Square>();
template void dump_layout<IDraw>();
template void dump_layout<IShape>();
template void dump_layout<IHasCorner>();