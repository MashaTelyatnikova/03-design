using System;
using battleships.Interfaces;
using Ninject.Modules;

namespace battleships
{
    class AiTesterModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Settings>().ToSelf().WithConstructorArgument("settingsFilename", "settings.txt");
            Bind<IGameVisualizer>().To<GameVisualizer>();
            Bind<IMapGenerator>().To<MapGenerator>();
            Bind<ProcessMonitor>().ToSelf();
            Bind<IAiFactory>().To<AiFactory>();
            Bind<IGameFactory>().To<GameFactory>();
            Bind<AiTester>()
                .To<AiTester>()
                .WithConstructorArgument("textWriter", Console.Out)
                .WithConstructorArgument("textReader", Console.In);

           
        }
    }
}
