using cSharpIntroWinForms.IB160088.Izvjestaji;
using cSharpIntroWinForms.P10;
using cSharpIntroWinForms.P9;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cSharpIntroWinForms.IB160088
{
    public partial class frmPoruke : Form
    {
        private KorisniciPredmeti kp;
        KonekcijaNaBazu konekcija = DLWMS.DB;
        DataGridView dgv;

        public frmPoruke()
        {
            InitializeComponent();
            dgvPoruke.AutoGenerateColumns = false;
        }

        public frmPoruke(KorisniciPredmeti kp):this()
        {
            this.kp = kp;
            
            LoadData();
        }
        private void LoadData(List<KorisniciPoruke> korisnici = null)
        {
            try
            {
                dgvPoruke.DataSource = null;
                dgvPoruke.DataSource = korisnici ?? konekcija.KorisniciPoruke.Where(x=>x.Korisnik.Id==kp.Id).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.InnerException?.Message);
            }
        }

        private void frmPoruke_Load(object sender, EventArgs e)
        {
            lblKorisnik.Text = kp.Korisnik.Ime + " " + kp.Korisnik.Prezime;
        }

        private void dgvPoruke_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            KorisniciPoruke _kporuke = dgvPoruke.SelectedRows[0].DataBoundItem as KorisniciPoruke;
            if(e.ColumnIndex==2)
            {
                DialogResult dialogResult = MessageBox.Show("Da li zelite izbrisati poruku?","Poruka",MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                    konekcija.KorisniciPoruke.Remove(_kporuke);
                    konekcija.SaveChanges();
                    MessageBox.Show("Uspjesno ste izbrisali poruku!");
                    LoadData();
                }
            }
        }

        private void dgvPoruke_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            KorisniciPoruke _kporuke = dgvPoruke.SelectedRows[0].DataBoundItem as KorisniciPoruke;

            frmNovaPoruka frm = new frmNovaPoruka(_kporuke);
            frm.Show();
        }

        private void btnNovaPoruka_Click(object sender, EventArgs e)
        {
            dgv = dgvPoruke;
            frmNovaPoruka frm = new frmNovaPoruka(dgv,kp);
            frm.Show();
        }

        private void btnPrintaj_Click(object sender, EventArgs e)
        {
            if(kp!=null)
            {
                frmIzvjestaji frm = new frmIzvjestaji(kp);
                frm.Show();
            }
        }
    }
}
