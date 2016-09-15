using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Auth.Cryptography.Abstract;
using BookMe.Auth.Cryptography.Concrete;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookMe.UnitTests.Services
{
    [TestClass]
    public class AESCryptographyServiceTests
    {
        private const string ClearText = "text";
        private const string Key = "FB8F76CCDA872EF39893A0D8CE5EC574";
        private const string Cryptogram = "2kkqwaz2XdQyu9qyY37Y8g==";

        private AESCryptographyService ArrangeAESCryptographyService()
        {
            Mock<ISimetricCryptographyKeyProvider> keyProvider = new Mock<ISimetricCryptographyKeyProvider>();
            keyProvider.Setup(x => x.Key).Returns(Key);
            return new AESCryptographyService(keyProvider.Object);
        }

        [TestMethod]
        public void Encrypt_ClearText_ExpectedCryptogram()
        {
            // arrange
            AESCryptographyService cryptographyService = ArrangeAESCryptographyService();

            // act
            string cryptogram = cryptographyService.Encrypt(ClearText);

            // assert
            Assert.AreEqual(Cryptogram, cryptogram);
        }

        [TestMethod]
        public void Decrypt_Cryptogram_ExpectedClearText()
        {
            // arrange
            AESCryptographyService cryptographyService = ArrangeAESCryptographyService();

            // act
            string clearText = cryptographyService.Decrypt(Cryptogram);

            // assert
            Assert.AreEqual(ClearText, clearText);
        }
    }
}
