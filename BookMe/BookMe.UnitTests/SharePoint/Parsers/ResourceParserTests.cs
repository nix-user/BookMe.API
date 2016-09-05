using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BookMe.ShareProint.Data;
using BookMe.ShareProint.Data.Parsers;
using BookMe.ShareProint.Data.Parsers.Concrete;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookMe.UnitTests.SharePoint.Parsers
{
    [TestClass]
    public class ResourceParserTests
    {
        private Mock<ClientContext> clientContextMock;

        [TestMethod]
        public void CheckConnection_ValidBaseAddress_Should_Not_Throw_Exception()
        {
            ResourceParser parser = new ResourceParser(new ClientContext(Constants.BaseAddress)); 
            parser.CheckConnection();
        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void CheckConnection_InvalidBaseAddress_Should_Throw_Exception()
        {
            string wrongBaseAddress = "http://wrongBaseAddress.com";

            ResourceParser parser = new ResourceParser(new ClientContext(wrongBaseAddress));
            parser.CheckConnection();
        }
    }
}
