using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSS_SYSTEM
{
    public partial class frm_nouhin_schedule_preview : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();
        DataTable w_dt_nouhin_schedule = new DataTable();

        //親画面から参照できるプロパティを作成
        public DataTable fld_dt = new DataTable();   //印刷する明細データ

        public DataTable ppt_dt
        {
            get
            {
                return fld_dt;
            }
            set
            {
                fld_dt = value;
            }
        }




        public frm_nouhin_schedule_preview()
        {
            InitializeComponent();
        }

        private void btn_syuuryou_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_nouhin_schedule_preview_Load(object sender, EventArgs e)
        {
            rpt_nouhin_schedule rpt = new rpt_nouhin_schedule();
            rpt.DataSource = ppt_dt;

            rpt.Run();
            this.vwr.Document = rpt.Document;
            
        }
    }
}
