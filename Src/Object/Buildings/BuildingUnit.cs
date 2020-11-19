using Godot;
using System;

namespace Unit
{
    public class BuildingUnit : Unit
    {
        protected bool _IsMovable = false; // для юнитов вроде Ancient'ов из Warcraft III (night elves)
        public uint _GridSizeX{get;set;} = 1;
        public uint _GridSizeY{get;set;} = 1;
        public override void _Ready()
        {
            
        }
        public void Build(){}
        public void Upgrade(){}
    }
}