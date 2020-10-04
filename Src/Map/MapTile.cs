using Godot;
using System;
namespace Map{
    public class MapTile
    {
        private TileType Type{get;set;}
        public double Rotation;// 0-3 I suppose
        public int Height;
        public int Color;

        // public override string ToString()
        // {
        //     return Type.ToString() + " " + Rotation +" " + Height +" " +Color;
        // }

        public MapTile(int type, int rotation, int height, int color)
        {
            Type = (TileType)type;
            Rotation = rotation;
            Height = height;
            Color = color;
        }
        
        public MapTile(String input)
        {
            String[] parameters= input.Split(" ");
            Type = (TileType)int.Parse(parameters[0]);
            Rotation = int.Parse(parameters[1]);
            Height = int.Parse(parameters[2]);
            Color = int.Parse(parameters[3]);
        }
        public MapTile()
        {
            
        }
    }
}