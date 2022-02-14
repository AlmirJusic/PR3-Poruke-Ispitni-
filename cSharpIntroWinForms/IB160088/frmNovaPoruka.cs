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
    public partial class frmNovaPoruka : Form
    {
        private KorisniciPoruke kporuke;
        private KorisniciPredmeti kp;
        private DataGridView dgv;
        KorisniciPoruke _kporuke = new KorisniciPoruke();
        KonekcijaNaBazu konekcija = DLWMS.DB;
        bool edit = false;

        public frmNovaPoruka()
        {
            InitializeComponent();
            
        }

        public frmNovaPoruka(KorisniciPoruke kporuke):this()
        {
            this.kporuke = kporuke;
            txtKorisnik.Text = kporuke.Korisnik.Ime + " " + kporuke.Korisnik.Prezime;
            txtSadrzaj.Text = kporuke.Sadrzaj;
            edit = true;
        }

        public frmNovaPoruka(DataGridView dgv,KorisniciPredmeti kp):this()
        {
            this.dgv = dgv;
            this.kp = kp;
            txtKorisnik.Text= kp.Korisnik.Ime + " " + kp.Korisnik.Prezime;
        }

        private void btnSacuvaj_Click(object sender, EventArgs e)
        {
            if (edit)
                Close();

            if(Validiraj())
            {
                _kporuke.Korisnik = kp.Korisnik;
                _kporuke.Datum = DateTime.Now.ToString();
                _kporuke.Sadrzaj = txtSadrzaj.Text;

                konekcija.KorisniciPoruke.Add(_kporuke);
                konekcija.SaveChanges();
                MessageBox.Show("Uspjeno ste dodali poruku!");
                Close();
                LoadData(dgv);
            }
        }
        private void LoadData(DataGridView dgv,List<KorisniciPoruke> korisnici = null)
        {
            try
            {
                dgv.DataSource = null;
                dgv.DataSource = korisnici ?? konekcija.KorisniciPoruke.Where(x => x.Korisnik.Id == kp.Id).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.InnerException?.Message);
            }
        }

        private bool Validiraj()
        {
            return Validator.ObaveznoPolje(txtSadrzaj, errorProvider1, Validator.porObaveznaVrijednost);
        }
    }
}
