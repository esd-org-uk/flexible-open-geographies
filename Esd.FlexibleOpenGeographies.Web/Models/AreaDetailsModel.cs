using Esd.FlexibleOpenGeographies.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class AreaDetailsModel
    {
        public int Id { get; set; }
        [DisplayName("Area type")]
        public AreaTypeBasic AreaType { get; set; }
        public string Code { get; set; }
        public string Label { get; set; }
        [DisplayName("Alternate labels")]
        public IList<string> AlternateLabels { get; set; }
        public string Creator { get; set; }
        public string CreatorDescription { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Colour { get; set; }
        public bool Editable { get; set; }
        [DisplayName("Same as link"), Url]
        public string SameAsLink { get; set; }
        [DisplayName("URI link"), Url]
        public string URILink { get; set; }
    }
}