using NRobot.Server.Imp.Config;
using NRobot.Server.Imp.Domain;
using NRobot.Server.Imp;
using NUnit.Framework;

namespace NRobot.Server.Test.ServiceTests
{

#pragma warning disable 1591

    /// <summary>
    /// Tests to start and stop the service with different configurations
    /// </summary>
    [TestFixture]
    public class StartStopServiceFixture
    {

        private NRobotService _service;

        [TearDown]
        public void TearDown()
        {
            if (_service != null)
            {
                _service.Stop();
            }
        }


        [Test]
        public void StartService_SingleType()
        {
            var config = new NRobotServerConfig();
            config.Port = 8270;
            config.AssemblyConfigs.Add("NRobot.Server.Test.Keywords.TestKeywords",
                new LibraryConfig()
                { Assembly = "NRobot.Server.Test",
                    TypeName = "NRobot.Server.Test.Keywords.TestKeywords",
                    Documentation = "NRobot.Server.Test.xml" });
            _service = new NRobotService(config);
            _service.StartAsync();
        }

        [Test]
        public void StartService_MultipleTypes()
        {
            var config = new NRobotServerConfig();
            config.Port = 8270;
            config.AssemblyConfigs.Add("NRobot.Server.Test.Keywords.TestKeywords",
                new LibraryConfig()
                {
                    Assembly = "NRobot.Server.Test",
                    TypeName = "NRobot.Server.Test.Keywords.TestKeywords",
                    Documentation = "NRobot.Server.Test.xml"
                });
            config.AssemblyConfigs.Add("NRobot.Server.Test.Keywords.RunKeyword",
                new LibraryConfig()
                {
                    Assembly = "NRobot.Server.Test",
                    TypeName = "NRobot.Server.Test.Keywords.RunKeyword",
                    Documentation = "NRobot.Server.Test.xml"
                });
            _service = new NRobotService(config);
            _service.StartAsync();
        }

        [Test]
        public void StartService_NoDocumentation()
        {
            var config = new NRobotServerConfig();
            config.Port = 8270;
            config.AssemblyConfigs.Add("NRobot.Server.Test.Keywords.TestKeywords",
                new LibraryConfig()
                {
                    Assembly = "NRobot.Server.Test",
                    TypeName = "NRobot.Server.Test.Keywords.TestKeywords"
                });
            _service = new NRobotService(config);
            _service.StartAsync();
        }

        [ExpectedException(typeof(KeywordLoadingException))]
        [Test]
        public void StartService_InvalidAssembly()
        {
            var config = new NRobotServerConfig();
            config.Port = 8270;
            config.AssemblyConfigs.Add("NRobot.Server.Test.Keywords.TestKeywords",
                new LibraryConfig()
                {
                    Assembly = "NRobot.Server.TestUnknown",
                    TypeName = "NRobot.Server.Test.Keywords.TestKeywords",
                    Documentation = "NRobot.Server.Test.xml"
                });
            _service = new NRobotService(config);
            _service.StartAsync();
        }

        [ExpectedException(typeof(KeywordLoadingException))]
        [Test]
        public void StartService_InvalidType()
        {
            var config = new NRobotServerConfig();
            config.Port = 8270;
            config.AssemblyConfigs.Add("NRobot.Server.Test.Keywords.TestKeywords",
                new LibraryConfig()
                {
                    Assembly = "NRobot.Server.Test",
                    TypeName = "NRobot.Server.Test.Keywords.TestKeywordsUnknown",
                    Documentation = "NRobot.Server.Test.xml"
                });
            _service = new NRobotService(config);
            _service.StartAsync();
        }

        [ExpectedException(typeof(KeywordLoadingException))]
        [Test]
        public void StartService_InvalidDocumentation()
        {
            var config = new NRobotServerConfig();
            config.Port = 8270;
            config.AssemblyConfigs.Add("NRobot.Server.Test.Keywords.TestKeywords",
                new LibraryConfig()
                {
                    Assembly = "NRobot.Server.Test",
                    TypeName = "NRobot.Server.Test.Keywords.TestKeywords",
                    Documentation = "NRobot.Server.TestUnknown.XML"
                });
            _service = new NRobotService(config);
            _service.StartAsync();
        }


    }

#pragma warning restore 1591

}
