#ifndef COLLIDERPOLY_H
#define COLLIDERPOLY_H
#include "../Collider.h"
class ColliderPoly : public Collider{
public:
    Collision collide(Collider);
};
#endif // !COLLIDERPOLY_H
