#ifndef COLLIDER_H
#define COLLIDER_H
#include "../Collision/Collision.h"
class Collision;
class Collider{
    double CollisionRadius;
    Collision collideSphereWithSphere(Collider*,Collider*);
    Collision collideBoxWithBox(Collider*,Collider*);
    Collision collideBoxWithSphere(Collider*,Collider*);
    Collision collidePolyWithSphere(Collider*,Collider*);
    Collision collidePolyWithCube(Collider*,Collider*);
    Collision polyWithPoly(Collider*,Collider*);
public:
    virtual Collision collide(Collider*)=0;
};
#endif