using Godot;
using System;
using System.Collections.Generic;
namespace Map{
    enum TileType
    {
        Plain = 0,
        Slope,
        LeftSlope,
        RightSlope,
        Pit,
        Edge,
        EdgeCorner,
        EdgeInsideCorner,
        PitEdge,
        PitEdgeCorner,
        PitEdgeInsideCorner,
    }
    /*
    .--------X
    | . . . . 
    | . . . .
    | . . . .
    Y
    */
    class Map 
    {
        String MapName;
        List<string> GridMapPaths;
        private int SizeX;
        private int SizeY;
        public MapTile[,] Matrix{get;set;}
        //methods
        public void SetTile(MapTile inputTile,int x,int y)
        {
            Matrix[x,y] = inputTile;
        }

        public void AddGridmap(int index)
        {

        }

        public void PrintMap()
        {
            for(int i = 0;i<SizeY;i++){
                for(int j = 0;j<SizeX;j++){
                    GD.Print(Matrix.ToString());
                }
            }
        }

        public Map()
        {
            SizeX = 10;
            SizeY = 10;
            Matrix = new MapTile[SizeX,SizeY];
        }

        public Map(int Sizex,int Sizey)
        {
            SizeX = Sizex;
            SizeY = Sizey;
            Matrix = new MapTile[SizeX,SizeY];
        }
    }
}