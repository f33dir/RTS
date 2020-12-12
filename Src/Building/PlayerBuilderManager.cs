using Godot;
using System;

namespace BuildingManager
{
    class PlayerBuildingManager
    {
        public Player.Player player;
        public Unit.BuildingUnit _currentBuilding;
        public Vector2 _cursorPos;// grid position
        public bool _showSilhouette;
        public PlayerBuildingManager(ref Player.Player player)
        {
            this.player = player;
            _currentBuilding = null;
            _cursorPos = new Vector2(3,4);
            _showSilhouette = false;
        }
    }
}