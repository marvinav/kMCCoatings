using System;
using System.Collections.Generic;
using kMCCoatings.Core.Configuration;
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
            Assert.True(Math.Round(dimer.Orientation.CosAlfa, 3) == -0.707 
                && Math.Round(dimer.Orientation.CosBetta,3) == -0.707 
                && Math.Round(dimer.Orientation.CosGamma, 3) == 0);
        }

        [Fact]
        public void CalculateSiteForDimer()
        {
            //TODO: Реализовать поиск возможных сайтов для диммера 
        }
    }

    public class Test
    {
        public static DimerSettings DimerSettings = new DimerSettings();
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
        }
    
    }
}
