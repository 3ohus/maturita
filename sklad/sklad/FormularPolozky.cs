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
        public CPolozka AktualniPolozka { get; private set; }
        private CPolozka _editovanaPolozka = null;

        public FormularPolozky()
        {
            InitializeComponent();
            this.Text = "Přidat novou položku";
            button1.Text = "Přidat";
        }

        public FormularPolozky(CPolozka polozka) : this()
        {
            _editovanaPolozka = polozka;
            this.Text = "Upravit položku";
            button1.Text = "Uložit změny";

            textBox1.Text = polozka.Number.ToString();
            textBox2.Text = polozka.Name;
            comboBox1.Text = polozka.Category;
            textBox3.Text = polozka.Quantity.ToString();
            comboBox2.Text = polozka.Unit;
            comboBox3.Text = polozka.Placement;
            textBox4.Text = polozka.Minimum.ToString();
            textBox5.Text = polozka.Price.ToString(CultureInfo.InvariantCulture);

            textBox1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ZkontrolujData())
            {
                PripravData();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public bool ZkontrolujData()
        {
            // 1. Kontrola prázdných polí
            var textboxes = new[] { textBox1, textBox2, textBox3, textBox4, textBox5 };
            if (textboxes.Any(tb => string.IsNullOrWhiteSpace(tb.Text)))
            {
                MessageBox.Show("Všechna pole musí být vyplněna!", "Chyba validace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 2. Validace číselných formátů (TryParse)
            // Používáme dočasné proměnné, abychom ověřili, že parsování projde
            if (!int.TryParse(textBox1.Text, out int n) || n < 0)
            {
                MessageBox.Show("Číslo zboží musí být celé kladné číslo!", "Chyba formátu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!int.TryParse(textBox3.Text, out int q) || q < 0)
            {
                MessageBox.Show("Množství musí být celé nezáporné číslo!", "Chyba formátu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!int.TryParse(textBox4.Text, out int m) || m < 0)
            {
                MessageBox.Show("Minimální zásoba musí být celé nezáporné číslo!", "Chyba formátu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // U ceny sjednotíme oddělovač před kontrolou
            string cenaText = textBox5.Text.Trim().Replace(',', '.');
            if (!float.TryParse(cenaText, NumberStyles.Any, CultureInfo.InvariantCulture, out float p) || p < 0)
            {
                MessageBox.Show("Cena musí být platné kladné číslo!", "Chyba formátu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // 3. Kontrola duplicity ID (pouze při přidávání nové položky)
            if (_editovanaPolozka == null)
            {
                int zadaneCislo = int.Parse(textBox1.Text); // Tady už víme, že to projde
                string path = Path.Combine(Application.StartupPath, "data.json");

                if (File.Exists(path))
                {
                    try
                    {
                        string json = File.ReadAllText(path);
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var seznam = JsonSerializer.Deserialize<List<CPolozka>>(json, options) ?? new List<CPolozka>();

                        if (seznam.Any(pol => pol.Number == zadaneCislo))
                        {
                            MessageBox.Show($"Zboží s číslem {zadaneCislo} už na skladě existuje!", "Duplicitní ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Pokud je JSON poškozený, raději upozorníme, ale nezablokujeme vše
                        MessageBox.Show("Chyba při kontrole databáze: " + ex.Message);
                    }
                }
            }

            return true;
        }

        private void PripravData()
        {
            AktualniPolozka = _editovanaPolozka ?? new CPolozka();

            // Jelikož ZkontrolujData() prošlo, můžeme bezpečně parsovat
            AktualniPolozka.Number = int.Parse(textBox1.Text.Trim());
            AktualniPolozka.Name = textBox2.Text.Trim();
            AktualniPolozka.Category = comboBox1.Text.Trim();
            AktualniPolozka.Quantity = int.Parse(textBox3.Text.Trim());
            AktualniPolozka.Unit = comboBox2.Text.Trim();
            AktualniPolozka.Placement = comboBox3.Text.Trim();
            AktualniPolozka.Minimum = int.Parse(textBox4.Text.Trim());

            string finalniCenaText = textBox5.Text.Trim().Replace(',', '.');
            AktualniPolozka.Price = float.Parse(finalniCenaText, CultureInfo.InvariantCulture);
        }
    }
}
