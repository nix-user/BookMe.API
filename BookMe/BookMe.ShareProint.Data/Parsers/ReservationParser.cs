using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CamlexNET;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Parsers
{
    public class ReservationParser : BaseParser
    {
        public ListItemCollection GetPossibleIntersecting(DateTime intervalStart, DateTime intervalEnd)
        {
            var context = new ClientContext(Constants.BaseAddress);
            var resourcesList = context.Web.Lists.GetByTitle(ListNames.Reservations);

            Expression<Func<ListItem, bool>> recurrentBookingCondition =
                reservation => (bool)reservation["fRecurrence"] && (DateTime)reservation["EndDate"] > DateTime.Now;

            Expression<Func<ListItem, bool>> regularBookingCondition = reservation => !(bool)reservation["fRecurrence"]
            && (DateTime)reservation["EventDate"] < intervalEnd && (DateTime)reservation["EndDate"] > intervalStart;

            var qry = new CamlQuery();
            qry.ViewXml = "<View Scope=\"RecursiveAll\">" + Camlex.Query().WhereAny(new List<Expression<Func<ListItem, bool>>>() { recurrentBookingCondition, regularBookingCondition}) + "</View>";


            ListItemCollection items = resourcesList.GetItems(qry);

            context.Load(items);
            context.ExecuteQuery();

            return items;
        }
    }
}
