using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementAPI.Models;

[Table("EventCategory")]
public class EventCategory
{
    [Key]
    [Column("event_category_id")]
    public int EventCategoryId { get; set; }


[Required]
    [Column("event_id")]
    public int EventId { get; set; }

    [Required]
    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("entry_fee", TypeName = "decimal(10,2)")]
    public decimal EntryFee { get; set; }

    [Column("participant_limit")]
    public int ParticipantLimit { get; set; }


}

