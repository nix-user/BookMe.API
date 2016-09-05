using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Models;

namespace BookMe.ShareProint.Data.Services.Abstract
{
    public interface IResourceService
    {
        IEnumerable<Resource> GetAll();
    }
}
