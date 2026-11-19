public class Emprestimo
{
    public int Id {get;set;}
    public decimal Valor {get;set;}
    public int Parcelas {get;set;}
    public DateTime DataContrato {get;set;}
    public Cliente? Cliente {get;set;}
    public int ClienteId {get;set;}
}