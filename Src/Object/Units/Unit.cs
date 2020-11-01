using Godot;
using CameraBase;
using System.Collections.Generic;

namespace Unit
{
    public enum Team
    {
        Empty = -1,
        Player = 0,
        Enemy,
        Enemy1,
    }
    public enum BasicBehavior
    {
        Waiting = 0,
        Produce,
        Build,
        Attack,
        MoveTo,
    }
    public enum ComplexBehavior
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

        protected int _Cost;
        protected int _HP;
        protected int _Protection;
        protected bool _IsDestructable = true;
        protected bool _IsSelectable = true;
        public Team _Team;
        //protected Queue<BasicBehavior> _CommandOrder;

        protected Spatial _Navigation;
        protected Vector3[] _PathTo;
        protected int _PathIndex = 0;
        protected MeshInstance _SelectionRing;
        // protected CameraBase.CameraBase _Camera;

        public override void _Ready()
        {
            _Navigation = GetParent().GetNode<Spatial>("DetourNavigationMesh");
            _SelectionRing = GetNode<MeshInstance>("SelectionRing");
            this.StatSetup();
        }
        public void Select()
        {
            _SelectionRing.Show();
        }
        public void Deselect()
        {
            _SelectionRing.Hide();
        }
        virtual public void StatSetup(){}
        virtual public void MoveTo(Vector3 target){}
        virtual public void UnitEnteredTheArea(Node Unit){}
    }
}
