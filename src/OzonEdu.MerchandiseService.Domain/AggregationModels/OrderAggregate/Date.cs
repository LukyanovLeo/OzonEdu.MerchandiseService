﻿using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAgregate
{
    public class Date : ValueObject
    {
        public Date(DateTime date)
        {
            if (date == default)
                throw new ArgumentNullException("Date is not set");

            Value = date.Date;
        }

        public DateTime Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}