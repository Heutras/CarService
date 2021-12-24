using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarService.Models
{
    public class ServiceHeader
    {
        public int Id { get; set; }
        public double Miles { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        public string Details { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}")]
        public DateTime DateAdded { get; set; }

        public int CarId { get; set; }

        [ForeignKey("CarId")]
        public virtual Car Car { get; set; }

        public static explicit operator List<object>(ServiceHeader v)
        {
            throw new NotImplementedException();
        }
    }
}
