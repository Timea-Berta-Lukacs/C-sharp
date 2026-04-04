using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Project.Domain.Exceptions;
using static LanguageExt.Prelude;

namespace Project.Domain.Models
{
    public record ProductName
    {
        private static readonly Regex ValidPattern = new("^[a-zA-Z0-9]+(?:\\s[a-zA-Z0-9]+)?$");

        public string Value { get; set; }

        public ProductName(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductNameException("");
            }
        }

        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

        public override string ToString()
        {
            return Value;
        }

        public static Option<ProductName> TryParse(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<ProductName>(new(stringValue));
            }
            else
            {
                return None;
            }
        }
    }
}
