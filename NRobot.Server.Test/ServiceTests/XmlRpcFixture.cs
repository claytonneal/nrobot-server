using System;
using CookComputing.XmlRpc;
using NRobot.Server.Imp.Config;
using NRobot.Server.Imp.Services;
using NRobot.Server.Imp;
using NUnit.Framework;

namespace NRobot.Server.Test.ServiceTests
{

#pragma warning disable 1591

    /// <summary>
    /// Tests to call the individual xml-rpc methods and assert the returned values
    /// </summary>
    [TestFixture]
    public class XmlRpcFixture
    {

        private NRobotService _service;

        [SetUp]
        public void Setup()
        {
            //start service
            var config = new NRobotServerConfig();
            config.Port = 8270;
            config.AssemblyConfigs.Add("NRobot.Server.Test.Keywords.TestKeywords",
                new LibraryConfig()
                {
                    Assembly = "NRobot.Server.Test",
                    TypeName = "NRobot.Server.Test.Keywords.TestKeywords",
                    Documentation = "NRobot.Server.Test.xml"
                });
            config.AssemblyConfigs.Add("NRobot.Server.Test.Keywords.WithDocumentationClass",
                new LibraryConfig()
                {
                    Assembly = "NRobot.Server.Test",
                    TypeName = "NRobot.Server.Test.Keywords.WithDocumentationClass",
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

        [TearDown]
        public void TearDown()
        {
            if (_service != null)
            {
                _service.Stop();
            }
        }


#region get_keyword_names

        [Test]
        public void get_keyword_names()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/TestKeywords";
            string[] result = client.get_keyword_names();
            Assert.IsTrue(result.Length > 0);
            Assert.Contains("INT RETURNTYPE",result);
        }

        [ExpectedException(typeof(XmlRpcFaultException))]
        [Test]
        public void get_keyword_names_invalid_url()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/UnknownType";
            string[] result = client.get_keyword_names();
            Assert.IsTrue(result.Length > 0);
            Assert.Contains("INT RETURNTYPE", result);
        }

#endregion

#region get_keyword_arguments

        [Test]
        public void get_keyword_arguments()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/TestKeywords";
            string[] result = client.get_keyword_arguments("STRING PARAMETERTYPE");
            Assert.IsTrue(result.Length > 0);
            Assert.Contains("arg1", result);
            Assert.Contains("arg2", result);
        }

#endregion

#region get_keyword_documentation

        [Test]
        public void get_keyword_documentation()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/WithDocumentationClass";
            string result = client.get_keyword_documentation("MethodWithComments");
            Assert.IsFalse(String.IsNullOrEmpty(result));
            Assert.IsTrue(result == "This is a method with a comment");
        }

#endregion

#region run_keyword

        // NOTE: These are the same as the domain tests
        // However here we are asserting on the xml-rpc structure returned


        [Test]
        public void RunKeyword_NoArgs_VoidReturn_EmptyArgs()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/RunKeyword";
            var result = client.run_keyword("NoInputNoOutput", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "PASS");
            Assert.IsTrue(String.IsNullOrEmpty(result["error"].ToString()));
            Assert.IsTrue(String.IsNullOrEmpty(result["return"].ToString()));
        }

        [Test]
        public void RunKeyword_ThrowsException()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/RunKeyword";
            var result = client.run_keyword("ThrowsException", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "FAIL");
            Assert.IsTrue(String.IsNullOrEmpty(result["return"].ToString()));
            Assert.IsFalse(result.ContainsKey("fatal"));
            Assert.IsFalse(result.ContainsKey("continuable"));
            Assert.IsTrue(result["error"].ToString() == "A regular exception");
            Assert.IsFalse(String.IsNullOrEmpty(result["traceback"].ToString()));
        }

        [Test]
        public void RunKeyword_ThrowsFatalException()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/RunKeyword";
            var result = client.run_keyword("ThrowsFatalException", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "FAIL");
            Assert.IsTrue(String.IsNullOrEmpty(result["return"].ToString()));
            Assert.IsTrue(result.ContainsKey("fatal"));
            Assert.IsTrue(result["error"].ToString() == "A fatal exception");
            Assert.IsFalse(String.IsNullOrEmpty(result["traceback"].ToString()));
        }

        [Test]
        public void RunKeyword_ThrowsContinuableException()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/RunKeyword";
            var result = client.run_keyword("ThrowsContinuableException", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "FAIL");
            Assert.IsTrue(String.IsNullOrEmpty(result["return"].ToString()));
            Assert.IsTrue(result.ContainsKey("continuable"));
            Assert.IsTrue(result["error"].ToString() == "A continuable exception");
            Assert.IsFalse(String.IsNullOrEmpty(result["traceback"].ToString()));
        }

        [Test]
        public void RunKeyword_TraceOutput()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/RunKeyword";
            var result = client.run_keyword("WritesTraceOutput", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "PASS");
            Assert.IsTrue(String.IsNullOrEmpty(result["return"].ToString()));
            Assert.IsTrue(result["output"].ToString().Contains("First line"));
            Assert.IsTrue(result["output"].ToString().Contains("Second line"));
        }

        [Test]
        public void RunKeyword_IntReturnType()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/TestKeywords";
            var result = client.run_keyword("Int ReturnType", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "PASS");
            Assert.IsTrue(Convert.ToInt32(result["return"]) == 1);
        }

        [Test]
        public void RunKeyword_Int64ReturnType()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/TestKeywords";
            var result = client.run_keyword("Int64 ReturnType", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "PASS");
            Assert.IsTrue(Convert.ToInt32(result["return"]) == 1);
        }

        [Test]
        public void RunKeyword_StringReturnType()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/TestKeywords";
            var result = client.run_keyword("String ReturnType", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "PASS");
            Assert.IsTrue(Convert.ToString(result["return"]) == "1");
        }

        [Test]
        public void RunKeyword_DoubleReturnType()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/TestKeywords";
            var result = client.run_keyword("Double ReturnType", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "PASS");
            Assert.IsTrue(Convert.ToDouble(result["return"]).Equals(1));
        }

        [Test]
        public void RunKeyword_BooleanReturnType()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/TestKeywords";
            var result = client.run_keyword("Boolean ReturnType", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "PASS", result["error"].ToString());
            Assert.IsTrue(Convert.ToBoolean(result["return"]));
        }

        [Test]
        public void RunKeyword_StringArrayReturnType()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/TestKeywords";
            var result = client.run_keyword("StringArray ReturnType", new object[0]);
            var returnval = (string[]) result["return"];
            Assert.IsTrue(result["status"].ToString() == "PASS");
            Assert.IsTrue(returnval.Length == 3);
        }

        [Test]
        public void RunKeyword_LessThanRequiredArgs()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/TestKeywords";
            var result = client.run_keyword("String ParameterType", new object[] {"1"});
            Assert.IsTrue(result["status"].ToString() == "FAIL");
        }

        [Test]
        public void RunKeyword_MoreThanRequiredArgs()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/TestKeywords";
            var result = client.run_keyword("String ParameterType", new object[] {"1", "2", "3"});
            Assert.IsTrue(result["status"].ToString() == "FAIL");
        }

        [Test]
        public void RunKeyword_StaticMethod()
        {
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270/NRobot/Server/Test/Keywords/TestKeywords";
            var result = client.run_keyword("PublicStatic Method", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "PASS");
        }


#endregion

    }

#pragma warning restore 1591

}
