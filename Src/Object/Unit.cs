using Godot;
using System;

public class Unit : KinematicBody
{
    Vector3 velocity = Vector3.Zero;
    Vector3 gravity = Vector3.Down*10;
    int speed = 10;
    
     public override void _PhysicsProcess(float delta)
     {
        GD.Print("Unit pos: " + GlobalTransform.origin);
        velocity += gravity*delta;
        getInput(delta);
        MoveAndSlideWithSnap(velocity,Vector3.Down*2,Vector3.Up,true);
    }

    public void getInput(float delta)
    {
        float vy = velocity.y;
        velocity = Vector3.Zero;
        
        if(Input.GetActionStrength("MoveUp") == 1){
            velocity += -GlobalTransform.basis.z * speed;
            velocity.y = 0;
        }
        else if(Input.GetActionStrength("MoveDown") == 1){
            velocity += GlobalTransform.basis.z * speed;
            velocity.y = 0;
        }
        else if(Input.GetActionStrength("MoveLeft") == 1)
            velocity += -GlobalTransform.basis.x * speed;
        else if(Input.GetActionStrength("MoveRight") == 1)
            velocity += GlobalTransform.basis.x * speed;

        velocity.y = vy;
    }
}
