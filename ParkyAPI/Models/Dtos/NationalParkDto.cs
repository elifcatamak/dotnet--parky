using System;

namespace ParkyAPI.Models.Dtos
{
    public class NationalParkDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string State { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime EstablishDate { get; set; }
    }
}