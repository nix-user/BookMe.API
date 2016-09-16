using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BookMe.ShareProint.Data;
using BookMe.ShareProint.Data.Constants;
using BookMe.ShareProint.Data.Parsers.Concrete;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookMe.IntegrationTests.SharePoint.Parsers
{
    [TestClass]
    public class ResourceParserTests
    {
        [TestMethod]
        public void CheckConnection_ValidBaseAddress_Should_Not_Throw_Exception()
        {
            ResourceParser parser = new ResourceParser(new ClientContext(UriConstants.BaseAddress), null);
            parser.CheckConnection();
        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void CheckConnection_InvalidBaseAddress_Should_Throw_Exception()
        {
            string wrongBaseAddress = "http://wrongBaseAddress.com";

            ResourceParser parser = new ResourceParser(new ClientContext(wrongBaseAddress), null);
            parser.CheckConnection();
        }
    }
}
