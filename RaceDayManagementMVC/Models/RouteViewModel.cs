namespace RaceDayManagementMVC.Models
{
    public class RouteViewModel
    {
        public int RouteId { get; set; }
        public int EventId { get; set; }
        public string RouteName { get; set; } = string.Empty;
        public decimal DistanceKm { get; set; }
        public string? RouteDescription { get; set; }
        public string? RouteMapUrl { get; set; }
    }
}
