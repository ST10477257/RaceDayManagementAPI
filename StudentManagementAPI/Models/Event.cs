using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementAPI.Models;

[Table("Event")]
public class Event
{
    [Key]
    [Column("event_id")]
    public int EventId { get; set; }


[Required]
    [Column("organiser_id")]
    public int OrganiserId { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("event_name")]
    public string EventName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("event_date", TypeName = "date")]
    public DateTime EventDate { get; set; }

    [Column("start_time", TypeName = "time")]
    public TimeSpan StartTime { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("location")]
    public string Location { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "Upcoming";


}
