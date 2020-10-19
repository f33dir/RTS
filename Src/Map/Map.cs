using Godot;
using System;
using System.Collections.Generic;
namespace Map{

    /*
    .--------X
    | . . . . 
    | . . . .
    | . . . .
    Y
    */
    class MapStaticObject{
        public string _name;
        public Transform _transform;
    } 
    class Map 
    {
        String MapName;
        private int SizeX;
        private int SizeY;
        public List<MapStaticObject>  _staticObjects;
        public MapTile[,] Matrix{get;set;}
        //methods
        public void SetTile(MapTile inputTile,int x,int y)
        {
            Matrix[x,y] = inputTile;
        }

        public int GetSizeX(){
            return SizeX;
        }

        public int GetSizeY(){
            return SizeY;
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
            SizeX = 2;
            SizeY = 2;
            Matrix = new MapTile[SizeX,SizeY];
        }

        public Map(int Sizex,int Sizey)
        {
            SizeX = Sizex;
            SizeY = Sizey;
            Matrix = new MapTile[SizeX,SizeY];
            //fill basement 
        }
    }
}