using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMe.WebApi.Models
{
    public class ResponseModel<T> : ResponseModel
    {
        public T Result { get; set; }
    }
}