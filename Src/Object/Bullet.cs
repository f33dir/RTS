using Godot;
using System;

namespace Unit
{
    public class Bullet : RigidBody
    {
        protected const int SPEED = 10;
        // protected Tower _Parent;
        // protected DynamicUnit _Target;
        protected CollisionShape _AOEAreaCollision;
        protected DynamicUnit _PrimaryTarget;
        protected int _Damage;
        public override void _Ready()
        {
            _AOEAreaCollision = GetNode<CollisionShape>("AOEArea/CollisionShape");
            SetAsToplevel(true);
        }
        public override void _PhysicsProcess(float delta)
        {
            ApplyImpulse(Transform.basis.z, -Transform.basis.z*SPEED);
        }
        public void _on_Bullet_body_entered(Node body)
        {
            DynamicUnit EnteredUnit = body as DynamicUnit;
            if(EnteredUnit != null)
                if(EnteredUnit.Team == Team.Enemy || EnteredUnit.Team == Team.Enemy1)
                {
                    EnteredUnit.HP -= _Damage;
                    _PrimaryTarget = EnteredUnit;
                    QueueFree();
                }
                else QueueFree();
            else QueueFree();
        }
        public void _on_Bullet_AOE_body_entered(Node body)
        {
            DynamicUnit EnteredUnit = body as DynamicUnit;
            if(EnteredUnit != null)
                if((EnteredUnit.Team == Team.Enemy || EnteredUnit.Team == Team.Enemy1) && _PrimaryTarget != null)
                {
                    if(EnteredUnit != _PrimaryTarget)
                        EnteredUnit.HP -= _Damage;
                    QueueFree();
                }
        }
        public int Damage
        {
            get{ return _Damage;}
            set{ _Damage = value;}
        }
    }
}
