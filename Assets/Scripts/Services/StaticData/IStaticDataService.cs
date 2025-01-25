using Assets.Scripts.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Services.StaticData
{
    internal interface IStaticDataService : IService
    {
        void Load();
        LevelStaticData ForLevel(int level);
    }
}
