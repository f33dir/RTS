using Godot;
using CameraBase;
using System.Collections.Generic;

namespace Unit
{
    public enum Team // Team tag
    {
        Empty = -1,
        Player = 0,
        Enemy,
        Enemy1,
    }
    public enum State // What unit doing at the moment
    {
        Rest = 0,
        GoingTo = 1,
        Attacking,
        Under_Attack,
        Casting,
        Building,
    }
    // public enum BasicBehavior
    // {
    //     Waiting = 0,
    //     Produce,
    //     Build,
    //     Attack,
    //     MoveTo,
    // }
    public enum ComplexState // State that contain multiple different states
    {
        Patrol,
        Follow,
        Hold,
        Hide,
        Seek,
    }
    public abstract class Unit : KinematicBody
    {
        protected const float MIN_MOVE_SPEED = 1f;
        protected const float MAX_MOVE_SPEED = 50f;
        protected const int MIN_ATTACK_RANGE = 1;
        protected const int MAX_ATTACK_RANGE = 50;
        protected const int MAX_ATTACK_POWER = 1000;
        protected const int MIN_ATTACK_POWER = 1;
        // Unit specific parameters
        protected int _Cost;
        protected int _HP;
        protected int _Protection;
        protected int _AttackPower;
        protected int _MoveSpeed;
        protected float _AttackSpeed = 1;
        protected Team _Team;
        protected Unit _Target = null;
        protected State _State = State.Rest;
        // General flags
        protected bool _CanAttackNow = true;
        protected bool _AbleToAttack = true;
        protected bool _IsInvulnerable = false;
        protected bool _IsSelectable = true;
        protected bool _IsEnemyInRange = false;
        protected bool _IsDead = false;
        //protected Queue<ComplexBehavior> _ComplexCommandOrder;
        // Godot nodes and it's dependencies that Unit MUST have
        protected Spatial _Navigation;
        protected Vector3[] _PathTo;
        protected int _PathIndex = 0;
        protected MeshInstance _SelectionRing;
        protected Godot.Timer _Timer;

        public override void _Ready()
        {
            _Navigation = GetParent().GetNode<Spatial>("DetourNavigationMesh");
            _SelectionRing = GetNode<MeshInstance>("SelectionRing");
            _Timer = GetNode<Timer>("AttackTimer");
            this.StatSetup();
            _Timer.OneShot = false;
            _Timer.WaitTime = _AttackSpeed;
        }
        public int HP
        {
            get { return _HP; }
            set
            {
                if(value <= 0 && _HP > 0 && !_IsInvulnerable)
                {
                    _IsDead = true;
                    this.QueueFree();
                    return;
                }
                _HP = value;
            }
        }
        public Unit Target
        {
            get{ return _Target;}
            set
            {
                if(value != null)
                    _Target = value;
            }
        }
        public int AttackPower
        {
            get{ return _AttackPower; }
            set
            {
                if(value > MIN_ATTACK_POWER && value < MAX_ATTACK_POWER)
                    _AttackPower = value;
                else _AttackPower = MIN_ATTACK_POWER;
            }
        }
        public int Protection
        {
            get{ return _Protection; }
            set{ _Protection = value; }
        }
        public int Cost
        {
            get{ return _Cost; }
        }
        public Team Team
        {
            get{ return _Team; }
        }
        public State State
        {
            get{ return _State; }
            set{ _State = value; }
        }
        public void OnTimeoutComplete()
        {
            _CanAttackNow = true;
        }
        public void Select()
        {
            _SelectionRing.Show();
        }
        public void Deselect()
        {
            _SelectionRing.Hide();
        }
        virtual public void TargetCheck(){}
        virtual public void StatSetup(){}
        virtual public void MoveTo(Vector3 target){}
        virtual public void UnitEnteredTheArea(Node Unit){}
        virtual public void UnitExitedTheArea(Node Unit){}
        virtual public void InteractWith(Unit unit){}
    }
}