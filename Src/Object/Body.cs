using Godot;
using System;

public class Body : MeshInstance
{
    Vector3 direction = Vector3.Zero;
    float speed = 4;

    public override void _Ready(){
        GD.Print("Oi, " + Name);
    }
     public override void _Process(float delta){
         
         float vertical = Input.GetActionStrength("MoveDown") - Input.GetActionStrength("MoveUp");
         float horizontal = Input.GetActionStrength("MoveLeft") - Input.GetActionStrength("MoveRight");

        direction = new Vector3(vertical,0,horizontal).Normalized();

         Transform t = GlobalTransform;
        //  KinematicBody.MoveAndCollide(direction,true,true,true);
         t.origin += direction*delta*speed;
         GlobalTransform = t;
    }
}
