using Godot;
using System;

namespace Unit
{
    public class Bullet : RigidBody
    {
        protected const int SPEED = 10;
        protected CollisionShape _AOEAreaCollision;
        protected Godot.Collections.Array<DynamicUnit> _UnitsInAOE;
        protected int _Damage;
        protected bool _IsAOE = false;
        protected bool _IsFreezing = false;
        public override void _Ready()
        {
            _AOEAreaCollision = GetNode<CollisionShape>("AOEArea/CollisionShape");
            _UnitsInAOE = new Godot.Collections.Array<DynamicUnit>();
            SetAsToplevel(true);
        }
        public override void _PhysicsProcess(float delta)
        {
            ApplyImpulse(Transform.basis.z, -Transform.basis.z*SPEED);
            if(_UnitsInAOE.Count != 0)
            {
                foreach (var unit in _UnitsInAOE)
                {
                    unit.HP -= _Damage;
                    if(_IsFreezing)
                    {
                        unit.MoveSpeed *= 0.75f;
                        unit.SlowDebuffTimer.Start();
                    }
                }
                QueueFree();
            }
        }
        public void _on_Bullet_body_entered(Node body)
        {
            if(!_IsAOE)
            {
                DynamicUnit EnteredUnit = body as DynamicUnit;
                if(EnteredUnit != null)
                    if(EnteredUnit.Team == Team.Enemy || EnteredUnit.Team == Team.Enemy1)
                    {
                        EnteredUnit.HP -= _Damage;
                        if(_IsFreezing)
                        {
                            EnteredUnit.MoveSpeed *= 0.75f;
                            EnteredUnit.SlowDebuffTimer.Start();
                        }
                        QueueFree();
                    }
                    else QueueFree();
                else QueueFree();
            }
            else
            {
                _AOEAreaCollision.Disabled = false;
            }
        }
        public void _on_Bullet_AOE_body_entered(Node body)
        {
            DynamicUnit EnteredUnit = body as DynamicUnit;
            if(EnteredUnit != null)
                if((EnteredUnit.Team == Team.Enemy || EnteredUnit.Team == Team.Enemy1))
                {
                    _UnitsInAOE.Add(EnteredUnit);
                }
        }
        public int Damage
        {
            get{ return _Damage;}
            set{ _Damage = value;}
        }
        public bool IsAOE
        {
            get { return _IsAOE;}
            set{ _IsAOE = value; }
        }
        public bool IsFreezing
        {
            get{ return _IsFreezing;}
            set{ _IsFreezing = value;}
        }
    }
}
