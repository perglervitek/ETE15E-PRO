using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ETE15E_PRO_PERGLER
{
    class Storage {
        // List pro dostupné položky nahrané z .eshop.csv
        public List<ProductItem> AvailableProduts { get; private set; }

        // List pro položky košíku
        public List<CartItem> CartList { get; set; }

        // Cesta k souboru .eshop.csv
        private const string CSV_FILE_LOCATION = "./eshop.csv";
        // Testovací data, která budou nahrána do souboru .eshop.csv, pokud neexistuje
        private const string TEST_DATA = "#;Název itemu;Cena;DPH;\n1; Mléko 1l 500g; 10; 15; 100;\n2; Ovesné vločky; 10; 15; 50;\n3; Sýr 100g; 10; 18; 30;\n4; Šunka; 20; 17; 40;\n5; Salám; 29; 21; 38;\n6; Párek; 5; 21; 64;\n7; Horčice; 30; 21; 37;\n8; Kečup; 60; 21; 58;\n9; Bílý jogurt; 20; 21; 17;\n10; Coca - cola; 35; 21; 28;\n11; Fanta; 35; 21; 28;\n12; Sprite; 25; 21; 28;\n13; Džus; 15; 21; 28;\n14; Pizza; 80; 21; 28;\n15; Tuňák; 350; 21; 25;\n16; Losos; 300; 21; 28;\n17; Kapr; 110; 10; 24;\n18; Další ryba; 220; 21; 28;\n19; Prase; 350; 21; 27;\n20; Sešit; 5; 21; 28;";

        // Vrací celkový součet v košíku včetně DPH
        public decimal Checkout() {
            // Inicializace proměnné
            decimal total = 0;
            foreach (var item in CartList) {
                total += item.Product.Price * item.Amount * (1 + (item.Product.VatRate / 100));
            }
            return total;
        }

        // Nahraj položky ze souboru .eshop.csv do listu AvailableProduts
        private void GetAvailableItemsFromFile() {
            if(!File.Exists(CSV_FILE_LOCATION)) GenerateTestItemsFile();
            try {
                using (var reader = new StreamReader(CSV_FILE_LOCATION)) {
                    while (!reader.EndOfStream) {
                        var line = reader.ReadLine();
                        if (line.StartsWith("#")) continue;
                        var values = line.Split(';');
                        AvailableProduts.Add(new ProductItem(int.Parse(values[0]), values[1], decimal.Parse(values[2]), int.Parse(values[3]), int.Parse(values[4])));
                    }
                }
            } catch (Exception) { 
                Console.WriteLine("Nastala chyba při načítání .csv souboru.");
            }
        }

        // Vygeneruj soubor .eshop.csv
        private void GenerateTestItemsFile() {
            try {
                using var sw = new StreamWriter("./eshop.csv");
                sw.WriteLine(TEST_DATA);
                sw.Close();
            } catch (Exception) {
                Console.WriteLine("Nastala chyba při vytváření .csv souboru.");
            }
        }

        // Vypiš všechny dostupné produkty. Používá se ProductItem ToString
        public void ListAvailableItems() {
            foreach (ProductItem item in AvailableProduts) {
                Console.WriteLine(item);
            }
        }

        // Vypiš všechny produkty v košíku. Používá se CartItem ToString a teké metoda Checkout pro vypsání součtu
        public void ListCartItems() {
            foreach (CartItem item in CartList) {
                Console.WriteLine(item);
            }
            Console.WriteLine($"Celková cena: {Checkout()} CZK");
        }

        // Vracení True/False, jestli je košík prázdný, nebo ne
        public bool CartIsEmpty() {
            return !CartList.Any();
        }

        // Odstraň všechny položky v košíku a vrať počet zboží v košíku zpět produktům
        public void EmptyCart() {
            try {
                foreach (CartItem item in CartList) {
                    ProductItem productItem = AvailableProduts.Where(i => i.Id == item.Product.Id).FirstOrDefault();
                    if (productItem != null) {
                        productItem.AvailableAmount += item.Amount;
                    }
                }
                CartList = new List<CartItem>();
                Console.Write("Košík byl vyprázdněn. Pro pokračování stiskněte klávesu.");
                Console.ReadLine();
            } catch (Exception) {
                Console.WriteLine("Nastala chyba při odstraňování účtenky.");
            }
        }

        // Vytištění účtenky, používá se TextWriter, aby bylo možno metodu volat s výpisem na jiná místa (Konzole, Soubor)
        public void ShowReceipt(TextWriter output) {
            output.WriteLine("***********************************");
            output.WriteLine("{0,28}", "Zemědělský obchodník");
            output.WriteLine("{0,21}", "Zemědělská 27 110 00");
            output.WriteLine("{0,34}", "IČO: 123456789   DIČ: CZ123456789");
            output.WriteLine("{0,27}", "Číslo dokladu: " + 588);
            output.WriteLine("***********************************");
            output.Write("Provozovna: " + 1);
            output.WriteLine("{0,22}", "Pokladna: " + 15);
            output.WriteLine("Datum: " + DateTime.Now.ToString("dd/MM/yyyy"));
            output.WriteLine("Čas: " + DateTime.Now.ToString("HH:mm:ss"));
            output.WriteLine("Režim tržby:   Běžný");
            output.WriteLine("{0,17}", $"FIK: {CodeGenerator(12)}");
            output.WriteLine("{0,17}", $"BKP: {CodeGenerator(12)}");
            output.WriteLine("***********************************");
            foreach (CartItem item in CartList) {
                output.WriteLine(item);
            }
            output.WriteLine("***********************************");
            output.WriteLine("Celková cena: " + Checkout() + " CZK");
        }

        // Generování kódů BKP a FIK
        private string CodeGenerator(int length) {
            Random res = new Random();
            string str = "abcdefghijklmnopqrstuvwxyz0123456789";
            string randomstring = "";
            for (int i = 0; i < length; i++) {
                randomstring = randomstring + str[res.Next(str.Length)];
            }
            return randomstring;
        }

        public void PrintCart() { 
            ShowReceipt(Console.Out);
            using var sw = new StreamWriter("./uctenka.txt");
            ShowReceipt(sw);
            sw.Close();
            CartList = new List<CartItem>();
        }

        // Vytiskni aktuální košík
        public Storage() {
            AvailableProduts = new List<ProductItem>();
            GetAvailableItemsFromFile();
            CartList = new List<CartItem>();
        }
    }
}
