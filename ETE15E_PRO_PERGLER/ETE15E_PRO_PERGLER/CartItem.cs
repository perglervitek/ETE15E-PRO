using System.Threading;

public class CartItem
{
    static int nextId;
    public int Id { get; private set; }
    public ProductItem Product {get; set;}

    public int Amount { get; set; }

    // Defaultní konstruktor, který naplní třídu prázdnými daty, pokud je konstruktor zavolán bez parametrů
    public CartItem() { 
        this.Id = Interlocked.Increment(ref nextId);
        this.Product = new ProductItem();
        this.Amount = 0;
    }

    // Konstruktor pro případ, kdy je třáda instanciována s parametry
    public CartItem(ProductItem product, int amount)
    {
        this.Id = Interlocked.Increment(ref nextId);
        this.Product = product;
        this.Amount = amount;
    }

    // Metoda přepisující defaultní výpis, aby byl objekt vypsán v lepším formátu
    override public string ToString() {
        return this.Id + " " + this.Product.Name + " " + (this.Product.Price*(1 + (this.Product.VatRate/100))) + "CZK " + this.Amount + "x";
    }
}
