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
    class Map : Node
    {
        //fields
        private int SizeX;
        private int SizeY;
        private MapTile[][] Matrix;
        //methods
        public void SetTile(MapTile inputTile,int x,int y){
            Matrix[x][y] = inputTile;
        }

        public void PrintMap(){
            for(int i = 0;i<SizeY;i++){
                for(int j = 0;j<SizeX;j++){
                    GD.Print(Matrix.ToString());
                }
            }
        }

        public override void  _Ready(){
            
        }
        public Map(){
            this.SizeX = 10;
            this.SizeY = 10;
        }
        public Map(int Sizex,int Sizey){
            this.SizeX = Sizex;
            this.SizeY = Sizey;
        }
    }
}