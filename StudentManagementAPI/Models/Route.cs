using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementAPI.Models;

[Table("Route")]
public class Route
{
    [Key]
    [Column("route_id")]
    public int RouteId { get; set; }


[Required]
    [Column("event_id")]
    public int EventId { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("route_name")]
    public string RouteName { get; set; } = string.Empty;

    [Column("distance_km", TypeName = "decimal(6,2)")]
    public decimal DistanceKm { get; set; }

    [MaxLength(500)]
    [Column("route_description")]
    public string? RouteDescription { get; set; }

    [MaxLength(500)]
    [Column("route_map_url")]
    public string? RouteMapUrl { get; set; }


}
