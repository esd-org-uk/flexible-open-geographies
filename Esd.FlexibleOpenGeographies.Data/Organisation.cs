using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Serializable]
    [DataContract(Name = "user")]
    [Table("organisation")]
    public class Organisation
    {
        [DataMember(Name = "identifier")]
        [Key, Column("organisation_id", TypeName = "varchar"), MaxLength(255)]
        public string OrganisationId { get; set; }
        [DataMember(Name = "label")]
        [Column("organisation_name", TypeName = "varchar"), MaxLength(255)]
        public string OrganisationName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
