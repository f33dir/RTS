using System;
using Godot;
using System.Collections.Generic;

namespace Unit
{
    public class DynamicUnit: Unit
    {
        protected const int MIN_ATTACK_RANGE = 1;
        protected const int MAX_ATTACK_RANGE = 50;

        protected bool _IsRanged = true;
        protected bool _IsSemiRanged = false;
        protected bool _IsInvisible = false;
        protected bool _HasTrueSight = false;
        
        protected int _AttackPower;
        protected int _MainResource;
        protected int _AttackRange = MIN_ATTACK_RANGE;

        public override void MoveTo(Vector3 Target)
        {
            // _Navigation = (Spatial)this.GetParent().GetNode<Spatial>("DetourNavigationMesh");
            var DetourPath = (Godot.Collections.Dictionary)(_Navigation.Call("find_path",GlobalTransform.origin,Target));
            _PathTo = (Vector3[])DetourPath["points"];
            _PathIndex = 0;
        }
        virtual public void StatSetup(){}
        virtual public void InteractWith(){}
        // public override void _Ready()
        // {
        //     SelectionRing = GetNode<MeshInstance>("Body/SelectionRing");
        // }
        public override void _PhysicsProcess(float delta)
        {
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
        }
        // public void Select()
        // {   
        //     _SelectionRing.Show();
        // }
        // public void Deselect()
        // {
        //     _SelectionRing.Hide();
        // }
    }
}