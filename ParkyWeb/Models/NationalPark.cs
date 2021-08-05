using System;
using System.ComponentModel.DataAnnotations;

namespace ParkyWeb.Models
{
    public class NationalPark
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string State { get; set; }

        public byte[] Image { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime EstablishDate { get; set; }
    }
}