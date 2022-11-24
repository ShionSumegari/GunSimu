using System;

namespace TeraJet { 
    public static class TeraJetServerConfig
    {
        public static readonly string BASE_PATH = "https://us-central1-terajet-954dd.cloudfunctions.net/api";
        public static readonly string GAME_ADS_CONFIG_PATH = "/game-ads";

        public static readonly int SUCCESS_CODE = 200;
        public static readonly int ERROR_CODE = 500;
    }
}

