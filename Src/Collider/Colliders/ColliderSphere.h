#ifndef COLLIDERSPHERE_H
#define COLLIDERSPHERE_H
#include "../Collider.h"
class ColliderSphere : public Collider{
public:
    Collision collide(Collider);
}

#endif // !COLLIDERSPHERE_H
