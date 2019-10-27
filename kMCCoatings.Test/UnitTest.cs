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
                ContactRadius = 1.25,
                CrossRadius = 3,
                ForbiddenRadius = 0.707,
                DiffusionRadius = 1,
                InteractionRadius = 2
            };
            var cacl = new Calculator(cals);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    for (int z = 0; z < 100; z++)
                    {
                        var curPont = new Point3D(x, y, z);
                        cacl.AddAtom(curPont, n);
                    }
                }
            }
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
        public void SelectBenchmark()
        {
            var points = new List<Atom>(1000000);
            var rnd = new Random();
            var counter = 0;

            for (int i = 0; i < 1000000; i++)
            {
                var x = rnd.Next(0, 100000);
                var y = rnd.Next(0, 100000);
                var z = rnd.Next(0, 100000);
                counter++;
                points.Add(new Atom()
                {
                    Site = new Site(),
                    Transitions = new List<Transition>(50)
                });
            };

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var atom = points[1000];
            Point3D dimension = new Point3D(100, 100, 100);
            stopWatch.Stop();
            Console.WriteLine($"Ellapsed time: {stopWatch.ElapsedMilliseconds}");
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

            firstAtomInDimer = new Atom()
            {
                Element = chrome,
                Site = new Site()
                {
                    Coordinates = new Point3D(0, 0, 0)
                }
            };
            secondAtomInDimer = new Atom()
            {
                Element = titanium,
                Site = new Site()
                {
                    Coordinates = new Point3D(1, 1, 0)
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

