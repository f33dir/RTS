using Godot;
using System;

public class Portal : Unit.BuildingUnit
{
    private PackedScene Bug;
    private PackedScene Tank;
    private PackedScene ATAT;
    public override void _Ready()
    {
        Bug = ResourceLoader.Load<PackedScene>("res://Scenes/Units/Bug.tscn");
        // ATAT = ResourceLoader.Load<PackedScene>("res://Src/Object/Units/AT_AT.cs");
        // Tank = ResourceLoader.Load<PackedScene>("res://Src/Object/Units/Tank.cs");
    }
    public void SpawnWave()
    {

    }
    public void SpawnBug(double speed,double size, double damage, double hp)
    {

    }
}
