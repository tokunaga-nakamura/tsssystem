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
    public partial class frm_bank_m : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();
    
        public frm_bank_m()
        {
            InitializeComponent();
        }

        private void frm_bank_m_Load(object sender, EventArgs e)
        {
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from TSS_BANK_M LEFT OUTER JOIN TSS_TORIHIKISAKI_M ON TSS_BANK_M.TORIHIKISAKI_CD = TSS_TORIHIKISAKI_M.TORIHIKISAKI_CD  ORDER BY TORIHIKISAKI_CD");

            dgv_bank_m.DataSource = dt_work;
            



        }

    }
}
