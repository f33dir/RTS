using System;
using Godot;
using System.Collections.Generic;

namespace Unit
{
    public enum RangedUnitID
    {
        Sharpshooter,
        Sniper,
        Tank,
    }
    public enum MeleeUnitID
    {
        Brawler,
        Samurai,
        Builder,
    }
    public class DynamicUnit: Unit
    {
        protected bool _IsRanged = true;
        protected bool _IsSemiRanged = false;
        protected bool _IsInvisible = false;
        protected bool _HasTrueSight = false;
        
        protected int _MainResource;
        protected int _AttackRange = MIN_ATTACK_RANGE;

        public override void _PhysicsProcess(float delta)
        {
            if(_IsDead)
            {
                this.Hide();
                QueueFree();
            }
            switch(_State)
            {
                case State.Rest:
                    break;
                case State.GoingTo:
                    if(_PathTo != null)
                        if(_PathIndex < _PathTo.Length)
                        {
                            Vector3 MoveVec = (_PathTo[_PathIndex] - GlobalTransform.origin);
                            if(MoveVec.Length() < 0.1)
                                _PathIndex += 1;
                            else
                            {
                                MoveAndSlide(MoveVec.Normalized()*5,Vector3.Up);
                            }
                        }
                    break;
                case State.Attacking:
                    if(!_AbleToAttack)
                        break;
                    if(_CanAttackNow)
                    {
                        InteractWith(_Target);
                    }
                    break;
                default:
                    break;
            }
        }
        public override void TargetCheck()
        {
            if(_Target != null && _Target != this as Unit)
                switch(this._Target.Team)
                {
                    case Team.Enemy:
                        this._State = State.Attacking;
                        break;
                    case Team.Enemy1:
                        this._State = State.Attacking;
                        break;
                    case Team.Player:
                        this._State = State.GoingTo;
                        // this.State = State.Attacking;
                        break;
                    default:
                        break;
                }
        }
        public override void MoveTo(Vector3 Target)
        {
            this.LookAt(Target,Vector3.Up);
            if(this.RotationDegrees.x != 0)
                this.RotationDegrees = new Vector3(0,this.RotationDegrees.y,this.RotationDegrees.z);
            var DetourPath = _Navigation.Call("find_path",GlobalTransform.origin,Target) as Godot.Collections.Dictionary;
            _PathTo = DetourPath["points"] as Vector3[];
            _PathIndex = 0;
            _State = State.GoingTo;
        }
        public override void UnitEnteredTheArea(Node unit)
        {
            var UnitInArea = unit as Unit;
            if(UnitInArea.Team == Team.Enemy /*|| UnitInArea.Team == Team.Player*/)
            {
                GD.Print("Unit entered: " + unit.Name);
                _IsEnemyInRange = true;
                if(UnitInArea == this._Target && this.State == State.GoingTo)
                {
                    this.State = State.Attacking;
                    _CanAttackNow = true;
                }
                // LookAt(UnitInArea.GlobalTransform.origin, Vector3.Up);
            }
        }
        public override void UnitExitedTheArea(Node unit)
        {
            var ExitedUnit = unit as Unit;
            if(ExitedUnit.Team == Team.Enemy)
            {
                GD.Print("Unit exited: " + unit.Name);
                _IsEnemyInRange = false;
            }
        }
        public override void InteractWith(Unit unit)
        {
            if(unit.Team == Team.Enemy || unit.Team == Team.Enemy1)
            {
                if(_IsEnemyInRange && _CanAttackNow)
                {
                    while(_CanAttackNow)
                    {
                        _CanAttackNow = false;
                        LookAt(unit.GlobalTransform.origin,Vector3.Up);
                        if(this.RotationDegrees.x != 0)
                            this.RotationDegrees = new Vector3(0,this.RotationDegrees.y,this.RotationDegrees.z);
                        GD.Print(this.State);
                        PhysicalAttack(unit);
                        _Timer.Start();
                    }
                    if(unit == null)
                    {
                        this.State = State.Rest;
                        _PathTo = null;
                        _PathIndex = 0;
                    }
                }
                else
                {
                    this.State = State.GoingTo;
                    MoveTo(unit.GlobalTransform.origin);
                }
            }
        }
        virtual public void PhysicalAttack(Unit unit)
        {
                unit.HP = unit.HP - this.AttackPower;
                GD.Print(this.Name + " attack speed is: " + _AttackSpeed);
                GD.Print(unit.Name +  " HP is: " + unit.HP);
        }
    }
}