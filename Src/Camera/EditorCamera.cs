using Godot;
using System;
namespace Camera
{
    public class EditorCamera : KinematicBody
    {
        class CameraMove
        {
            public float _mouseMargin = 50;
            public float _XSpeed;
            public float _YSpeed;
            public float _MaxXSpeed = 40;
            public float _MaxYSpeed = 40;
            public float _acceleration = 5;
            public Vector3 Calculate(Viewport input)
            {
                Vector2 mousePos = input.GetMousePosition();
                if(mousePos.x<_mouseMargin)
                {
                    _XSpeed+=_acceleration;
                }
                else if(mousePos.x>  input.Size.x -_mouseMargin)
                {
                    _XSpeed-=_acceleration;
                }
                else _XSpeed = 0;
                if(mousePos.y<_mouseMargin)
                {
                    _YSpeed+=_acceleration;
                }
                else if(mousePos.y>  input.Size.y -_mouseMargin)
                {
                  _YSpeed-=_acceleration;
                }
                else _YSpeed = 0;
                Vector3 moveVector3 = new Vector3();
                moveVector3.x = _XSpeed;
                moveVector3.z = _YSpeed;
                return moveVector3;
            }
        };
        private CameraMove _cameraMove = new CameraMove();
        private Map.MapManager CurrentMap;
        private Godot.Camera ViewCam;
        public override void _Ready()
        {
            CurrentMap = GetTree().CurrentScene.GetNode<Map.MapManager>("MapManager");
            ViewCam = GetNode<Godot.Camera>("Camera");
            
        }

        public override void _PhysicsProcess(float delta)
        {
            Move();
        }
        public void Move()
        {
            MoveAndSlide(_cameraMove.Calculate(GetViewport()),new Vector3(0,1,0));
        }
    }
}
