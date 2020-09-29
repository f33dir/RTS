using Godot;
using System;
namespace Map{
    enum TileType
    {
        Slain,
        Slope,
        LeftSlope,
        RightSlope,
        Pit
    }   
    class Map
    {
        //fields
        private MapTile[,] Matrix;
        private int SizeX;
        private int SizeY;
        //methods
        public void SetTile(MapTile inputTile,int x,int y){
            
        }
        public void PrintMap(){
            for(int i = 0;i<size;i++)
        }
    }
}