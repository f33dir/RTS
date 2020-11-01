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
        private const float MoveSpeed = 5;

        private Spatial _nav;
        private Vector3[] _path;
        private Godot.Collections.Dictionary _detourPath;
        private int _pathIndex = 0;
        private CameraBase.CameraBase _camera;
        private MeshInstance _selectionRing;

        public void MoveTo(Vector3 endPos)
        {
            _nav = (Spatial)this.GetParent().GetNode<Spatial>("DetourNavigationMesh");
            _detourPath = (Godot.Collections.Dictionary)(_nav.Call("find_path",GlobalTransform.origin,endPos));
            _path = _detourPath["points"]as Vector3[];
            _pathIndex = 0;

            var lookAtPos = endPos;
            Vector3 objectPos = GlobalTransform.origin;
            lookAtPos = objectPos - lookAtPos;
            var angle = new Vector2(lookAtPos.x,lookAtPos.z).AngleTo(new Vector2(objectPos.x,objectPos.z));
            this.Rotate(Vector3.Up,angle);
        }
        public override void _Ready()
        {
            _camera = GetParent().GetNode<CameraBase.CameraBase>("CameraBase");
            _selectionRing = GetNode<MeshInstance>("Body/SelectionRing");
        }
        public override void _PhysicsProcess(float delta)
        {
            if(_path != null)
                if(_pathIndex < _path.Length)
                {
                    var moveVec = (_path[_pathIndex] - GlobalTransform.origin); //(Vector3)
                    if(moveVec.Length() < 0.1)
                        _pathIndex += 1;
                    else
                    {
                        // var lookatvec = new Vector3(MoveVec);
                        // LookAt(MoveVec.Normalized()*delta,Vector3.Up);
                        MoveAndSlide(moveVec.Normalized()*MoveSpeed,Vector3.Up);
                    }
                }
        }
        public void Select()
        {   
            _selectionRing.Show();
        }
        public void Deselect()
        {
            _selectionRing.Hide();
        }
    }
}
