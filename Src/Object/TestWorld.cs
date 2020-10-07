using Godot;
using System;

public class TestWorld : Spatial
{
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("Oi, " + Name);
    }


//  public override void _Process(float delta)
//  {
//      
//  }
}
