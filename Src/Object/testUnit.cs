using Godot;
using System;

public class testUnit : KinematicBody
{
     private const float MOVE_SPEED = 10;

    private Spatial nav;
    private Vector3[] Path;
    private int PathIndex = 0;
    // private int team = 0;
    // private Godot.Collections.Array<Resource> teamColors;

    // public override void _Ready()
    // {
    //     teamColors.Add(ResourceLoader.Load("res://TempRes/FriendColor.tres"));
    //     teamColors.Add(ResourceLoader.Load("res://TempRes/EnemyColor.tres"));
    // }
    private void MoveTo(Vector3 endPos)
    {
        nav.GetParent();
        Path = (Vector3[])nav.Call("find_path",GlobalTransform.origin,endPos);
        PathIndex = 0;
    }
    public override void _PhysicsProcess(float delta)
    {
        // Vector3 moveVec = Path[PathIndex] - GlobalTransform.origin;
        // if(moveVec.Length() < 0.1)
        //     PathIndex += 1;
        // else
        //     MoveAndSlide(moveVec.Normalized()*MOVE_SPEED, Vector3.Up);
    }
}
