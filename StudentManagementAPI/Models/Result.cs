using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementAPI.Models;

[Table("Result")]
public class Result
{
    [Key]
    [Column("result_id")]
    public int ResultId { get; set; }


[Required]
    [Column("enrolment_id")]
    public int EnrolmentId { get; set; }

    [Column("finish_time", TypeName = "time")]
    public TimeSpan? FinishTime { get; set; }

    [Column("finishing_position")]
    public int? FinishingPosition { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("result_status")]
    public string ResultStatus { get; set; } = "Pending";

    [Column("recorded_at", TypeName = "datetime")]
    public DateTime RecordedAt { get; set; }


}

