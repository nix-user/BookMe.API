using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Auth.Cryptography.Abstract;

namespace BookMe.Auth.Cryptography.Concrete
{
    public class AppConfigSimetricCryptographyKeyProvider : ISimetricCryptographyKeyProvider
    {
        public string Key => ConfigurationManager.AppSettings["SymmetricCryptographyKey"];
    }
}
