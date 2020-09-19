#ifndef COLLIDERBOX_H
#define COLLIDERBOX_H
#include "../Collider.h"
class ColliderBox : public Collider{
public:
    Collision collide(Collider);
};
#endif // !COLLIDERBOX_H
