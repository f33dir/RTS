using Godot;
using System;
//не юзать, для этого юзай CameraBase.cs!
public class EditorCamera : Godot.Camera
{
    class CameraMove
    {
        public float _mouseMargin = 30;
        public float _XSpeed;
        public float _YSpeed;
        public float _MaxXSpeed = 1;
        public float _MaxYSpeed = 1;
        public float _acceleration = 0.5f;
        public Vector3 Calculate(Viewport input)
        {
            Vector2 mousePos = input.GetMousePosition();

            if(mousePos.x<_mouseMargin)
                _XSpeed+=_acceleration;
            else if(mousePos.x>  input.Size.x -_mouseMargin)
                _XSpeed-=_acceleration;
            else
                _XSpeed=0;
            if(mousePos.y<_mouseMargin)
                _YSpeed+=_acceleration;
            else if(mousePos.y>  input.Size.y -_mouseMargin)
                _YSpeed-=_acceleration;
            else
                _YSpeed = 0;
            
                // if(_XSpeed > 0){_XSpeed-=_acceleration;}
                // else if (_XSpeed< 0){_XSpeed+=_acceleration;};

                // if(_YSpeed > 0){_YSpeed-=_acceleration/2;}
                // else if (_YSpeed< 0){_YSpeed+=_acceleration/2;};
            _XSpeed=Math.Min(Math.Max(_XSpeed,-_MaxXSpeed),_MaxXSpeed);
            _YSpeed=Math.Min(Math.Max(_YSpeed,-_MaxYSpeed),_MaxYSpeed);
            Vector3 moveVector3 = new Vector3();
            moveVector3.x = -_XSpeed; // - для нормального передвижения
            moveVector3.z = -_YSpeed; // без него камера инвертирована передвигается
            return moveVector3.Normalized();
        }
    };
    private const float RAY_LENGTH = 10000;

    private CameraMove _cameraMove = new CameraMove();
    private Godot.Camera ViewCam;

    public override void _Ready()
    {
        //CurrentMap = GetTree().CurrentScene.GetNode<Map.MapManager>("MapManager");
        ViewCam = GetNode<Godot.Camera>("Camera");
        Input.SetMouseMode(Input.MouseMode.Confined);
    }
    public Godot.Collections.Dictionary RaycastFromMouse(Vector2 mPosition, uint CollisionMask)
    {
        var RayStart = ViewCam.ProjectRayOrigin(mPosition);
        var RayEnd = RayStart + ViewCam.ProjectRayNormal(mPosition) * RAY_LENGTH;
        var SpaceState = GetWorld().DirectSpaceState;
        return SpaceState.IntersectRay(RayStart,RayEnd,null,CollisionMask);
    }
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("action_command"))
        {
            var result = RaycastFromMouse(GetViewport().GetMousePosition(),1);
            if(result != null)
                GetTree().CallGroup("Objects","moveTo",result["position"]);
        }   
        Move();
    }
    public void Move()
    {
        this.Translate(_cameraMove.Calculate(GetViewport()));
    }
}