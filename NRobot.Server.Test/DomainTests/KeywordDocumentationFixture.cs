using System;
using NRobot.Server.Imp.Config;
using NRobot.Server.Imp.Domain;
using NUnit.Framework;

namespace NRobot.Server.Test.DomainTests
{

    /// <summary>
    /// Tests that check the xml documentation on a keyword
    /// </summary>
    [TestFixture]
    class KeywordDocumentationFixture
    {

        private KeywordManager _keywordManager;
        private const string Typename = "NRobot.Server.Test.Keywords.WithDocumentationClass";

        [TestFixtureSetUp]
        public void Setup()
        {
            var config = new LibraryConfig();
            config.Assembly = "NRobot.Server.Test";
            config.TypeName = Typename;
            config.Documentation = "NRobot.Server.Test.xml";
            _keywordManager = new KeywordManager();
            _keywordManager.AddLibrary(config);
        }

        [Test]
        public void GetKeywordDocumentation_HasComments()
        {
            var keyword = _keywordManager.GetKeyword(Typename, "MethodWithComments");
            Assert.IsFalse(String.IsNullOrEmpty(keyword.KeywordDocumentation));
            Assert.IsTrue(keyword.KeywordDocumentation == "This is a method with a comment");
        }

        [Test]
        public void GetKeywordDocumentation_NoComments()
        {
            var keyword = _keywordManager.GetKeyword(Typename, "MethodWithNoComment");
            Assert.IsTrue(String.IsNullOrEmpty(keyword.KeywordDocumentation));
        }

        [Test]
        public void GetKeywordDocumentation_NoXMLFileLoaded()
        {
            var config = new LibraryConfig();
            config.Assembly = "NRobot.Server.Test";
            config.TypeName = "NRobot.Server.Test.Keywords.WithDocumentationClass";
            var keywordManager = new KeywordManager();
            keywordManager.AddLibrary(config);
            var keyword = keywordManager.GetKeyword(Typename, "MethodWithComments");
            Assert.IsTrue(String.IsNullOrEmpty(keyword.KeywordDocumentation));
        }

    }
}
