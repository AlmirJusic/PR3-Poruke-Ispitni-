using cSharpIntroWinForms.P10;
using cSharpIntroWinForms.P9;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cSharpIntroWinForms.IB160088
{
    public partial class frmPretraga : Form
    {
        KonekcijaNaBazu konekcija = DLWMS.DB;
        public frmPretraga()
        {
            InitializeComponent();
            dgvPredmeti.AutoGenerateColumns = false;
            LoadData();
        }
        private void LoadData(List<KorisniciPredmeti> korisnici = null)
        {
            try
            {
                dgvPredmeti.DataSource = null;
                dgvPredmeti.DataSource = korisnici ?? konekcija.KorisniciPredmeti.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.InnerException?.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            List<KorisniciPredmeti> temp = new List<KorisniciPredmeti>();
            foreach (var item in konekcija.KorisniciPredmeti)
            {
                if (item.Predmet.Naziv.ToLower().Contains(txtNaziv.Text.ToLower()))
                    temp.Add(item);
                LoadData(temp);
            }
        }

        private void frmPretraga_Load(object sender, EventArgs e)
        {
            int brojac = 0;
            float suma = 0;
            foreach (var item in konekcija.KorisniciPredmeti)
            {
                suma += item.Ocjena;
                brojac++;
            }
            if (brojac == 0)
                lblProsjek.Text = "0";
            else
            {
                suma /= brojac;
                lblProsjek.Text = suma.ToString();
            }
        }

        private void btnSuma_Click(object sender, EventArgs e)
        {
            Thread t1 = new Thread(IzracunajSumu);
            t1.Start();
        }

        private void IzracunajSumu()
        {
            Thread.Sleep(1000);
            Action action = () =>
            {
                long broj = Int64.Parse(txtSuma.Text);
                long suma = 0;
                for (long i = 1; i <= broj; i++)
                {
                    suma += i;
                }
                lblSuma.Text = suma.ToString();
            };
            BeginInvoke(action);
        }

        private void dgvPredmeti_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            KorisniciPredmeti kp = dgvPredmeti.SelectedRows[0].DataBoundItem as KorisniciPredmeti;
            if(e.ColumnIndex==4)
            {
                frmPoruke frm = new frmPoruke(kp);
                frm.Show();
            }
        }
    }
}
