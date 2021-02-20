using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Enumerations
{
    public enum ResponseEnum
    {
        [EnumDisplay(Name = "Approved Or Completed Successfully", Description = "Transacion or operation was successful")]
        ApprovedOrCompletedSuccesfully = 00,

        [EnumDisplay(Name = "Pending", Description = "Status of transacion or operation is pending")]
        PendingStatus = 01,

        [EnumDisplay(Name = "Failed", Description = "Transaction or operation failed")]
        Failure = 02,
        [EnumDisplay(Name = "Bad Request - Invalid Amount", Description = "Invalid request data - Amount")]
        BadRequest = 03,

        [EnumDisplay(Name = "System Malfunction ", Description = "System malfunction ")]
        SystemMalfunction = 96,
    }
}
