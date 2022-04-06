using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ETE15E_PRO_PERGLER
{
    class Program {
        static Storage store = new Storage();

        // Odstranění položek z košíku
        static void DeleteItems() {
            if (store.CartIsEmpty()) {
                Console.WriteLine("Váš nákupní seznam je prázdný!");
            } else {
                try {
                    int productId = 0;
                    Console.Clear();
                    store.ListCartItems();
                    Console.WriteLine("*****************");
                    Console.Write("Pro odstranění zadej ID položky v košíku: ");
                    productId = int.Parse(Console.ReadLine());
                    CartItem item = store.CartList.Where(i => i.Id == productId).FirstOrDefault();
                    if (item != null) {
                        Console.WriteLine("Položka " + item.Product.Name + " byla odstraněna");
                        ProductItem productItem = store.AvailableProduts.Where(i => i.Id == item.Product.Id).FirstOrDefault();
                        if (productItem != null) {
                            productItem.AvailableAmount += item.Amount;
                        }
                        store.CartList.RemoveAll(cartItem => cartItem.Id == item.Id);
                    } else Console.WriteLine("Zadáno neznámé ID.");

                } catch (Exception) {
                    Console.WriteLine("Zadán nepodporovaný parametr.");
                }
            }

            Console.Write("Pro zobrazení menu napiště jakýkoliv znak. ");
            Console.ReadLine();
            Console.Clear();
            ShowMenu();
        }

        // Metoda pro úpravu položek v košíku
        static void EditItems() {
            if(store.CartIsEmpty()) Console.WriteLine("Váš nákupní seznam je prázdný!");
            else { 
                store.ListCartItems();
                try {
                    int amount, productId = 0;
                    Console.Clear();
                    store.ListCartItems();
                    Console.WriteLine("*****************");
                    Console.Write("Pro úpravu zadej id položky v košíku: ");
                    productId = int.Parse(Console.ReadLine());
                    CartItem item = store.CartList.Where(i => i.Id == productId).FirstOrDefault();
                    Console.Write("Zadej nový počet. (Max " + (item.Amount + item.Product.AvailableAmount) + "): ");
                    amount = int.Parse(Console.ReadLine());
                    // Rozhodování, jestli je počet větší, než aktuální, nebo menší. Podle toho se přičítá, nebo odčítá
                    if (amount < item.Amount) {
                        if (amount <= 0) store.CartList.RemoveAll(cartItem => cartItem.Id == item.Id);
                        else {
                            item.Product.AvailableAmount += (item.Amount - amount);
                            item.Amount = amount;
                            Console.WriteLine(item);
                        }
                    } else if (amount > item.Amount) {
                        if (item.Amount + item.Product.AvailableAmount < amount) {
                            Console.WriteLine("Zadaný počet je větší, než maximální možný počet.");
                        } else {
                            item.Product.AvailableAmount = (item.Amount + item.Product.AvailableAmount) - amount;
                            item.Amount = amount;
                            Console.WriteLine(item);
                        }
                    }
                } catch (Exception) {
                    Console.WriteLine("Zadán nepodporovaný parametr.");
                }
            }

            Console.Write("Pro zobrazení menu napiště jakýkoliv znak. ");
            Console.ReadLine();
            Console.Clear();
            ShowMenu();
            return;
        }
            

        static void AddItem() {
            try {
                //Deklarace proměnných použitých pro nákup 
                int productId = 0;
                int amount = 0;
                string again;
                do {
                    Console.Clear();
                    store.ListAvailableItems();
                    Console.WriteLine("*****************");
                    Console.Write("Pro nákup zadej ID vybraného zboží: ");
                    productId = int.Parse(Console.ReadLine());
                    // Najdi produk v listu podle zadaného ID
                    ProductItem item = store.AvailableProduts.Where(i => i.Id == productId).FirstOrDefault();
                    if (item != null && item.AvailableAmount > 0)
                    {
                        Console.WriteLine("Vybráno: " + item.Name);
                        Console.Write("Zadej počet: ");
                        amount = int.Parse(Console.ReadLine());
                        // Zkontroluj, jestli je zadaný počet dostupný
                        if (amount > 0 && amount <= item.AvailableAmount)
                        {
                            item.AvailableAmount -= amount;
                            // Vyhledání aktuální položky v košíku pro její editaci, nebo přidání
                            CartItem currentCartItem = store.CartList.Where(item => item.Product.Id == productId).FirstOrDefault();
                            if (currentCartItem == null) store.CartList.Add(new CartItem(item, amount));
                            else currentCartItem.Amount += amount;
                            store.ListCartItems();
                        }
                        else if (amount > item.AvailableAmount) Console.WriteLine("Maximální dostupný počet " + item.Name + " je " + item.AvailableAmount + ". Položka nebude přidána.");
                        else Console.WriteLine("Prosíme zadejte kladný počet položek. Položka nebude přidána");
                    }
                    else if (item != null && item.AvailableAmount <= 0) Console.WriteLine("Položka " + item.Name + " již není na skladě. Položka nebude přidáná.");
                    else Console.WriteLine("Zadáno neznámé ID.");

                    Console.Write("Chceš dále nakupovat? [A/N] ");
                    again = Console.ReadLine();
                    // Převod na velké písmeno pro předejití typografické chyby
                } while (again.ToUpper() == "A");
                Console.Clear();
                ShowMenu();
            }
            catch (Exception)
            {
                Console.Write("Nastala chyba při přidávání produktu do košíku! Pro pokračování stiskněte klávesu.");
                Console.ReadLine();
                Console.Clear();
                ShowMenu();
            }
        }
        static void EndApplication() {
            Console.Write("********************\nChceš se vrátit zpět do menu [A] nebo ukončit aplikaci [N]? [A/N]: ");
            try {
                //Nahrání hodnoty do proměné a převod hodnoty na velká písmena, čili je možnost zadat jak velká tak i malá 
                string decision = Console.ReadLine().ToUpper();

                //Příkaz If vyhodnocuje zadanou hodnotu, pokud se vyhodnotí jako "A" je vymazána konzole a uživatel je přesměrován do Menu. 
                if (decision == "A") {
                    Console.Clear();
                    ShowMenu();
                }
                //Pokud se vyhodnotí jako "N" program se následně ukončí
                else if (decision == "N") Environment.Exit(0); //Příkaz sloužící pro ukončení
                //Tento příkaz zde slouží proto aby se nemohlo zadat cokoliv jiného než chceme
                else {
                    Console.WriteLine("Zadána nepodporovaná volba. Zkuste to prosím znovu.");
                    EndApplication();
                }
            }
            catch (Exception) {
                Console.WriteLine("Zadán chybný vstupní parametr.");
            }
        }

        static void ClearCart() {
            // Vyprázdni košík
            store.EmptyCart();
            Console.Clear();
            ShowMenu();
        }

        static void ShowMenu()
        {
            Console.WriteLine("*****************************\nESHOP CZU\n*****************************\n");
            Console.WriteLine("Přidání zboží do košíku: [A]");
            Console.WriteLine("Vypsání obsahu košíku: [L]");
            Console.WriteLine("Vytištění účtenky: [P]");
            Console.WriteLine("Odstranit zboží košiku: [D]");
            Console.WriteLine("Vyprázdnit košík: [R]");
            Console.WriteLine("Upravit zboží v košiku: [E]");
            Console.WriteLine("*******************************************");
            try {
                Console.Write("Zadej volbu: ");
                char decision = char.Parse(Console.ReadLine());
                // Převeď vstupní parametr na malé písmeno, aby se předešlo typografickým chybám
                switch (char.ToLower(decision)) {
                    case 'a':
                        AddItem();
                        break;
                    case 'l':
                        ListItems();
                        break;
                    case 'p':
                        PrintItems();
                        break;
                    case 'd':
                        DeleteItems();
                        break;
                    case 'e':
                        EditItems();
                        break;
                    case 'r':
                        ClearCart();
                        break;
                    default:
                        // Pokud zadaný parametr není podporován, tak se vykoná.
                        Console.WriteLine("Nepodporovaná volba.");
                        EndApplication();
                        break;
                }
            }
            catch (Exception) {
                Console.WriteLine("Zadán chybný vstupní parametr!!!");
                EndApplication();
            }
        }

        static void ListItems() {
            // Kontrola, jestli není košík prázdný
            if (store.CartIsEmpty()) Console.WriteLine("Váš nákupní seznam je prázdný!");
            else store.ListCartItems();
            Console.Write("Pro zobrazení menu napiště jakýkoliv znak. ");
            Console.ReadLine();
            Console.Clear();
            ShowMenu();           
        }

        static void PrintItems() {
            // Kontrola, jestli není košík prázdný
            if (store.CartIsEmpty()) {
                Console.WriteLine("Váš nákupní seznam je prázdný!");
                Console.Write("Pro zobrazení menu napiště jakýkoliv znak. ");
            } else {
                store.PrintCart();
            }
           
            Console.ReadLine();
            Console.Clear();
            ShowMenu();
        }



        static void Main(string[] args)
        {
            // Zobraz menu při prvním spuštění
            ShowMenu();
        }
    }
}