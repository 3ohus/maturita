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
        // Veřejná vlastnost, kterou si hlavní formulář přečte po zavření tohoto okna
        public CPolozka AktualniPolozka { get; private set; }

        // Pomocná proměnná pro uložení reference, pokud okno otevíráme v režimu editace
        private CPolozka _editovanaPolozka = null;

        // Konstruktor pro PŘIDÁVÁNÍ nové položky
        public FormularPolozky()
        {
            InitializeComponent();
            this.Text = "Přidat novou položku";
            button1.Text = "Přidat";
        }

        // Konstruktor pro EDITACI existující položky
        // polozka je Objekt položky, která se má upravit
        public FormularPolozky(CPolozka polozka) : this()
        {
            _editovanaPolozka = polozka;
            this.Text = "Upravit položku";
            button1.Text = "Uložit změny";

            // Naplnění ovládacích prvků daty z objektu
            textBox1.Text = polozka.Number.ToString();
            textBox2.Text = polozka.Name;
            comboBox1.Text = polozka.Category;
            textBox3.Text = polozka.Quantity.ToString();
            comboBox2.Text = polozka.Unit;
            comboBox3.Text = polozka.Placement;
            textBox4.Text = polozka.Minimum.ToString();

            // Cena se vypisuje s tečkou (InvariantCulture), aby se s ní lépe pracovalo při ukládání
            textBox5.Text = polozka.Price.ToString(CultureInfo.InvariantCulture);

            // Při editaci zakážeme změnu ID (Číslo zboží), aby se nerozbily vazby v JSONu
            textBox1.Enabled = false;
        }

        // Tlačítko OK (Uložit/Přidat)
        private void button1_Click(object sender, EventArgs e)
        {
            if (ZkontrolujData())
            {
                PripravData();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        // Tlačítko Storno
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Validace vstupů: kontroluje prázdná pole, formát čísel a záporné hodnoty
        public bool ZkontrolujData()
        {
            // kontrola prázdných polí a záporných čísel
            var textboxes = new[] { textBox1, textBox2, textBox3, textBox4, textBox5 };
            if (textboxes.Any(tb => string.IsNullOrWhiteSpace(tb.Text)))
            {
                MessageBox.Show("Vyplňte vše!");
                return false;
            }

            // kontrola že číslo zboží je kladné a že se nejedná o duplicitní ID (při přidávání)

 
            if (int.TryParse(textBox1.Text, out int zadaneCislo))
            {

                string path = System.IO.Path.Combine(Application.StartupPath, "data.json");

                if (System.IO.File.Exists(path))
                {
                    string json = System.IO.File.ReadAllText(path);
                    var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var seznam = System.Text.Json.JsonSerializer.Deserialize<List<CPolozka>>(json, options) ?? new List<CPolozka>();


                    if (_editovanaPolozka == null)
                    {
                        if (seznam.Any(p => p.Number == zadaneCislo))
                        {
                            MessageBox.Show($"Zboží s číslem {zadaneCislo} už na skladě existuje! Zvolte jiné.", "Duplicitní ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
            }


            return true;
        }

        // Přetvoří data zpět do objektu CPolozka
        private void PripravData()
        {
            // Pokud editujeme, upravíme původní objekt. Pokud ne, vytvoříme nový.
            AktualniPolozka = _editovanaPolozka ?? new CPolozka();

            AktualniPolozka.Number = int.Parse(textBox1.Text.Trim());
            AktualniPolozka.Name = textBox2.Text.Trim();
            AktualniPolozka.Category = comboBox1.Text.Trim();
            AktualniPolozka.Quantity = int.Parse(textBox3.Text.Trim());
            AktualniPolozka.Unit = comboBox2.Text.Trim();
            AktualniPolozka.Placement = comboBox3.Text.Trim();
            AktualniPolozka.Minimum = int.Parse(textBox4.Text.Trim());

            // Ošetření ceny: sjednotíme čárky na tečky a parsujeme nečeským formátem (InvariantCulture)
            // Tím zajistíme, že se data uloží vždy správně.
            string finalniCenaText = textBox5.Text.Trim().Replace(',', '.');
            AktualniPolozka.Price = float.Parse(finalniCenaText, CultureInfo.InvariantCulture);
        }
    }
}