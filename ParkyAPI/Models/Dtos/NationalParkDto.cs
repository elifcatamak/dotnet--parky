using System;
using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.Dtos
{
    public class NationalParkDto
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string State { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime EstablishDate { get; set; }
    }
}