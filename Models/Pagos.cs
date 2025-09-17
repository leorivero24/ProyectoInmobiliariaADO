public class Pago
{
    public int Id { get; set; }
    public int ContratoId { get; set; }
    
    // Cambiar esto:
    public DateTime Fecha { get; set; }

    public decimal Monto { get; set; }   // o Importe si quieres igualar tabla
    public string? MedioPago { get; set; }
    public string? Observaciones { get; set; }
}
