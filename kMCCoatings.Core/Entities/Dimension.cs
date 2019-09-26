using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kMCCoatings.Core.Entities
{
    public class Dimension
    {
        public int Lx { get; set; }

        public int Ly { get; set; }

        public int Lz { get; set; }

        public Cell.Cell[,,] Cells { get; set; }

        public Dimension(int lx, int ly, int lz)
        {
            Lx = lx;
            Ly = ly;
            Lz = lz;
            Cells = new Cell.Cell[Lx, Ly, Lz];
            GenerateCells();
        }

        private void GenerateCells()
        {
            for (int x = 0; x < Lx; x++)
            {
                for (int y = 0; y < Ly; y++)
                {
                    for (int z = 0; z < Lz; z++)
                    {
                        Cells[x, y, z] = new Cell.Cell()
                        {
                            Neighbors = NeigboreCellsCoordinates(x, y, z)
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Рассчитать соседние клетки для указанной клетки
        /// </summary>
        private List<GlobalCoordinates> NeigboreCellsCoordinates(int xCell, int yCell, int zCell)
        {
            var neigbors = new List<GlobalCoordinates>();
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    for (int z = -1; z < 2; z++)
                    {
                        if(!(x == 0 && y == 0 && z == 0))
                        {
                            neigbors.Add(new GlobalCoordinates(x + xCell, y + yCell, z + zCell, Lx, Ly));
                        }
                    }
                }
            }
            return neigbors;
        }
    }
}
