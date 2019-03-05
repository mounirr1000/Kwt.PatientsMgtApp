using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Interfaces;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class BaseEntity : IDomainObject
    {
        public int Id { get; set; }

        //[DataMember(Name = "CreatedDate")]
        //private string CreatedDateForSerialization;
        //public DateTime CreatedDate { get; set; }

        //[DataMember(Name = "ModifiedDate")]
        //private string ModifiedDateForSerialization;
        //public DateTime? ModifiedDate { get; set; }


        //[OnSerializing]
        //void OnSerializing(StreamingContext ctx)
        //{
        //    if (this.CreatedDate != DateTime.MinValue)
        //    {
        //        this.CreatedDateForSerialization = this.CreatedDate.ToString("d");
        //            //.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
        //    }

        //    if (this.ModifiedDate.HasValue && this.ModifiedDate.Value != DateTime.MinValue)
        //    {
        //        this.ModifiedDateForSerialization = this.ModifiedDate.Value.ToString("d");//.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
        //    }
        //}

        //[OnDeserializing]
        //void OnDeserializing(StreamingContext ctx)
        //{
        //    this.CreatedDateForSerialization = "1900-01-01";
        //    this.ModifiedDateForSerialization = "1900-01-01";
        //}

        //[OnDeserialized]
        //void OnDeserialized(StreamingContext ctx)
        //{
        //    DateTime createdDate;
        //    if (DateTime.TryParse(this.CreatedDateForSerialization, CultureInfo.InvariantCulture, DateTimeStyles.None, out createdDate))
        //    {
        //        this.CreatedDate = createdDate;
        //    }

        //    DateTime modifiedDate;
        //    if (DateTime.TryParse(this.ModifiedDateForSerialization, CultureInfo.InvariantCulture, DateTimeStyles.None, out modifiedDate))
        //    {
        //        this.ModifiedDate = modifiedDate;
        //    }
        //}
    }
}
