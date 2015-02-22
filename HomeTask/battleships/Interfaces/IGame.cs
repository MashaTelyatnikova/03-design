using System;
using battleships.Enums;

namespace battleships.Interfaces
{
    public interface IGame
    {
        Vector LastTarget { get;}
        int TurnsCount { get;}
        int BadShots { get; }
        Map Map { get;}
        ShotInfo LastShotInfo { get;  }
        bool AiCrashed { get; }
        Exception LastError { get;  }

        bool IsOver();

        void MakeStep();
    }
}
