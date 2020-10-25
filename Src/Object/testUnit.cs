using Godot;
using System;
using CameraBase;

namespace Unit
{
    public class testUnit : KinematicBody
    {
        // nav = (Spatial)this.GetParent().GetNode<Spatial>("DetourNavigation").GetNode<Spatial>("DetourNavigationMesh");
        // DetourPath = (Godot.Collections.Dictionary)(nav.Call("find_path",GlobalTransform.origin,endPos));
        // Path = DetourPath["points"] as Godot.Vector3[];
        private const float MOVE_SPEED = 5;

        private Spatial nav;
        private Vector3[] Path;
        private Godot.Collections.Dictionary DetourPath;
        private int PathIndex = 0;
        private CameraBase.CameraBase camera;
        private MeshInstance SelectionRing;

        public void MoveTo(Vector3 endPos)
        {
            nav = (Spatial)this.GetParent().GetNode<Spatial>("DetourNavigationMesh");
            DetourPath = (Godot.Collections.Dictionary)(nav.Call("find_path",GlobalTransform.origin,endPos));
            Path = (Vector3[])DetourPath["points"];
            PathIndex = 0;

            var LookAtPos = endPos;
            Vector3 ObjectPos = GlobalTransform.origin;
            LookAtPos = ObjectPos - LookAtPos;
            var angle = new Vector2(LookAtPos.x,LookAtPos.z).AngleTo(new Vector2(ObjectPos.x,ObjectPos.z));
            this.Rotate(Vector3.Up,angle);
        }
        public override void _Ready()
        {
            camera = GetParent().GetNode<CameraBase.CameraBase>("CameraBase");
            SelectionRing = GetNode<MeshInstance>("Body/SelectionRing");
        }
        public override void _PhysicsProcess(float delta)
        {
            if(Path != null)
                if(PathIndex < Path.Length)
                {
                    var MoveVec = (Path[PathIndex] - GlobalTransform.origin); //(Vector3)
                    if(MoveVec.Length() < 0.1)
                        PathIndex += 1;
                    else
                    {
                        // var lookatvec = new Vector3(MoveVec);
                        // LookAt(MoveVec.Normalized()*delta,Vector3.Up);
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
