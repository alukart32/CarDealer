namespace CarDealer.Models.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Car")]
    public partial class Car
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Car()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int car_id { get; set; }

        [Required]
        [StringLength(30)]
        public string model { get; set; }

        [Required]
        [StringLength(20)]
        public string type { get; set; }

        [Required]
        [StringLength(20)]
        public string manufacturer { get; set; }

        [Column(TypeName = "money")]
        public decimal price { get; set; }

        [Required]
        [StringLength(40)]
        public string country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
