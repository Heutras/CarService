﻿using System.ComponentModel.DataAnnotations.Schema;

namespace CarService.Models
{
    public class ServiceShoppingCart
    {

        public int Id { get; set; }
        public int CarId { get; set; }
        public int ServiceTypeId { get; set; }

        [ForeignKey("CarId")]
        public virtual Car Car { get; set; }

        [ForeignKey("ServiceTypeId")]
        public virtual ServiceType ServiceType { get; set; }
    }
}
