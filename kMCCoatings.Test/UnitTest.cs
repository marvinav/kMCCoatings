using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using kMCCoatings.Core.Configuration;
using kMCCoatings.Core.Constants;
using kMCCoatings.Core.Entities;
using kMCCoatings.Core.Entities.AtomRoot;
using kMCCoatings.Core.Entities.DimerRoot;
using kMCCoatings.Core.Entities.SiteRoot;
using Xunit;

namespace kMCCoatings.Test
{
    public class UnitTest : Test
    {
        [Fact]
        public void FirstTest()
        {
            Assert.True(true);
        }
        
        [Fact]
        public void DimerFormation()
        {
            // TODO: Реализовать формирование списка возможных трансляций вектора доя поиска сайта
            Dimer dimer = new Dimer(firstAtomInDimer, secondAtomInDimer, DimerSettings);
            dimers.Add(dimer);
        }

        [Fact]
        public void CalculateSiteForDimer()
        {
            //TODO: Реализовать поиск возможных сайтов для диммера 
        }
    }

    public class Test
    {
        public DimerSettings DimerSettings = new DimerSettings();
        public static Atom firstAtomInDimer;
        public static Atom secondAtomInDimer;
        public static List<Dimer> dimers = new List<Dimer>();

        public Test()
        {
            firstAtomInDimer = new Atom()
            {
                AtomTypeId = 1,
                Site = new Site()
                {
                    Coordinates = new GlobalCoordinates()
                    {
                        X = 0,
                        Y = 0,
                        Z = 0
                    }
                }
            };
            secondAtomInDimer = new Atom()
            {
                AtomTypeId = 2,
                Site = new Site()
                {
                    Coordinates = new GlobalCoordinates()
                    {
                        X = 1,
                        Y = 1,
                        Z = 0
                    }
                }
            };

            /// Кристаллическая решётка fcc для теста
            var crd = new Dictionary<(int, int), Vector3>()
            {
                {(0,0), new Vector3(0, 0 ,0)},
                {(0,1), new Vector3(0.5f, 0.5f, 0.5f)}
            };
            
            var lattice = new List<Lattice>
            {
                new Lattice("meN", new int[] { 0, 1 }, crd, new Vector3(1, 0, 0))
            };
            
            /// 0 - Ti, 1 - Cr, 2 - N
            lattice.First().AddElementsToLattice(0, 0);
            lattice.First().AddElementsToLattice(1, 0);
            lattice.First().AddElementsToLattice(2, 1);

            DimerSettings.Lattices = lattice;
        }
    
    }
}
