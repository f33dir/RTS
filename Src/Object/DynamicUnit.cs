using System;
using Godot;
using System.Collections.Generic;

namespace Unit
{
    public class DynamicUnit: Unit
    {
        protected const float MIN_ATTACK_RANGE = 1f;
        protected const float MAX_ATTACK_RANGE = 50f;

        protected bool _IsRanged = true;
        protected int _MainResource;
        protected bool _IsInvisible = false;

        public override void MoveTo(Vector3 Target)
        {
            nav = (Spatial)this.GetParent().GetNode<Spatial>("DetourNavigationMesh");
            var DetourPath = (Godot.Collections.Dictionary)(nav.Call("find_path",GlobalTransform.origin,endPos));
            Path = (Vector3[])DetourPath["points"];
            PathIndex = 0;
        }
        public void InteractWith(){}
        public override void _Ready()
        {
            SelectionRing = GetNode<MeshInstance>("Body/SelectionRing");
        }
        public override void _PhysicsProcess(float delta)
        {
            if(Path != null)
                if(PathIndex < Path.Length)
                {
                    Vector3 MoveVec = (Path[PathIndex] - GlobalTransform.origin);
                    if(MoveVec.Length() < 0.1)
                        PathIndex += 1;
                    else
                    {
                        MoveAndSlide(MoveVec.Normalized()*MOVE_SPEED,Vector3.Up);
                    }
                }
        }
        public void Select()
        {   
            SelectionRing.Show();
        }
        public void Deselect()
        {
            SelectionRing.Hide();
        }
    }
}