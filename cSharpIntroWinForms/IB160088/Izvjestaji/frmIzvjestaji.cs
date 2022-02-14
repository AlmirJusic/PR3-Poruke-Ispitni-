using cSharpIntroWinForms.P10;
using cSharpIntroWinForms.P9;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cSharpIntroWinForms.IB160088.Izvjestaji
{
    public partial class frmIzvjestaji : Form
    {
        private KorisniciPredmeti kp;
        KonekcijaNaBazu konekcija = DLWMS.DB;

        public frmIzvjestaji()
        {
            InitializeComponent();
        }

        public frmIzvjestaji(KorisniciPredmeti kp):this()
        {
            this.kp = kp;
        }

        private void frmIzvjestaji_Load(object sender, EventArgs e)
        {
            ReportParameterCollection rpc = new ReportParameterCollection();
            rpc.Add(new ReportParameter("Korisnik", kp.Korisnik.Ime + " " + kp.Korisnik.Prezime));

            List<object> lista = new List<object>();
            int i = 1;

            foreach (var item in konekcija.KorisniciPoruke.Where(x=>x.Korisnik.Id==kp.Korisnik.Id))
            {
                lista.Add(new
                {
                    Rb=i++,
                    Datum=item.Datum,
                    Sadrzaj=item.Sadrzaj
                });
            }

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DLWMS";
            rds.Value = lista;

            reportViewer1.LocalReport.SetParameters(rpc);
            reportViewer1.LocalReport.DataSources.Add(rds);


            this.reportViewer1.RefreshReport();
        }
    }
}
