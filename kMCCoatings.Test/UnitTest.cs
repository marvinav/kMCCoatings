using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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
        public void LatticeFromJSON()
        {
            var path = @"C:\Users\av_ch\source\repos\kMCCoatings\kMCCoatings.Core\Lattice\fcc.json";
            using (var fs = new FileStream(path, FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    var fileContent = sr.ReadToEnd();
                    var result = DimerSettings.GetLatticeFromJson(fileContent);
                }
            }
        }

        [Fact]
        public void DimerFormation()
        {
            // TODO: Реализовать формирование списка возможных трансляций вектора доя поиска сайта
            Dimer dimer = new Dimer(firstAtomInDimer, secondAtomInDimer, DimerSettings);
            dimers.Add(dimer);
        }

        [Fact]
        public void IsContains()
        {
            var lattice = DimerSettings.Lattices.First();
            Assert.True(lattice.IsContains(firstAtomInDimer.ElementId, secondAtomInDimer.ElementId));
            Assert.True(lattice.IsContains(22, 22));
            Assert.True(lattice.IsContains(22, 24));
            Assert.True(lattice.IsContains(24, 22));
            Assert.True(lattice.IsContains(24, 24));
            Assert.True(lattice.IsContains(24, 7));
            Assert.True(lattice.IsContains(22, 7));
            Assert.True(lattice.IsContains(7, 22));
            Assert.True(lattice.IsContains(7, 24));
            Assert.False(lattice.IsContains(2, 45));
            // Ti - N in fcc     
        }

        /// <summary>
        /// Проверка формирования списка транслиций
        /// </summary>
        [Fact]
        public void CreateVectorFromMath()
        {

            CoordinateSystem cs = new CoordinateSystem();
            Vector3D vect = new Vector3D(2, 3, 1);
            var rotationVect = new Vector3D(1, -1, 1);
            var a = Angle.FromDegrees(355);

            var resultedVector = vect.Rotate(rotationVect, a);
            Console.WriteLine(resultedVector.ToString());
            Assert.True(false);
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
                ElementId = 7,
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
                ElementId = 22,
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

            var path = @"C:\Users\av_ch\source\repos\kMCCoatings\kMCCoatings.Core\Lattice\fcc.json";
            using (var fs = new FileStream(path, FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    var fileContent = sr.ReadToEnd();
                    DimerSettings.Lattices = DimerSettings.GetLatticeFromJson(fileContent);
                }
            }
        }
    }
}
