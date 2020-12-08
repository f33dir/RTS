using Godot;
using CameraBase;
using System.Collections.Generic;

namespace Unit
{
    public enum Team // Team tag
    {
        Empty = -1,
        Player = 0,
        Enemy,
        Enemy1,
        Neutral
    }
    public enum State // What unit doing at the moment
    {
        Rest = 0,
        GoingTo = 1,
        Attacking,
        UnderAttack,
        Casting,
        Building,
        AttackOnSight,
    }
    public abstract class   Unit : KinematicBody
    {
        // Unit specific parameters
        protected Player.Player _Player;
        //Setup
        public override void _Ready()
        {
            _Player = GetParent().GetParent().GetNode<Player.Player>("Player");
        }
    }
}