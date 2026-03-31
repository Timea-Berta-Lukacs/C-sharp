using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Models
{
    public record CardDetails(UserCardNumber UserCardNumber, UserCardCVV UserCardCVV, UserCardExpiryDate UserCardExpiryDate, UserCardBalance UserCardBalance, bool ToUpdate);
}