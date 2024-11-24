using Rocket.API;

namespace CarInsurance
{
    public class Conf : IRocketPluginConfiguration
    {
        public uint Cost;
        public void LoadDefaults()
        {
            Cost = 1000;
        }
    }
}
