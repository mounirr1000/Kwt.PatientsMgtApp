using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Kwt.PatientsMgtApp.Core.Models;


namespace Kwt.PatientsMgtApp.Core.Infrastructure
{

    public class RequiredIfAttribute : ValidationAttribute
    {

        private readonly RequiredAttribute _innerAttribute = new RequiredAttribute();
        public string DependentUpon { get; set; }
        public object Value { get; set; }

        public RequiredIfAttribute(string dependentUpon, object value)
        {
            this.DependentUpon = dependentUpon;
            if ((bool) value)
            {
                this.Value = value;
            }
            else this.Value = null;
        }

        public RequiredIfAttribute(string dependentUpon)
        {
            this.DependentUpon = dependentUpon;
            this.Value = null;
        }

        public override bool IsValid(object value)
        {
             return _innerAttribute.IsValid(value);
        }
        
    }
   
}
