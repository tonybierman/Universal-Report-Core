using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniversalReportCore
{
    public interface ICohort
    {
        int Id { get; set; }

        string Name { get; set; }
    }
}
