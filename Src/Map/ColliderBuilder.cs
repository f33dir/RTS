using Godot;
using System;
using System.Collections.Generic;
namespace Map
{
    public class ColliderBuilder : Spatial
    {  
        private MapManager _mapManager;
        private MapTile[,] _mapMatrix;
        private float _scaleHorizontal = 1;
        private float _scaleVertical = 0.5f;
        public override void _Ready()
        {
            _mapManager = GetParent<MapManager>();
        }
        public void PlaceColliderBox(int x,int y,Vector3 position)
        {
            //setup mesh
            MeshInstance mesh = new MeshInstance();
            Vector3 meshScale = new Vector3();
            meshScale.x = _scaleHorizontal;
            meshScale.z = _scaleHorizontal;
            meshScale.y = _scaleVertical;
            mesh.Scale = meshScale;
            StaticBody body = new StaticBody();
            //setup shape
            CollisionShape shape = new CollisionShape();
            Shape col = new BoxShape();
            shape.Shape = col;
            //transpose collider to position
            mesh.Translate(position*2);
            mesh.Translate(new Vector3(1,0.5f,1));
            this.AddChild(mesh);
            mesh.AddChild(body);
            body.AddChild(shape);
        }
        public int[,] ImportMatrix(MapTile[,] input)
        {
            int[,] matrix = new int[_mapManager.GetLoadedMap().GetSizeX(),_mapManager.GetLoadedMap().GetSizeY()];
            for(int i = 0;i<matrix.GetLength(0);i++)
            {
                for(int j = 0;j<matrix.GetLength(1);j++)
                {
                    if(input[i,j].Type!=TileType.Slope){
                        matrix[i,j] = input[i,j].Height;
                    }
                    else{
                        matrix[i,j] = input[i,j].Height-1;
                    }
                }
            }
            return matrix;
        }
        public void SlowFill()
        {
            int[,] matrix = ImportMatrix(_mapManager.GetMatrix());
            for(int i = 0;i<matrix.GetLength(0);i++)
            {
                for(int j = 0;j<matrix.GetLength(1);j++)
                {
                    PlaceColliderBox(1,1,new Vector3(i,_mapManager.GetMatrix()[i,j].Height,j));
                }
            }
            _mapMatrix = _mapManager.GetMatrix();
            PlaceRamps(_mapMatrix);
        }
        public void FastFill()
        {  
            
        }
        public void Clear()
        {
            var children = GetChildren();
            var childArray = new Godot.Collections.Array<Node>(children);
            foreach(var child in childArray){
                child.QueueFree();
            }
        }
        private void PlaceRamps(MapTile[,] input)
        {
            var ramp = ResourceLoader.Load<PackedScene>("res://Scenes/SlopeCollider.tscn");
            for(var i = 0;i<_mapManager.GetLoadedMap().GetSizeX();i++){
                for(var j = 0;j<_mapManager.GetLoadedMap().GetSizeY();j++){
                    if(input[i,j].Type == TileType.Slope){
                        PlaceRamp(i,j,input[i,j]);
                    }
                }
            }
        }
        public void PlaceRamp(int x, int y,MapTile tile)
        {
            var ramp = ResourceLoader.Load<PackedScene>("res://Scenes/SlopeCollider.tscn");
            var collision =  ramp.Instance()as Spatial;
            var  h = tile.Height;
            Vector3 position = new Vector3(x,h,y);
            collision.Translate(position*2);
            collision.Translate(new Vector3(1,0.5f,1));
            AddChild(collision);
        }
            
    }
}