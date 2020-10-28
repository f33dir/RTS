using Godot;
using System;

namespace Unit
{
    public class BuildingUnit : Unit
    {
        protected bool _IsMovable = false; // для юнитов вроде Ancient'ов из Warcraft III (night elves)
        protected uint _GridSize;

        public void Build(){}
        public void Upgrade(){}
    }
}