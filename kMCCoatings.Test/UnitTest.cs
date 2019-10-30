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
            //TODO: Реализовать формирование списка возможных трансляций вектора доя поиска сайта
            Dimer dimer = new Dimer(firstAtomInDimer, secondAtomInDimer, DimerSettings);
            dimers.Add(dimer);
        }

        [Fact]
        public void AddAtomToCalculator()
        {
            var ti = new Element()
            {
                Id = 22,
                InteractionEnergy = new Dictionary<int, double>()
                {
                    {7, 2},
                    {22, 2},
                    {24, 1.5}
                },
                InteractionRadius = new Dictionary<int, double>()
                {
                    {7, 0.70710678},
                    {22, 1},
                    {24, 1}
                }
            };
            var cr = new Element()
            {
                Id = 24,
                InteractionEnergy = new Dictionary<int, double>()
                {
                    {7, 1.8},
                    {22, 1.5},
                    {24, 1.3}
                },
                InteractionRadius = new Dictionary<int, double>()
                {
                    {7, 0.70710678},
                    {22, 1},
                    {24, 1}
                }
            };
            var n = new Element()
            {
                Id = 7,
                InteractionEnergy = new Dictionary<int, double>()
                {
                    {7, 1},
                    {22, 2},
                    {24, 1.8}
                },
                InteractionRadius = new Dictionary<int, double>()
                {
                    {7, 0.70710678},
                    {22, 0.70710678},
                    {24, 0.70710678}
                }
            };
            var cals = new CalculatorSettings()
            {
                Dimension = new Point3D(100, 100, 100),
                ContactRadius = 1.3,
                CrossRadius = 3,
                ForbiddenRadius = 0.707,
                DiffusionRadius = 1,
                InteractionRadius = 2,
                ContactRule = 3
            };
            var cacl = new Calculator(cals);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var O = new Point3D(10, 10, 10);
            var z1 = new Point3D(10, 10, 11);
            var z_1 = new Point3D(10, 10, 9);
            var y1 = new Point3D(10, 11, 10);
            var y_1 = new Point3D(10, 9, 10);
            var z2 = new Point3D(10, 10, 12);
            var y1z1 = new Point3D(10, 11, 11);
            cacl.AddAtom(O, n);
            cacl.AddAtom(z1, n);
            cacl.AddAtom(z_1, n);
            cacl.AddAtom(y1, n);
            cacl.AddAtom(y_1, n);
            cacl.AddAtom(z2, n);
            var lists = Newtonsoft.Json.JsonConvert.SerializeObject(cacl.SiteService.SitesByCells.SelectMany(x => x.Value).ToList().Select(x => x.Coordinates));

            Debug.Print(lists);
            int numberOfSites = 0;
            foreach (var cell in cacl.SiteService.SitesByCells.Values)
            {
                numberOfSites += cell.Count();
            }
            int numberOfTransition = 0;
            foreach (var trans in cacl.Transitions.Values)
            {
                numberOfTransition += trans.Count();
            }
            Assert.True(numberOfSites == 33);
            Assert.True(numberOfTransition == 28);
            stopWatch.Stop();
            Console.WriteLine($"Ellapsed time: {stopWatch.ElapsedMilliseconds}");
        }

        [Fact]
        public void IsContains()
        {
            var lattice = DimerSettings.Lattices.First();
            Assert.True(lattice.IsContains(firstAtomInDimer.Element.Id, secondAtomInDimer.Element.Id));
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



        [Fact]
        public void AffectedAtomsCalculation()
        {
            //TODO: Выполнить тест диффузии заданного количества атомов без потока.
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
            var chrome = new Element()
            {
                Id = 24
            };
            var titanium = new Element()
            {
                Id = 22
            };
            var nitride = new Element()
            {
                Id = 7
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

