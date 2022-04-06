public class ProductItem
{
    // Inicializace proměnných - getterů a setterů
    public int Id { get; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public decimal VatRate { get; set; }
    public int AvailableAmount { get; set; }

    // Defaultní konstruktor, který naplní třídu prázdnými daty, pokud je konstruktor zavolán bez parametrů
    public ProductItem()
    {
        this.Id = 0;
        this.Name = "empty";
        this.Price = 0.00M;
        this.VatRate = 0;
        this.AvailableAmount = 0;
    }

    // Konstruktor pro případ, kdy je třáda instanciována s parametry
    public ProductItem(int id, string name, decimal price, decimal vatRate, int availableAmount)
    {
        this.Id = id;
        this.Name = name;
        this.Price = price;
        this.VatRate = vatRate;
        this.AvailableAmount = availableAmount;
    }

    // Přepsání defaultní funkce, pro hezký výpis objektu a jeho vlastností do konzole
    override public string ToString() {
        return this.Id + ": " + this.Name + " " + (this.Price * (1 + (this.VatRate / 100))) + "CZK " + this.AvailableAmount;
    }
}
