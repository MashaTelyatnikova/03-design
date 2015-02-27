using System;
using Ninject.Modules;

namespace battleships
{
    class AiTesterModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Settings>().ToSelf().WithConstructorArgument("settingsFilename", "settings.txt");
            Bind<ProcessMonitor>().ToSelf();
         
            Bind<AiTester>()
                .To<AiTester>()
                .WithConstructorArgument("textWriter", Console.Out)
                .WithConstructorArgument("textReader", Console.In);

     
        }
    }
}
