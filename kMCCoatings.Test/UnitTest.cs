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
using kMCCoatings.Core.Extension;
using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;
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
        public void TestVector3Extension()
        {
            //TODO: Реализовать поиск возможных сайтов для диммера             
            Vector3 basicVector = new Vector3(1, 0, 0);
            Vector3 globalBasicVector = new Vector3(2, 2, 0);
            var length = Math.Round(globalBasicVector.Length(), 4);
            // Ti - Ti (Ti - Cr, Cr - Cr) in fcc
            var schemes = new List<(double, double)>()
            {
                (0, 0),
                (90, 0),
                (180, 0),
                (270, 0),
                (0, 90),
                (0, -90),
                (45, 0),
                (135, 0),
                (225, 0),
                (315, 0),
                (0, 45),
                (90, 45),
                (180, 45),
                (270, 45)
            };
            var resultedVectors = globalBasicVector.RotatesInXYPlane(schemes);
            var printableResult = Newtonsoft.Json.JsonConvert.SerializeObject(resultedVectors);
            Console.Write(printableResult);
            // Если хоть один вектор длинной отличается, значит ошибка
            Assert.True(resultedVectors.FirstOrDefault(x => Math.Round(x.Length(), 4) != length) == default);
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
