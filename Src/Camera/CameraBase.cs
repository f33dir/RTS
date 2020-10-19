using Godot;
using System;

public class CameraBase : Spatial
{
    private const float MOVE_MARGIN = 30;
    private const float MOVE_SPEED = 30;
    private const float RAY_LENGTH = 10000;

    private Godot.Camera Cam;
    //private int team = 0;
    private Control SelectionBox;

    public override void _Process(float delta)
    {
        Vector2 mousePos = GetViewport().GetMousePosition();
        CalculateMove(mousePos,delta);
        if(Input.IsActionJustPressed("action_command"))
            MoveAllUnits(mousePos);
    }
    public override void _Ready()
    {
        Cam = GetNode<Godot.Camera>("Camera");
        SelectionBox = GetNode<Control>("SelectionBox");
        Input.SetMouseMode(Input.MouseMode.Confined);
    }
    public void CalculateMove(Vector2 mousePos, float delta) // перемещение камеры
    {
        Vector2 vecSize = GetViewport().Size;
        Vector3 moveVec = Vector3.Zero;

        if(mousePos.x < MOVE_MARGIN)
            moveVec.x -= 1;
        if(mousePos.y < MOVE_MARGIN)
            moveVec.z -= 1;
        if(mousePos.x > vecSize.x - MOVE_MARGIN)
            moveVec.x += 1;
        if(mousePos.y > vecSize.y - MOVE_MARGIN)
            moveVec.z += 1;
        
        moveVec = moveVec.Rotated(Vector3.Up,RotationDegrees.y);
        GlobalTranslate(moveVec*delta*MOVE_SPEED);
    }
    // public object GetUnitUnderMouse(Vector2 mousePos)
    // {
    //     var result = RaycastFromMousePosition(mousePos,3);
    //     if(result != null && result["collider"] != null && result["collider"].team == team)
    //         return result["collider"];
    // }
    public void MoveAllUnits(Vector2 mousePos)
    {
        var result = RaycastFromMousePosition(mousePos,1);
        //GD.Print(result["position"]);
        if(result != null && result.Count != 0)
            GetParent().GetTree().CallGroup("Objects","MoveTo",result["position"]);
    }
    public Godot.Collections.Dictionary RaycastFromMousePosition(Vector2 mousePos, uint CollisionMask) // определение положения курсора на карте
    {
        Vector3 RayStart = Cam.ProjectRayOrigin(mousePos);
        Vector3 RayEnd = RayStart + Cam.ProjectRayNormal(mousePos) * RAY_LENGTH;
        PhysicsDirectSpaceState SpaceState = GetWorld().DirectSpaceState;
        return SpaceState.IntersectRay(RayStart,RayEnd,new Godot.Collections.Array(),CollisionMask);
    }
}
