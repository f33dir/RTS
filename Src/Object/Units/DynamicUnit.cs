using System;
using Godot;
using System.Collections.Generic;

namespace Unit
{
    public class DynamicUnit: KinematicBody
    {
        //Godot nodes
        protected AnimationPlayer _Animation;
        protected Area _Area;
        protected Spatial _Navigation;
        protected KinematicBody _Target;
        protected HealthBar _HPBar;
        protected Player.Player _Player;
        protected Team _Team;
        protected Position3D _Position;
        //Pathfinding
        protected Vector3[] _PathTo;
        protected uint _PathIndex;
        //Enemy parameters
        protected int _HP;
        protected int _Protection;
        protected float _MoveSpeed;
        protected int _Damage;
        
        public override void _Ready()
        {
            _Navigation = GetParent().GetNode<Spatial>("DetourNavigationMesh");
            _Animation = GetNode<AnimationPlayer>("AnimationPlayer");
            _Area = GetNode<Area>("InteractionArea");
            _Target = GetParent().GetNode<KinematicBody>("DefenseLocation");
            _HPBar = GetNode<HealthBar>("HPBar");
            _Player = GetParent().GetParent().GetNode<Player.Player>("Player");
            _Position = GetNode<Position3D>("Position3D");
            _PathIndex = 0;
            StatSetup();
        }
        // public Vector3 Position
        // {
        //     get { return _Position.GlobalTransform.origin;}
        // }
        public int HP
        {
            get { return _HP; }
            set
            {
                if(value > _HPBar.MaxValue)
                    _HPBar.MaxValue = value;
                _HP = value;
                _HPBar.UpdateBar(_HP);
            }
        }
        public Team Team
        {
            get { return _Team;}
        }
        public float MoveSpeed
        {
            get { return _MoveSpeed;}
            set
            {
                if (value > 0)
                    _MoveSpeed = value;
            }
        }
        public override void _PhysicsProcess(float delta)
        {
            if(HP <= 0)
            {
                this.Hide();
                QueueFree();
            }
            if(_Player.Start)
                if(_Animation != null)
                {
                    _Animation.Play("Walk");
                    if(_PathTo != null)
                        if(_PathIndex < _PathTo.Length)
                        {
                            Vector3 MoveVec = (_PathTo[_PathIndex] - GlobalTransform.origin);
                            if(MoveVec.Length() < 0.1)
                                _PathIndex += 1;
                            else
                            {
                                MoveAndSlideWithSnap(MoveVec.Normalized()*_MoveSpeed,Vector3.Zero,Vector3.Up);
                            }
                        }
                        else
                        {
                            _PathTo = null;
                            _PathIndex = 0;
                        }
                }
        }
        public void MoveTo()
        {
            LookAt(_Target.GlobalTransform.origin,Vector3.Up);
            if(RotationDegrees.x != 0)
                RotationDegrees = new Vector3(0,RotationDegrees.y,RotationDegrees.z);
            var DetourPath = _Navigation.Call("find_path",GlobalTransform.origin,_Target.GlobalTransform.origin) as Godot.Collections.Dictionary;
            _PathTo = DetourPath["points"] as Vector3[];
            _PathIndex = 0;
        }
        public void _on_InteractionArea_body_entered(Node body)
        {
            KinematicBody Target = body as KinematicBody;
            if(Target == _Target)
            {
                _Player.Lives -= _Damage;
                QueueFree();
            }
        }
        virtual public void StatSetup()
        {
            _HP = 100;
            _Protection = 1;
            _MoveSpeed = 5f;
        }    
    }
}