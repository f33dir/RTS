using Godot;
using System;

namespace Unit
{
    public class Tower : BuildingUnit
    {
        //Parameters
        protected int _LvL = 1;
        protected int _Cost;
        protected int _HP;
        protected int _AttackPower;
        protected float _AttackRange;
        protected float _AttackSpeed;
        protected Team _Team;
        protected State _State;
        protected DynamicUnit _Target;
        protected Godot.Collections.Array<DynamicUnit> _EnemiesInRange;
        //Flags
        protected bool _CanAttackNow;
        protected bool _IsEnemyInRange;
        protected bool _IsTargetInRange;
        protected bool _IsAOE;
        protected bool _IsFreezing;
        //Godot nodes
        //protected HealthBar _HPBar; 
        protected Godot.Timer _Timer;
        protected Area _Area;
        protected Spatial _Muzzle;
        protected PackedScene _Bullet;
        protected MeshInstance _Gun;
        //Methods
        public override void _Ready()
        {
            // _HPBar = GetNode<HealthBar>("HPBar");
            _Player = GetNode<Player.Player>("/root/Environment/Player");
            _Timer = GetNode<Timer>("AttackTimer");
            _Area = GetNode<Area>("InteractionArea");
            _Muzzle = GetNode<Spatial>("Body/Gun/Muzzle");
            _Bullet = GD.Load<PackedScene>("res://Scenes/Units/Bullet.tscn");
            _Gun = GetNode<MeshInstance>("Body/Gun");
            StatSetup();
        }
        public float AttackRange
        {
            get {return _AttackRange;} 
        }
        public int Cost
        {
            get{return _Cost;}
            set{_Cost = value;}
        }
        public DynamicUnit Target
        {
            get { return _Target; }
            set
            {
                if(value != null)
                    _Target = value;
            }
        }
        public int AttackPower
        {
            get { return _AttackPower; }
            set { _AttackPower = value; }
        }
        public Team Team
        {
            get { return _Team; }
        }
        public State State
        {
            get { return _State; }
            set { _State = value; }
        }
        virtual public void StatSetup()
        {
            //default init
            _HP = 100;
            _AttackPower = 50;
            _AttackRange = 5f;
            _AttackSpeed = 15f;
            _Cost = 25;
            _CanAttackNow = true;
            _Team = Team.Empty;
            _EnemiesInRange = new Godot.Collections.Array<DynamicUnit>();
            _IsEnemyInRange = false;
            _IsTargetInRange = false;
            _State = State.AttackOnSight;
            var AreaScaleVector = Vector3.One;
            AreaScaleVector *= _AttackRange/10;
            AreaScaleVector.y = 1f;
            _Area.Scale = AreaScaleVector;
            _Timer.OneShot = false;
            _Timer.WaitTime = _AttackSpeed/10;
        }
        public override void _Process(float delta)
        {
            if(_Target != null)
            {
                _Gun.LookAt(_Target.GlobalTransform.origin,Vector3.Up);
                if(_Gun.RotationDegrees.x != 0)
                    _Gun.RotationDegrees = new Vector3(0,_Gun.RotationDegrees.y - 90,_Gun.RotationDegrees.z);
            }
            switch (_State)
            {
                case State.Rest:
                    break;
                case State.AttackOnSight:
                    if(_Target != null)
                        Attack();
                    break;
                default:
                    break;
            }
        }
        public void BodyEnteredTheArea(Node Body)
        {
            GD.Print("Body entered -> " + Body.Name);
            if(_Area.Owner == Body)
                return;
            var EnteredUnit = Body as DynamicUnit;
            if(EnteredUnit == null)
                return;
            if(EnteredUnit.Team == Team.Enemy || EnteredUnit.Team == Team.Enemy1)
            {
                _EnemiesInRange.Add(EnteredUnit);
                if(_Target == null)
                {
                    _Target = EnteredUnit;
                    _Gun.LookAt(_Target.GlobalTransform.origin,Vector3.Up);
                    GD.Print("Looking at -> " + _Target.Name);
                    if(_Gun.RotationDegrees.x != 0)
                        _Gun.RotationDegrees = new Vector3(0,_Gun.RotationDegrees.y - 90,_Gun.RotationDegrees.z);
                    _IsTargetInRange = true;
                }
            }
            PrintInfo();
        }
        public void BodyExitedTheArea(Node Body)
        {
            GD.Print("Body exited -> " + Body.Name);
            var ExitedUnit = Body as DynamicUnit;
            if(ExitedUnit != null && (ExitedUnit.Team == Team.Enemy1 || ExitedUnit.Team == Team.Enemy))
            {
                _EnemiesInRange.Remove(ExitedUnit);
                if(ExitedUnit == _Target)
                {
                    _Gun.LookAt(GlobalTransform.origin,Vector3.Up);
                    GD.Print("Stopped looking at -> " + _Target.Name);
                    _Target = null;
                    if(_Gun.RotationDegrees.x != 0)
                        _Gun.RotationDegrees = new Vector3(0,_Gun.RotationDegrees.y - 90,_Gun.RotationDegrees.z);
                    _IsTargetInRange = false;
                }
                if(_Target == null && _EnemiesInRange.Count != 0)
                    _Target = _EnemiesInRange[0];
                if(_EnemiesInRange.Count == 0)
                    _IsEnemyInRange = false;
            }
            PrintInfo();
        }
        public void OnTimeoutComplete()
        {
            _CanAttackNow = true;
        }
        private void Attack()
        {
            while(_CanAttackNow)
            {
                _CanAttackNow = false;
                var bullet_scene = _Bullet.Instance();
                var bullet = bullet_scene as Bullet;
                _Muzzle.AddChild(bullet_scene);
                Vector3 TargetPosition = _Target.GlobalTransform.origin;
                TargetPosition.y += 2f;
                bullet.LookAt(TargetPosition,Vector3.Up);
                bullet.Damage = _AttackPower;
                bullet.IsAOE = _IsAOE;
                bullet.IsFreezing = _IsFreezing;
                _Timer.Start();
            }
        }
        //debug only
        public void PrintInfo()
        {
            GD.Print("Enemies in range: ");
            foreach (var item in _EnemiesInRange)
            {
                GD.Print(" -> " + item.Name);
            }
            if(_Target == null)
                GD.Print(Name + " has no target");
            else GD.Print(Name + " target -> " + _Target.Name);
        }
        public override void Upgrade()
        {
            if(_Player.Resource >= _Cost*(_LvL))
            {
                if(_LvL < 10)
                {
                    _LvL++;
                    _AttackPower += _AttackPower / 4;
                    _AttackSpeed -= _AttackSpeed / 4;
                    _Timer.WaitTime = _AttackSpeed / 10;
                    Scale *= 1.01f;
                    _Player.Resource -= _Cost*(_LvL);
                }
                if(_LvL >= 5)
                    _IsAOE = true;
                if(_LvL == 10)
                {
                    _IsFreezing = true;
                    _AttackSpeed = 1f;
                    _Timer.WaitTime = _AttackSpeed / 10;
                }
            }
        }
    }
}
