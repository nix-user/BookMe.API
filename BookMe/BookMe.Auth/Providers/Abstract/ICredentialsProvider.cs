using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;

namespace BookMe.Auth.Providers.Abstract
{
    public interface ICredentialsProvider
    {
        CredentialsDTO Credentials { get; }
    }
}
