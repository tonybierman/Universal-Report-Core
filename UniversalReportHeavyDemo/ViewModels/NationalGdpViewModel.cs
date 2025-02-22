﻿using UniversalReportCore.ViewModels;

namespace UniversalReportHeavyDemo.ViewModels
{
    public class NationalGdpViewModel : IEntityViewModel<int>
    {
        public int Id { get; set; }  // Primary Key
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
        public int? Year { get; set; }
        public double? Value { get; set; }
    }
}
