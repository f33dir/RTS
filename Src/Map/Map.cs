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
    /*
    .--------X
    | . . . . 
    | . . . .
    | . . . .
    Y
    
    */
    class Map
    {
        //fields
        private MapTile[,] Matrix;
        private int SizeX;
        private int SizeY;
        //methods
        public void SetTile(MapTile inputTile,int x,int y){
            Matrix[x,y] = inputTile;
        }
        public void PrintMap(){
            for(int i = 0;i<SizeY;i++){
                for(int j = 0;j<SizeX;j++){
                    GD.Print(Matrix.ToString());
                }
            }
        }
    }
}