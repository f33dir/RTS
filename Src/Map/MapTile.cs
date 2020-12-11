using Godot;
using System;
using System.Collections.Generic;
namespace Map{
    public enum TileType
    {
        Empty = -1,
        Basement = 0,
        Plain = 1,
        Slope,
        Pit,
    }
    public class MapTile
    {
        public TileType Type{get;set;}
        public int Rotation;// 0-3 I suppose 
        public int Height;
        public int Color;
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
            Type = TileType.Basement;
        }
        public MapTile(TileType type)
        {
            Type = type;
        }
    }
}