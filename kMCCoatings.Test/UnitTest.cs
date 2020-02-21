using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Timers;
using kMCCoatings.Core;
using kMCCoatings.Core.Configuration;
using kMCCoatings.Core.Entities;
using kMCCoatings.Core.Entities.AtomRoot;
using kMCCoatings.Core.Entities.DimerRoot;
using kMCCoatings.Core.Entities.SiteRoot;
using kMCCoatings.Core.Extension;
using kMCCoatings.Core.LatticeRoot;
using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;
using Xunit;

namespace kMCCoatings.Test
{
    public class UnitTest : Test
    {
        [Fact]
        public void IsContains()
        {
            var lattice = Calculator.Settings.Di.Lattices[0];
            Assert.True(lattice.IsContains(22, 22));
            Assert.True(lattice.IsContains(22, 24));
            Assert.True(lattice.IsContains(24, 22));
            Assert.True(lattice.IsContains(24, 24));
            Assert.True(lattice.IsContains(24, 7));
            Assert.True(lattice.IsContains(22, 7));
            Assert.True(lattice.IsContains(7, 22));
            Assert.True(lattice.IsContains(7, 24));
            Assert.False(lattice.IsContains(2, 45));
            Assert.False(lattice.IsContains(7, 7));
        }

        [Fact]
        public void LoadJsonSettings()
        {
            var calc = CalculatorFabric.CreateCalculator("C:\\Users\\av_ch\\source\\repos\\kmcCoatings\\kmcCoatings.Test\\settings.json");
            Assert.True(calc != null);
            Assert.True(calc.Settings.Dimer.Lattices.Length > 0);
            Assert.True(calc.Settings.RndSeed == 1);
        }

        [Fact]
        public void Deposit()
        {
            var depositedAtoms = new List<Site>();
            for (int i = 0; i < Calculator.Settings.Deposition.ConcentrationFlow.Length; i++)
            {
                Calculator.Deposit();
            }
            var occupiedCells = Calculator.SiteService.SitesByCells.
                Where(x => x.Value.Count > 0).
                Select(x => x.Value).
                Where(x => x.Any(y => y.SiteStatus == SiteStatus.Occupied));
            foreach (var sites in occupiedCells)
            {
                depositedAtoms.AddRange(sites.Where(x => x.SiteStatus == SiteStatus.Occupied));
            }
            // foreach (var flow in Calculator.Settings.Deposition.ConcentrationFlow)
            // {
            //     Assert.True(depositedAtoms.Count(x => x.OccupiedAtom.Element.Id == flow.ElementId) == flow.Concentration);
            // }
            foreach (var site in Calculator.Atoms.Select(x => x.Site.Coordinates))
            {
                var count = 0;
                foreach (var s in Calculator.Atoms.Select(x => x.Site.Coordinates))
                {
                    if (s.X == site.X && site.Y == s.Y && site.Z == s.Z)
                    {
                        count++;
                    }
                    if (count > 1)
                    {
                        var a = s;
                    }
                }
            }
        }
    }

    public class Test
    {
        public Calculator Calculator { get; set; }
        public static List<Dimer> dimers = new List<Dimer>();
        private const string settingsPath = @"C:\Users\av_ch\source\repos\kMCCoatings\kMCCoatings.Test\settings.json";

        public Test()
        {
            Calculator = CalculatorFabric.CreateCalculator(settingsPath);
        }
    }
}