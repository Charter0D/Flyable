using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Flyable
{
    public class Plugin : RocketPlugin<Config>
    {
        public List<CSteamID> InFly { get; set; }
        public static Plugin Instance { get; set; }

        protected override void Load()
        {
            Instance = this;
            InFly = new List<CSteamID>();
            U.Events.OnPlayerDisconnected += Events_OnPlayerDisconnected;
        }

        private void Events_OnPlayerDisconnected(UnturnedPlayer player)
        {
            if (InFly.Contains(player.CSteamID)) InFly.Remove(player.CSteamID);
        }

        protected override void Unload()
        {
            U.Events.OnPlayerDisconnected -= Events_OnPlayerDisconnected;
        }

        public void Update()
        {
            foreach (var id in InFly)
            {
                var v = UnturnedPlayer.FromCSteamID(id);
                
                if (v.Player.input.keys[0])
                {
                    v.Teleport(new Vector3(v.Position.x, v.Position.y + 0.1f, v.Position.z), v.Rotation);
                }
                else if (v.Player.input.keys[5])
                {
                    v.Teleport(new Vector3(v.Position.x, v.Position.y - 1, v.Position.z), v.Rotation);
                }
            }
        }

        public void SendFly(UnturnedPlayer p, bool fly)
        {
            if (fly)
            {
                p.Player.movement.sendPluginGravityMultiplier(0);
                p.Player.movement.sendPluginSpeedMultiplier(Configuration.Instance.SpeedInFly);
                p.Features.GodMode = true;
                InFly.Add(p.CSteamID);
            }
            else
            {
                p.Player.movement.sendPluginGravityMultiplier(1);
                p.Player.movement.sendPluginSpeedMultiplier(1);
                p.Features.GodMode = false;
                InFly.Remove(p.CSteamID);
            }
        }
    }
}
