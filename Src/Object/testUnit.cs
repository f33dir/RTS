using Godot;
using System;

public class testUnit : KinematicBody
{
     private const float MOVE_SPEED = 5;

    private Spatial nav;
    private Vector3[] Path;
    private Godot.Collections.Dictionary DetourPath;
    private int PathIndex = 0;
    
    private void MoveTo(Vector3 endPos)
    {
        nav = (Spatial)this.GetParent().GetNode<Spatial>("DetourNavigation").GetNode<Spatial>("DetourNavigationMesh");
        DetourPath = (Godot.Collections.Dictionary)(nav.Call("find_path",GlobalTransform.origin,endPos));
        Path = (Vector3[])DetourPath["points"];

        PathIndex = 0;
    }
    public override void _Ready()
    {
        
    }
    public override void _PhysicsProcess(float delta)
    {
        if(Path != null)
            if(PathIndex < Path.Length)
            {
                var MoveVec = ((Vector3)Path[PathIndex] - GlobalTransform.origin);
                if(MoveVec.Length() < 0.1)
                    PathIndex += 1;
                else
                {
                    LookAt(MoveVec.Normalized(),Vector3.Up);
                    MoveAndSlide(MoveVec.Normalized()*MOVE_SPEED,Vector3.Up);
                }
            }
    }
}
