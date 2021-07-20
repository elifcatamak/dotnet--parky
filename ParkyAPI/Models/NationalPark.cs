using System;
using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models
{
    //Missing xml comment
#pragma warning disable 1591
    public class NationalPark
    {
        [Key] public int Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string State { get; set; }

        public byte[] Image { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime EstablishDate { get; set; }
    }
#pragma warning restore 1591
}