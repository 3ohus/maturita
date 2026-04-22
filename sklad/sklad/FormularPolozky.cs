using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using System.Globalization;

namespace sklad
{
    public partial class FormularPolozky : Form
    {
        // Vlastnost pro předání dat zpět do hlavního okna
        public CPolozka AktualniPolozka { get; private set; }
        
        // Interní reference pro režim editace
        private readonly CPolozka _puvodniPolozka;
        private readonly bool _jeEditace;

        // Konstruktor pro PŘIDÁNÍ
        public FormularPolozky()
        {
            InitializeComponent();
            _jeEditace = false;
            SetupUI("Přidat novou položku", "Přidat");
        }

        // Konstruktor pro EDITACI
        public FormularPolozky(CPolozka polozka) : this()
        {
            _puvodniPolozka = polozka;
            _jeEditace = true;
            SetupUI("Upravit položku", "Uložit změny");
            NaplnPole(polozka);
        }

        private void SetupUI(string title, string buttonText)
        {
            this.Text = title;
            button1.Text = buttonText;
            button2.Text = "Storno";
        }

        private void NaplnPole(CPolozka p)
        {
            textBox1.Text = p.Number.ToString();
            textBox2.Text = p.Name;
            comboBox1.Text = p.Category;
            textBox3.Text = p.Quantity.ToString();
            comboBox2.Text = p.Unit;
            comboBox3.Text = p.Placement;
            textBox4.Text = p.Minimum.ToString();
            textBox5.Text = p.Price.ToString(CultureInfo.InvariantCulture);
            
            // Při editaci nepovolíme změnu ID
            textBox1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Pokusíme se o kompletní validaci a sestavení objektu
            if (ValidovatASestavit())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidovatASestavit()
        {
            // 1. Kontrola prázdných vstupů
            if (PoleJsouPrazdna())
            {
                ZobrazChybu("Všechna pole musí být vyplněna.");
                return false;
            }

            // 2. Parsování čísel (TryParse zabrání Exception při zadání znaků jako '-', 'abc' atd.)
            if (!int.TryParse(textBox1.Text.Trim(), out int id) || id < 0)
                return ZobrazChybu("Číslo zboží musí být kladné celé číslo.");

            if (!int.TryParse(textBox3.Text.Trim(), out int mnozstvi) || mnozstvi < 0)
                return ZobrazChybu("Množství musí být nezáporné celé číslo.");

            if (!int.TryParse(textBox4.Text.Trim(), out int minZasoba) || minZasoba < 0)
                return ZobrazChybu("Minimální zásoba musí být nezáporné celé číslo.");

            // Cena: nahradíme čárku tečkou pro InvariantCulture
            string cenaText = textBox5.Text.Trim().Replace(',', '.');
            if (!float.TryParse(cenaText, NumberStyles.Any, CultureInfo.InvariantCulture, out float cena) || cena < 0)
                return ZobrazChybu("Cena musí být kladné číslo (formát 10.50).");

            // 3. Kontrola duplicity ID v JSONu (pouze pokud přidáváme novou)
            if (!_jeEditace && ExistujeIDVDatabazi(id))
            {
                return ZobrazChybu($"Zboží s číslem {id} již v systému existuje.");
            }

            // 4. Pokud jsme se dostali až sem, data jsou validní a můžeme bezpečně vytvořit objekt
            AktualniPolozka = _jeEditace ? _puvodniPolozka : new CPolozka();
            
            AktualniPolozka.Number = id;
            AktualniPolozka.Name = textBox2.Text.Trim();
            AktualniPolozka.Category = comboBox1.Text.Trim();
            AktualniPolozka.Quantity = mnozstvi;
            AktualniPolozka.Unit = comboBox2.Text.Trim();
            AktualniPolozka.Placement = comboBox3.Text.Trim();
            AktualniPolozka.Minimum = minZasoba;
            AktualniPolozka.Price = cena;

            return true;
        }

        private bool PoleJsouPrazdna()
        {
            return string.IsNullOrWhiteSpace(textBox1.Text) ||
                   string.IsNullOrWhiteSpace(textBox2.Text) ||
                   string.IsNullOrWhiteSpace(textBox3.Text) ||
                   string.IsNullOrWhiteSpace(textBox4.Text) ||
                   string.IsNullOrWhiteSpace(textBox5.Text);
        }

        private bool ExistujeIDVDatabazi(int id)
        {
            string path = Path.Combine(Application.StartupPath, "data.json");
            if (!File.Exists(path)) return false;

            try
            {
                string json = File.ReadAllText(path);
                var polozky = JsonSerializer.Deserialize<List<CPolozka>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return polozky?.Any(p => p.Number == id) ?? false;
            }
            catch
            {
                return false; // V případě chyby čtení neblokujeme přidání, nebo logujeme
            }
        }

        private bool ZobrazChybu(string zprava)
        {
            MessageBox.Show(zprava, "Chyba zadání", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
    }
}
