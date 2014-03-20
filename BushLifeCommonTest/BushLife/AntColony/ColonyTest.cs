using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

using TypeMock.ArrangeActAssert;
using TypeMock;

using AU.Com.BushLife.Framework.Matrix;
using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.AntColony
{
	[TestFixture]
	[Isolated]
	public class ColonyTest
	{
		private PathTravellingSalesmanStep Adelaide;
		private PathTravellingSalesmanStep Brisbane;
		private PathTravellingSalesmanStep Canberra;
		private PathTravellingSalesmanStep Darwin;
		private PathTravellingSalesmanStep Melbourne;
		private PathTravellingSalesmanStep Perth;
		private PathTravellingSalesmanStep Sydney;
		private NodeTravellingSalesmanStep AdelaideNode;
		private NodeTravellingSalesmanStep BrisbaneNode;
		private NodeTravellingSalesmanStep CanberraNode;
		private NodeTravellingSalesmanStep DarwinNode;
		private NodeTravellingSalesmanStep MelbourneNode;
		private NodeTravellingSalesmanStep PerthNode;
		private NodeTravellingSalesmanStep SydneyNode;

		private PathTravellingSalesmanEdge AdelaideBrisbane;
		private PathTravellingSalesmanEdge AdelaideCanberra;
		private PathTravellingSalesmanEdge AdelaideDarwin;
		private PathTravellingSalesmanEdge AdelaideMelbourne;
		private PathTravellingSalesmanEdge AdelaidePerth;
		private PathTravellingSalesmanEdge BrisbaneAdelaide;
		private PathTravellingSalesmanEdge BrisbaneDarwin;
		private PathTravellingSalesmanEdge BrisbaneSydney;
		private PathTravellingSalesmanEdge CanberraAdelaide;
		private PathTravellingSalesmanEdge CanberraMelbourne;
		private PathTravellingSalesmanEdge CanberraSydney;
		private PathTravellingSalesmanEdge DarwinAdelaide;
		private PathTravellingSalesmanEdge DarwinBrisbane;
		private PathTravellingSalesmanEdge MelbourneAdelaide;
		private PathTravellingSalesmanEdge MelbourneCanberra;
		private PathTravellingSalesmanEdge MelbourneSydney;
		private PathTravellingSalesmanEdge SydneyBrisbane;
		private PathTravellingSalesmanEdge SydneyCanberra;
		private PathTravellingSalesmanEdge SydneyMelbourne;
		private PathTravellingSalesmanEdge PerthAdelaide;


		[FixtureInitializer]
		private void FixtureInitialise()
		{
			Adelaide = new PathTravellingSalesmanStep("Adelaide");
			Brisbane = new PathTravellingSalesmanStep("Brisbane");
			Canberra = new PathTravellingSalesmanStep("Canberra");
			Darwin = new PathTravellingSalesmanStep("Darwin");
			Perth = new PathTravellingSalesmanStep("Perth");
			Melbourne = new PathTravellingSalesmanStep("Melbourne");
			Sydney = new PathTravellingSalesmanStep("Sydney");

			AdelaideNode = new NodeTravellingSalesmanStep("Adelaide");
			BrisbaneNode = new NodeTravellingSalesmanStep("Brisbane");
			CanberraNode = new NodeTravellingSalesmanStep("Canberra");
			DarwinNode = new NodeTravellingSalesmanStep("Darwin");
			PerthNode = new NodeTravellingSalesmanStep("Perth");
			MelbourneNode = new NodeTravellingSalesmanStep("Melbourne");
			SydneyNode = new NodeTravellingSalesmanStep("Sydney");

			AdelaideBrisbane = new PathTravellingSalesmanEdge(Adelaide, Brisbane, 2280);
			AdelaideCanberra = new PathTravellingSalesmanEdge(Adelaide, Canberra, 1170);
			AdelaideDarwin = new PathTravellingSalesmanEdge(Adelaide, Darwin, 3040);
			AdelaideMelbourne = new PathTravellingSalesmanEdge(Adelaide, Melbourne, 750);
			AdelaidePerth = new PathTravellingSalesmanEdge(Adelaide, Perth, 1850);

			BrisbaneAdelaide = new PathTravellingSalesmanEdge(Brisbane, Adelaide, 2280);
			BrisbaneDarwin = new PathTravellingSalesmanEdge(Brisbane, Darwin, 3440);
			BrisbaneSydney = new PathTravellingSalesmanEdge(Brisbane, Sydney, 970);

			CanberraAdelaide = new PathTravellingSalesmanEdge(Canberra, Adelaide, 1170);
			CanberraMelbourne = new PathTravellingSalesmanEdge(Canberra, Melbourne, 650);
			CanberraSydney = new PathTravellingSalesmanEdge(Canberra, Sydney, 290);

			DarwinAdelaide = new PathTravellingSalesmanEdge(Darwin, Adelaide, 3040);
			DarwinBrisbane = new PathTravellingSalesmanEdge(Darwin, Brisbane, 3440);

			MelbourneAdelaide = new PathTravellingSalesmanEdge(Melbourne, Adelaide, 750);
			MelbourneCanberra = new PathTravellingSalesmanEdge(Melbourne, Canberra, 650);
			MelbourneSydney = new PathTravellingSalesmanEdge(Melbourne, Sydney, 880);

			SydneyBrisbane = new PathTravellingSalesmanEdge(Sydney, Brisbane, 970);
			SydneyCanberra = new PathTravellingSalesmanEdge(Sydney, Canberra, 290);
			SydneyMelbourne = new PathTravellingSalesmanEdge(Sydney, Melbourne, 880);

			PerthAdelaide = new PathTravellingSalesmanEdge(Perth, Adelaide, 1850);

			Adelaide.Edges = new List<IEdge>()
			{
				AdelaideBrisbane,
				AdelaideDarwin,
				AdelaideMelbourne,
				AdelaideCanberra,
				AdelaidePerth
			};
			Brisbane.Edges = new List<IEdge>()
			{
				BrisbaneAdelaide,
				BrisbaneDarwin,
				BrisbaneSydney
			};
			Canberra.Edges = new List<IEdge>()
			{
				CanberraMelbourne,
				CanberraAdelaide,
				CanberraSydney
			};
			Darwin.Edges = new List<IEdge>()
			{
				DarwinAdelaide,
				DarwinBrisbane
			};
			Perth.Edges = new List<IEdge>()
			{
				PerthAdelaide
			};
			Melbourne.Edges = new List<IEdge>()
			{
				MelbourneAdelaide,
				MelbourneCanberra,
				MelbourneSydney
			};
			Sydney.Edges = new List<IEdge>()
			{
				SydneyCanberra,
				SydneyBrisbane,
				SydneyMelbourne
			};
		}

		private IEnumerable<object[]> PathColonyTestData
		{
			get
			{
				#region Test 1: Travelling Salesman Test
				yield return new object[]
				{
					40,			// Number of ants
					200,		// Number of iterations
					0.001m,		// Initial Pheremone
					0.01m,		// Evaporation rate
					new PathTravellingSalesmanPath()
					{
					},
					new List<IStep>()
					{
						Brisbane,
						Adelaide,
						Canberra,
						Darwin,
						Melbourne,
						Perth,
						Sydney
					},
					#region Expected Results
					new List<IPathPath>()
					{
						new PathTravellingSalesmanPath()
						{
							Edges = new List<IEdge>()
							{
								PerthAdelaide,
								AdelaideMelbourne,
								MelbourneCanberra,
								CanberraSydney,
								SydneyBrisbane,
								BrisbaneDarwin
							}
						},
						new PathTravellingSalesmanPath()
						{
							Edges = new List<IEdge>()
							{
								DarwinBrisbane,
								BrisbaneSydney,
								SydneyCanberra,
								CanberraMelbourne,
								MelbourneAdelaide,
								AdelaidePerth
							}
						}
					}
					#endregion
				};
				#endregion
			}
		}

		[Test]
		[Factory("PathColonyTestData")]
		[Isolated]
        [Disable("Initialise Pheremones not set up properly")]
		public void AntColonyPathAntTest(Int32 numberOfAnts, Int32 numberOfIterations, decimal initialPheremone, decimal evaporationRate,
			IPathPath path,
			ICollection<IStep> steps,
			ICollection<IPathPath> expectedResult)
		{
			#region Set up test data
			IAntColonyContext context = new AntColonyContext();
			context.Path = path;
			context.Steps = steps;
			#endregion

			#region Run Test
			Colony<PathAnt, IEdge> colony = new Colony<PathAnt, IEdge>(numberOfAnts, numberOfIterations, initialPheremone, evaporationRate, InitialisePathPheremones);
			colony.InitialiseAnts(context);
            colony.InitialisePheremones();
			colony.RunAnts();
			#endregion

			#region Verify Results
			Assert.IsNotNull(context.BestPaths);
			context.BestPaths.ForEach(p => Console.WriteLine(string.Format("Best Path: [{0}] {1},{2}", (p as IPathPath).Score, (p as IPathPath).Edges.First().StartStep.StepName, string.Join(",", (p as IPathPath).Edges.Select(e => e.EndStep.StepName)))));
			Assert.AreEqual(expectedResult.Count, context.BestPaths.Count);
			Assert.AreElementsEqualIgnoringOrder(expectedResult, context.BestPaths);
			#endregion

		}

        private void InitialisePathPheremones(IAntColonyContext context, IDictionary<IEdge,Pheremone> map, decimal initialPheremoneLevel)
        {
            AntColonyContext c = context as AntColonyContext;
            foreach (IStep step in c.Steps)
            {
				//if (!map.ContainsKey(step))
				//    map[step] = new Pheremone(initialPheremoneLevel);
            }
        }


		private IEnumerable<object[]> NodeColonyTestData
		{
			get
			{
				yield return new object[]
				{
					20,			// Number of ants
					400,		// Number of iterations
					0.001m,		// Initial Pheremone
					0.01m,		// Evaporation rate
					new NodeTravellingSalesmanPath(),
					#region Expected Results
					new List<INodePath>()
					{
						new NodeTravellingSalesmanPath()
						{
						}
					}
					#endregion
				};
			}
		}

		[Test]
		[Factory("NodeColonyTestData")]
		[Isolated]
		[Disable("The mechanism for this mode not yet sorted")]
		public void AntColonyNodeAntTest(Int32 numberOfAnts, Int32 numberOfIterations, decimal initialPheremone, decimal evaporationRate,
			INodePath path,
			ICollection<INodePath> expectedResult)
		{
			#region Set up test data
			Matrix<IStep, IStep, decimal> distances = new Matrix<IStep, IStep, decimal>();
			distances.Add(AdelaideNode, BrisbaneNode, 2280);
			distances.Add(AdelaideNode, CanberraNode, 1170);
			distances.Add(AdelaideNode, DarwinNode, 3040);
			distances.Add(AdelaideNode, MelbourneNode, 750);
			distances.Add(AdelaideNode, PerthNode, 1850);

			distances.Add(BrisbaneNode, SydneyNode, 970);
			distances.Add(BrisbaneNode, DarwinNode, 3440);

			distances.Add(CanberraNode, SydneyNode, 290);
			distances.Add(CanberraNode, MelbourneNode, 650);

			distances.Add(SydneyNode, MelbourneNode, 880);

			IAntColonyContext context = new AntColonyContext();
			context.Path = path;
			context.Steps = distances.HorizontalKeys().Union(distances.VerticalKeys()).Distinct().ToList();
			#endregion

			#region Run Test
			Colony<NodeAnt, IStep> colony = new Colony<NodeAnt, IStep>(numberOfAnts, numberOfIterations, initialPheremone, evaporationRate, InitialiseNodePheremones);
			colony.InitialiseAnts(context);
			colony.RunAnts();
			#endregion

			#region Verify Results
			Assert.IsNotNull(context.BestPaths);
			context.BestPaths.ForEach(p => Console.WriteLine(string.Format("Best Path: [{0}] {1}", (p as INodePath).Score, string.Join(",", (p as INodePath).Steps.Select(e => (e as IStep).StepName)))));
			Assert.AreEqual(expectedResult.Count, context.BestPaths.Count);
			Assert.AreElementsEqualIgnoringOrder(expectedResult, context.BestPaths);
			#endregion

		}

		private void InitialiseNodePheremones(IAntColonyContext context, IDictionary<IStep, Pheremone> map, decimal initialPheremoneLevel)
		{
			AntColonyContext c = context as AntColonyContext;
			foreach (IStep step in c.Steps)
			{
				if (!map.ContainsKey(step))
					map[step] = new Pheremone(initialPheremoneLevel);
			}
		}


	}
}
