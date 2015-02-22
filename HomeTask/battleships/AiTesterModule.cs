using System;
using Ninject.Modules;
using NLog;

namespace battleships
{
    class AiTesterModule : NinjectModule
    {
        public override void Load()
        {
            var settings = new Settings("settings.txt");
            
            Bind<Settings>().ToSelf().WithConstructorArgument("settingsFilename", "settings.txt");
           
            Bind<IGameVisualizer>().To<GameVisualizer>();
           
            Bind<IMapGenerator>()
                .To<MapGenerator>()
                .WithConstructorArgument("width", settings.Width)
                .WithConstructorArgument("height", settings.Height)
                .WithConstructorArgument("shipSizes", settings.Ships)
                .WithConstructorArgument("random", new Random(settings.RandomSeed));

            Bind<ProcessMonitor>()
                    .ToSelf()
                    .WithConstructorArgument("timeLimit", TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount))
                    .WithConstructorArgument("memoryLimit", (long)settings.MemoryLimit);

            Bind<Logger>().ToConstant(LogManager.GetLogger("results"));
        }
    }
}
