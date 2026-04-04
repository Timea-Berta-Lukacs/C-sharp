using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LanguageExt;
using Project.Domain.Exceptions;
using System.Text.RegularExpressions;
using static LanguageExt.Prelude;

namespace Project.Domain.Models
{
    public record OrderNumber
    {
        public static readonly Regex PatternRegex = new("^ORD[0-9]{6}$");

        public string Value { get; }

        public OrderNumber(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidOrderNumberException($"{value} is an invalid order number.");
            }
        }

        private static bool IsValid(string stringValue) => PatternRegex.IsMatch(stringValue);

        public override string ToString()
        {
            return Value;
        }

        public static bool TryParse(string stringValue, out OrderNumber orderNumber)
        {
            bool isValid = false;
            orderNumber = null;

            if (IsValid(stringValue))
            {
                isValid = true;
                orderNumber = new(stringValue);
            }

            return isValid;
        }

        public static Option<OrderNumber> TryParse(string orderNumber)
        {
            if (IsValid(orderNumber))
            {
                return Some<OrderNumber>(new(orderNumber));
            }
            else
            {
                return None;
            }
        }
    }
}
