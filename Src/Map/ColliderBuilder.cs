using Godot;
using System;
using System.Collections.Generic;
namespace Map
{
    public class ColliderBuilder : Spatial
    {  
        private MapManager mapManager;
        private MapTile[,] mapMatrix;
        public float scaleHorizontal = 1;
        public float scaleVertical = 1;
        public override void _Ready()
        {
            mapManager = GetParent<MapManager>();
        }
        public void PlaceColliderBox(int x,int y,Vector3 position)
        {
            //setup mesh
            MeshInstance mesh = new MeshInstance();
            Vector3 MeshScale = new Vector3();
            MeshScale.x = scaleHorizontal;
            MeshScale.z = scaleHorizontal;
            MeshScale.y = scaleVertical;
            mesh.Scale = MeshScale;
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
            int[,] matrix = new int[mapManager.GetLoadedMap().GetSizeX(),mapManager.GetLoadedMap().GetSizeY()];
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
            int[,] matrix = ImportMatrix(mapManager.getMatrix());
            for(int i = 0;i<matrix.GetLength(0);i++)
            {
                for(int j = 0;j<matrix.GetLength(1);j++)
                {
                    PlaceColliderBox(1,1,new Vector3(i,mapManager.getMatrix()[i,j].Height,j));
                }
            }
            mapMatrix = mapManager.getMatrix();
            placeRamps(mapMatrix);
        }
        public void FastFill()
        {  
            
        }
        public void Clear()
        {
            var children = GetChildren();
            var nullers = new Godot.Collections.Array<Node>(children);
            foreach(var child in nullers){
                child.QueueFree();
            }
        }
        public void placeRamps(MapTile[,] input)
        {
            var Ramp = ResourceLoader.Load<PackedScene>("res://Scenes/SlopeCollider.tscn");
            for(int i = 0;i<mapManager.GetLoadedMap().GetSizeX();i++){
                for(int j = 0;j<mapManager.GetLoadedMap().GetSizeY();j++){
                    if(input[i,j].Type == TileType.Slope){
                        placeRamp(i,j,input[i,j]);
                    }
                }
            }
        }
        public void placeRamp(int x, int y,MapTile tile)
        {
            var Ramp = ResourceLoader.Load<PackedScene>("res://Scenes/SlopeCollider.tscn");
            var collision =  Ramp.Instance()as Spatial;
            var  h = tile.Height;
            Vector3 position = new Vector3(x,h,y);
            collision.Translate(position*2);
            collision.Translate(new Vector3(1,0.5f,1));
            AddChild(collision);
        }
            
    }
}