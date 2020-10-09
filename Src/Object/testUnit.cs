using Godot;
using System;

public class testUnit : KinematicBody
{
    Vector3 velocity = Vector3.Zero;
    Vector3 gravity = Vector3.Down*10;
    int speed = 5;

    public override void _PhysicsProcess(float delta)
    {   GD.Print(GlobalTransform.origin);
        getInput(delta);
        MoveAndSlideWithSnap(velocity,Vector3.Zero,Vector3.Up,true);
        // MoveAndCollide(velocity,false); какая то хуйня, а не коллизия, amen.
    }
    public void getInput(float delta)
    {
        if(Input.IsActionPressed("MoveUp"))
            velocity.z += -speed*delta;
        if(Input.IsActionPressed("MoveDown"))
            velocity.z += speed*delta;
        if(Input.IsActionPressed("MoveLeft"))
            velocity.x += -speed*delta;
        if(Input.IsActionPressed("MoveRight"))
            velocity.x += speed*delta;
    }
}
