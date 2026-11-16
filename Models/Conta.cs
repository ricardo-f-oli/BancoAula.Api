using System.ComponentModel.DataAnnotations;
public class Conta{
    public int Id {get; set; }
    [Required]
    public int ClienteId {get; set;}
    [Required]
    public int Numero {get; set;}

    [Required]
    public string Tipo {get; set;} = "Corrente";
    
    [Range(0, double.MaxValue, ErrorMessage = "O saldo não pode ser negativo.")]
    public decimal Saldo {get; set; }
    public bool Ativa {get; set;} = true;
}