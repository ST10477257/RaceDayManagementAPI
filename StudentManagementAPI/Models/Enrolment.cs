using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementAPI.Models;

[Table("Enrolment")]
public class Enrolment
{
    [Key]
    [Column("enrolment_id")]
    public int EnrolmentId { get; set; }


[Required]
    [Column("participant_id")]
    public int ParticipantId { get; set; }

    [Required]
    [Column("event_category_id")]
    public int EventCategoryId { get; set; }

    [Column("enrolment_date", TypeName = "datetime")]
    public DateTime EnrolmentDate { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("enrolment_status")]
    public string EnrolmentStatus { get; set; } = "Registered";


}

