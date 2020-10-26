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
    public class MapStaticObject{
        public string Name;
        public Transform Transform;
    }

    public class Map 
    {
        private String _mapName;
        private int _sizeX;
        private int _sizeY;
        public List<MapStaticObject>  _staticObjects;
        public MapTile[,] Matrix{get;set;}
        //methods
        public void SetTile(MapTile inputTile,int x,int y)
        {
            Matrix[x,y] = inputTile;
        }

        public int GetSizeX(){
            return _sizeX;
        }

        public int GetSizeY(){
            return _sizeY;
        }

        public void PrintMap()
        {
            for(int i = 0;i<_sizeY;i++){
                for(int j = 0;j<_sizeX;j++){
                    GD.Print(Matrix.ToString());
                }
            }
        }

        public Map()
        {
            _sizeX = 2;
            _sizeY = 2;
            Matrix = new MapTile[_sizeX,_sizeY];
        }

        public Map(int sizex,int sizey)
        {
            _sizeX = sizex;
            _sizeY = sizey;
            Matrix = new MapTile[_sizeX,_sizeY];
        }
    }
}