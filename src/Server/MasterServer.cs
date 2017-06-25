﻿namespace Destiny.Server
{
    public static class MasterServer
    {
        public static bool IsAlive { get; private set; }

        public static LoginServer Login { get; private set; }
        public static WorldServer World { get; private set; }
        public static ChannelServer[] Channels { get; private set; }
        public static CashShopServer CashShop { get; private set; }

        static MasterServer()
        {
            MasterServer.Login = new LoginServer(8484);
            MasterServer.World = new WorldServer();
            MasterServer.Channels = new ChannelServer[MasterServer.World.Channels];

            for (byte i = 0; i < MasterServer.Channels.Length; i++)
            {
                MasterServer.Channels[i] = new ChannelServer(i, (short)(8585 + i));
            }

            MasterServer.CashShop = new CashShopServer(9000);
        }

        public static void Start()
        {
            MasterServer.Login.Start();

            foreach (ChannelServer channel in MasterServer.Channels)
            {
                channel.Start();
            }

            MasterServer.CashShop.Start();

            MasterServer.IsAlive = true;

            Log.Success("MasterServer started.");
        }

        public static void Stop()
        {
            MasterServer.Login.Stop();

            foreach (ChannelServer channel in MasterServer.Channels)
            {
                channel.Stop();
            }

            MasterServer.CashShop.Stop();

            MasterServer.IsAlive = false;

            Log.Warn("MasterServer stopped.");
        }

        public static bool IsAccountOnline(int accountID)
        {
            foreach (ChannelServer channel in MasterServer.Channels)
            {
                lock (channel.Clients)
                {
                    foreach (MapleClient client in channel.Clients)
                    {
                        if (client.Account != null && client.Account.ID == accountID)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
