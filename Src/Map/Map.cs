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
        public Vector2 BasePos;
        public Vector2 StartPos;
        private String _mapName;
        public int _sizeX;
        public int _sizeY;
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
        public MapTile GetTile(Vector2 position)
        {
            return Matrix[(int)position.x,(int)position.y];
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
        public Map(int sizex,int sizey,TileType tileType)
        {
            _sizeX = sizex;
            _sizeY = sizey;
            MapTile tile = new MapTile(TileType.Plain);
            Matrix = new MapTile[_sizeX,_sizeY];
            for(int i = 0;i<_sizeX;i++)
            {
                for(int j = 0;j<_sizeY;j++)
                {
                    Matrix[i,j] = tile;
                }
            }
        }
    }
}