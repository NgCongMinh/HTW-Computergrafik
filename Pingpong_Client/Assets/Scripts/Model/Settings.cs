using System;

namespace Model
{
    [Serializable]
    public class Settings
    {
        public ServerSetting serverSetting;
    }

    [Serializable]
    public class ServerSetting
    {
        public string protocol;

        public string baseUrl;

        public string suffix;

        public string getAddress()
        {
            return protocol + baseUrl + suffix;
        }
    }
}