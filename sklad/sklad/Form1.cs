using System;using System.Collections.Generic;using System.Drawing;using System.Globalization;using System.IO;using System.Linq;using System.Text.Json;using System.Windows.Forms;namespace sklad
{
    public partial class Form1 : Form
    {
        // Cesta k datovému souboru
        private string path = Path.Combine(Application.StartupPath, "data.json");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NastavListView();          // Konfigurace tabulky
            ZkontrolujAVytvorSoubor(); // Kontrola jestli existuje datový soubor, pokud ne, vytvoří ho
            NactiDataDoListView();     // načtení dat

            // Nastavení defaultní hodnoty pro naskladnění/vyskladnění do TextBoxu
            textBox1.Text = "1";
        }

        // Nastavení vzhledu ListView (sloupce, řádky, výběr)
        private void NastavListView()
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.MultiSelect = true;

            listView1.Items.Clear();
            listView1.Columns.Clear();

            // Definice sloupců a jejich šířky
            listView1.Columns.Add("Číslo zboží", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("Název", 200, HorizontalAlignment.Center);
            listView1.Columns.Add("Počet", 80, HorizontalAlignment.Center);
            listView1.Columns.Add("MJ", 50, HorizontalAlignment.Center);
            listView1.Columns.Add("Umístění", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("Cena", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("Min. množství", 142, HorizontalAlignment.Center);
        }


        // Načte data z JSONu a naplní tabulku. Hlídá limity zásob a barví řádky.
        public void NactiDataDoListView()
        {
            try
            {
                listView1.Items.Clear();
                List<CPolozka> seznam = NactiSeznamZeSouboru();

                foreach (var p in seznam)
                {
                    ListViewItem radek = new ListViewItem(p.Number.ToString());
                    radek.SubItems.Add(p.Name ?? "");
                    radek.SubItems.Add(p.Quantity.ToString());
                    radek.SubItems.Add(p.Unit ?? "");
                    radek.SubItems.Add(p.Placement ?? "");
                    radek.SubItems.Add(p.Price.ToString("F2", CultureInfo.CurrentCulture));
                    radek.SubItems.Add(p.Minimum.ToString());

                    // Celý objekt uložíme do Tagu pro pozdější manipulaci (editace/naskladnění)
                    radek.Tag = p;

                    // OPRAVA: Pokud je stav pod limitem A ZÁROVEŇ je checkbox zaškrtnutý
                    if (p.Quantity <= p.Minimum)
                    {
                        radek.BackColor = Color.FromArgb(255, 230, 230);
                        radek.Font = new Font(listView1.Font, FontStyle.Bold);
                    }
                    else
                    {
                        // Pokud podmínka neplatí, vrátíme výchozí vzhled
                        radek.BackColor = Color.White;
                        radek.Font = new Font(listView1.Font, FontStyle.Regular);
                    }

                    listView1.Items.Add(radek);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání dat: " + ex.Message);
            }
        }



        private List<CPolozka> NactiSeznamZeSouboru()
        {
            if (!File.Exists(path)) return new List<CPolozka>();
            string json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return new List<CPolozka>();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<CPolozka>>(json, options) ?? new List<CPolozka>();
        }

        private void UlozSeznamDoSouboru(List<CPolozka> seznam)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(seznam, options);
            File.WriteAllText(path, json);
        }

        public void ZkontrolujAVytvorSoubor()
        {
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) Directory.CreateDirectory(directory);
            if (!File.Exists(path)) File.WriteAllText(path, "[]");
        }





        // Tlačítko PŘIDAT NOVOU POLOŽKU
        private void button1_Click(object sender, EventArgs e)
        {
            FormularPolozky novyForm = new FormularPolozky();
            if (novyForm.ShowDialog() == DialogResult.OK)
            {
                var seznam = NactiSeznamZeSouboru();
                seznam.Add(novyForm.AktualniPolozka);
                UlozSeznamDoSouboru(seznam);
                NactiDataDoListView();
            }
        }

        // Tlačítko EDITOVAT VYBRANOU
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
            {
                MessageBox.Show("Vyberte právě jednu položku.");
                return;
            }

            if (!(listView1.SelectedItems[0].Tag is CPolozka vybrana))
            {
                MessageBox.Show("Chyba: položka nemá připojená data.");
                return;
            }

            FormularPolozky editForm = new FormularPolozky(vybrana);

            if (editForm.ShowDialog() == DialogResult.OK)
            {
                var seznam = NactiSeznamZeSouboru();
                int index = seznam.FindIndex(p => p.Number == vybrana.Number);
                if (index != -1)
                {
                    seznam[index] = editForm.AktualniPolozka;
                    UlozSeznamDoSouboru(seznam);
                    NactiDataDoListView();
                }
            }
        }

        // Tlačítko SMAZAT
        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            var dotaz = MessageBox.Show($"Opravdu smazat {listView1.SelectedItems.Count} položek?", "Smazat", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dotaz == DialogResult.Yes)
            {
                var seznam = NactiSeznamZeSouboru();
                foreach (ListViewItem radek in listView1.SelectedItems)
                {
                    if (radek.Tag is CPolozka kOdstraneni)
                    {
                        seznam.RemoveAll(p => p.Number == kOdstraneni.Number);
                    }
                }
                UlozSeznamDoSouboru(seznam);
                NactiDataDoListView();
            }
        }

        // Tlačítko NASKLADNIT
        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            if (!int.TryParse(textBox1.Text, out int mnozstvi) || mnozstvi <= 0)
            {
                MessageBox.Show("Zadejte platné množství pro naskladnění.");
                return;
            }

            if (!(listView1.SelectedItems[0].Tag is CPolozka vybrana))
            {
                MessageBox.Show("Chyba: položka nemá připojená data.");
                return;
            }

            AktualizovatStavSkladu(vybrana.Number, mnozstvi);
        }

        // Tlačítko VYSKLADNIT
        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            if (!int.TryParse(textBox1.Text, out int mnozstvi) || mnozstvi <= 0)
            {
                MessageBox.Show("Zadejte platné množství pro vyskladnění.");
                return;
            }

            if (!(listView1.SelectedItems[0].Tag is CPolozka vybrana))
            {
                MessageBox.Show("Chyba: položka nemá připojená data.");
                return;
            }

            // Ochrana: Nesmíme vyskladnit víc, než máme
            if (vybrana.Quantity < mnozstvi)
            {
                MessageBox.Show($"Nelze vydat {mnozstvi}, na skladě zbývá jen {vybrana.Quantity}!");
                return;
            }

            AktualizovatStavSkladu(vybrana.Number, -mnozstvi);
        }

        // Změní množství v souboru a postará se o to, aby řádek zůstal označený
        private void AktualizovatStavSkladu(int id, int zmena)
        {
            var seznam = NactiSeznamZeSouboru();
            var p = seznam.Find(item => item.Number == id);
            if (p != null)
            {
                p.Quantity += zmena;
                UlozSeznamDoSouboru(seznam);
                NactiDataDoListView();
                OznacRadekZpetne(id);
            }
        }

        // Najde v ListView řádek podle ID a hodí na něj focus
        private void OznacRadekZpetne(int id)
        {
            foreach (ListViewItem radek in listView1.Items)
            {
                if (radek.Tag is CPolozka tag && tag.Number == id)
                {
                    radek.Selected = true;
                    radek.Focused = true;
                    radek.EnsureVisible(); // Odroluje k položce
                    break;
                }
            }
            listView1.Focus(); // aby byl výběr vidět hned po kliknutí na tlačítko
        }

        // EVENT: Spustí se při každé změně fajfky v checkboxu
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            NactiDataDoListView();
        }
    }
}