using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Auth.Cryptography.Abstract
{
    public interface ISimetricCryptographyKeyProvider
    {
        string Key { get; }
    }
}
