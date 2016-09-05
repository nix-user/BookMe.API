﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public void CheckConnection_Should_Not_Throw_Exception()
        {
            ResourceParser parser = new ResourceParser(new ClientContext(Constants.BaseAddress)); 
            parser.CheckConnection();
        }
    }
}
