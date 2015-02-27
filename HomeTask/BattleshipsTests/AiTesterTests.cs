using System;
using System.IO;
using battleships;
using battleships.Interfaces;
using FakeItEasy;
using NUnit.Framework;

namespace BattleshipsTests
{
    [TestFixture]
    public class AiTester_should
    {
        private IMapGenerator mapGenerator;
        private IGameVisualizer gameVisualizer;
        private TextWriter textWriter;
        private TextReader textReader;
        private Func<string, ProcessMonitor, IAi> createAi;
        private Func<Map, IAi, IGame> createGame;
        private Map map = new Map(5, 5);
        private IAi ai;
        [SetUp]
        public void Init()
        {
            mapGenerator = A.Fake<IMapGenerator>();
            A.CallTo(() => mapGenerator.GenerateMap()).Returns(map);

            gameVisualizer = A.Fake<IGameVisualizer>();
            A.CallTo(gameVisualizer).Where(call => call.Method.Name == "Visualize").WithAnyArguments().DoesNothing();

           
            ai = A.Fake<IAi>();
            createAi = (s, monitor) => ai;

            A.CallTo(() => ai.Dispose()).DoesNothing();
            A.CallTo(ai).Where(call => call.Method.Name == "Init").WithReturnType<Vector>().Returns(new Vector(0, 0));
            A.CallTo(ai).Where(call => call.Method.Name == "GetNextShot").WithReturnType<Vector>().Returns(new Vector(0, 0));
            var game = A.Fake<IGame>();
            A.CallTo(() => game.MakeStep()).Invokes(() => A.CallTo(()=>game.IsOver()).Returns(true));
            createGame = (map1, ai1) => game;

            textWriter = A.Fake<TextWriter>();
            textReader = A.Fake<TextReader>();
        }

        [Test]
        public void works_correctly_for_zero_games_amount()
        {
            var settings = new Settings();
            var tester = new AiTester(new Settings(), gameVisualizer, mapGenerator, new ProcessMonitor(settings), 
                createAi, createGame, textWriter, textReader);
            
            tester.TestSingleFile("");
            
            Assert.DoesNotThrow(() => A.CallTo(() => mapGenerator.GenerateMap()).MustNotHaveHappened());
            Assert.DoesNotThrow(() => A.CallTo(() => gameVisualizer.Visualize(null)).MustNotHaveHappened());
        }

        [Test]
        public void works_correctly_for_one_interactive_game()
        {
            var settings = new Settings() {GamesCount = 1, Interactive = true};
            var tester = new AiTester(settings, gameVisualizer, mapGenerator, 
                new ProcessMonitor(settings), createAi, createGame, textWriter, textReader);
            
            tester.TestSingleFile("");
            
            Assert.DoesNotThrow(() => A.CallTo(() => mapGenerator.GenerateMap()).MustHaveHappened());
            Assert.DoesNotThrow(() => A.CallTo(gameVisualizer).Where(call => call.Method.Name == "Visualize").WithAnyArguments().MustHaveHappened(Repeated.Exactly.Once));
        }

        [Test]
        public void works_correctly_for_one_non_interactive_game()
        {
            var settings = new Settings() {GamesCount = 1, Interactive = false};
            var tester = new AiTester(settings, gameVisualizer, mapGenerator, 
                new ProcessMonitor(settings), createAi, createGame, textWriter, textReader);
           
            tester.TestSingleFile("");
            
            Assert.DoesNotThrow(() => A.CallTo(() => mapGenerator.GenerateMap()).MustHaveHappened());
            Assert.DoesNotThrow(() => A.CallTo(gameVisualizer).Where(call => call.Method.Name == "Visualize").WithAnyArguments().MustNotHaveHappened());
        }

        [Test]
        public void works_correctly_for_several_non_interactive_games()
        {
            
            var settings = new Settings() {GamesCount = 2, Interactive = false};
            var m = new ProcessMonitor(settings);
            var tester = new AiTester(settings, gameVisualizer, mapGenerator, m
                , createAi, createGame, textWriter, textReader);
            
            tester.TestSingleFile("");

            Assert.DoesNotThrow(() => A.CallTo(createAi("", m)).MustHaveHappened());
            Assert.DoesNotThrow(() => A.CallTo(createGame(map, ai)).MustHaveHappened());
        }
    }
}
