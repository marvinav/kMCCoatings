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
            const string path = @"C:\Users\av_ch\source\repos\kMCCoatings\kMCCoatings.Core\Lattice\fcc.json";
            using var fs = new FileStream(path, FileMode.Open);
            using var sr = new StreamReader(fs);
            var fileContent = sr.ReadToEnd();
            var result = DimerSettings.GetLatticeFromJson(fileContent);
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
            var calсSettings = new CalculatorSettings()
            {
                Dimension = new Point3D(100, 100, 100),
                ContactRadius = 1.3,
                CrossRadius = 3,
                ForbiddenRadius = 0.5,
                DiffusionRadius = 1,
                InteractionRadius = 2,
                ContactRule = 3
            };
            var calc = new Calculator(new Settings() { Calculator = calсSettings });

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var O = new Point3D(10, 10, 10);
            var z1 = new Point3D(10, 10, 11);
            var z_1 = new Point3D(10, 10, 9);
            var y1 = new Point3D(10, 11, 10);
            var y_1 = new Point3D(10, 9, 10);
            var z2 = new Point3D(10, 10, 12);
            calc.AddAtom(O, n);
            calc.AddAtom(z1, n);
            calc.AddAtom(z_1, n);
            calc.AddAtom(y1, n);
            calc.AddAtom(y_1, n);
            // calc.AddAtom(z2, n);
            var lists = Newtonsoft.Json.JsonConvert.SerializeObject(calc.SiteService.SitesByCells.SelectMany(x => x.Value).ToList().Select(x => new { Coor = x.Coordinates, Dimer = x.DimerAtom?.Site.Coordinates, Occupied = x.OccupiedAtom?.Site.Coordinates, Reason = x.ProhibitedReason }));

            Debug.Print(lists);
            int numberOfSites = 0;
            foreach (var cell in calc.SiteService.SitesByCells.Values)
            {
                numberOfSites += cell.Count;
            }
            int numberOfTransition = 0;
            foreach (var trans in calc.Transitions.Values)
            {
                numberOfTransition += trans.Count;
            }
            Assert.True(numberOfSites == 21);
            Assert.True(numberOfTransition == 16);
            stopWatch.Stop();
            Console.WriteLine($"Elapsed time: {stopWatch.ElapsedMilliseconds}");
        }

        [Fact]
        public void IsContains()
        {
            var lattice = DimerSettings.Lattices[0];
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
        /// Проверка формирования списка трансляции
        /// </summary>
        [Fact]
        public void CreateVectorFromMath()
        {
            Vector3D vector = new Vector3D(2, 3, 1);
            var rotationVector = new Vector3D(1, -1, 1);
            var a = Angle.FromDegrees(355);

            var resultedVector = vector.Rotate(rotationVector, a);
            Console.WriteLine(resultedVector.ToString());
            Assert.True(false);
        }

        [Fact]
        public void AffectedAtomsCalculation()
        {
            //TODO: Выполнить тест диффузии заданного количества атомов без потока.
        }

        [Fact]
        public void LoadJsonSettings()
        {
            var calc = CalculatorFabric.CreateCalculator("C:\\Users\\av_ch\\source\\repos\\kmcCoatings\\kmcCoatings.Test\\settings.json");
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

            const string path = @"C:\Users\av_ch\source\repos\kMCCoatings\kMCCoatings.Core\Lattice\fcc.json";
            using var fs = new FileStream(path, FileMode.Open);
            using var sr = new StreamReader(fs);
            var fileContent = sr.ReadToEnd();
            DimerSettings.Lattices = DimerSettings.GetLatticeFromJson(fileContent);
        }
    }
}