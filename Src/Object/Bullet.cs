using Godot;
using System;

namespace Unit
{
    public class Bullet : RigidBody
    {
        protected const int SPEED = 5;
        protected Tower _Parent;
        protected Unit _Target;
        protected int _Damage;
        public override void _Ready()
        {
            // _Target = _Parent.Target;
            // _Parent = GetParent() as Tower
            SetAsToplevel(true);
        }
        public override void _PhysicsProcess(float delta)
        {
            ApplyImpulse(Transform.basis.z, -Transform.basis.z*SPEED);
            // if(_Target != null)
            // {
            //     // LookAt(_Target.GlobalTransform.origin,Vector3.Up);
            //     // MoveAndCollide(_Target.GlobalTransform.origin);
            // }
        }
        public void _on_Bullet_body_entered(Node body)
        {
            Unit EnteredUnit = body as Unit;
            if(EnteredUnit != null)
                if(EnteredUnit.Team == Team.Enemy || EnteredUnit.Team == Team.Enemy1)
                {
                    EnteredUnit.HP -= _Damage;
                    QueueFree();
                }
                else QueueFree();
            else QueueFree();
        }
        public Unit Target
        {
            get{ return _Target;}
            set{ _Target = value;}
        }
        public int Damage
        {
            get{ return _Damage;}
            set{ _Damage = value;}
        }
    }
}
