using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flyable
{
    public class Config : IRocketPluginConfiguration
    {
        public float SpeedInFly;
        public void LoadDefaults()
        {
            SpeedInFly = 3;
        }
    }
}
