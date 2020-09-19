#ifndef COLLISION_H
#define COLLISION_H
#include <Ogre.h>
#include "../Collider/Collider.h"
using namespace Ogre;
class Collider;
class Collision{
public:
    Collision();
    bool result;
    Vector3 point;
    int Initiator;
    int Victim;
    Vector3 normal;
};
#endif // !COLLISION