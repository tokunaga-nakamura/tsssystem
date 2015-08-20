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
    public partial class frm_siharai : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();
        double w_siire_no;

        public frm_siharai()
        {
            InitializeComponent();
        }


        private void tb_torihikisaki_cd_Validating_1(object sender, CancelEventArgs e)
        {
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select torihikisaki_name,syouhizei_sansyutu_kbn from tss_torihikisaki_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'");

            if (dt_work.Rows.Count == 0)
            {
                return;
            }

            else
            {
                tb_torihikisaki_name.Text = dt_work.Rows[0][0].ToString();
            }
        }

        private void btn_hyouji_Click(object sender, EventArgs e)
        {
            DataTable dt_work = new DataTable();
            tss.GetUser();
            dt_work = tss.OracleSelect("select * from tss_kaikake_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "' and siharai_no is null");
            int rc = dt_work.Rows.Count;

            for(int i = 0; i < rc  ; i++)
            {
                dgv_mibarai.Rows.Add();
                
                double goukeikingaku = double.Parse(dt_work.Rows[i][4].ToString()) + double.Parse(dt_work.Rows[i][5].ToString());
                
                dgv_mibarai.Rows[i].Cells[0].Value = dt_work.Rows[i][1].ToString();
                dgv_mibarai.Rows[i].Cells[1].Value = double.Parse(dt_work.Rows[i][4].ToString());
                dgv_mibarai.Rows[i].Cells[2].Value = double.Parse(dt_work.Rows[i][5].ToString());
                dgv_mibarai.Rows[i].Cells[3].Value = goukeikingaku;

                //使用数量右寄せ、カンマ区切り
                dgv_mibarai.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv_mibarai.Columns[1].DefaultCellStyle.Format = "#,0.##";

                //dgv_mibarai.Columns[4].DefaultCellStyle.Format = "#,0.##";

                dgv_mibarai.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv_mibarai.Columns[2].DefaultCellStyle.Format = "#,0.##";

                dgv_mibarai.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv_mibarai.Columns[3].DefaultCellStyle.Format = "#,0.##";


                //１行のみ選択可能（複数行の選択不可）
                dgv_mibarai.MultiSelect = false;
                //セルを選択すると行全体が選択されるようにする
                dgv_mibarai.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }

            object obj = dt_work.Compute("SUM([siire_kingaku])", null);
            object obj2 = dt_work.Compute("SUM([syouhizeigaku])", null);           double goukeikingku =  double.Parse(obj.ToString()) + double.Parse(obj2.ToString());

            tb_mibarai_goukei.Text = goukeikingku.ToString("#,0.##");

        }

        private void btn_siharai_syori_Click(object sender, EventArgs e)
        {
            tb_siire_no.Enabled = true;
            
        }

        private void frm_siharai_Load(object sender, EventArgs e)
        {

            w_siire_no = tss.GetSeq("07");
            tb_siire_no.Text = w_siire_no.ToString("0000000000");

            //tb_siire_no.Enabled = false;
        }

        private void tb_siharai_date_Validating(object sender, CancelEventArgs e)
        {
            if (tss.try_string_to_date(tb_siharai_date.Text.ToString()))
            {
                tb_siharai_date.Text = tss.out_datetime.ToShortDateString();
            }
            else
            {
                MessageBox.Show("仕入締日の値が異常です。yyyymmddで入力してください。");
                tb_siharai_date.Focus();
            }
        }

        private void btn_siharai_syori_Click_1(object sender, EventArgs e)
        {
            string str = dgv_mibarai.CurrentRow.Cells[0].Value.ToString();

            dgv_siharai.Rows.Add();
            dgv_siharai.Rows[0].Cells[0].Value = str;


        }

        private void dgv_siharai_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.Columns[e.ColumnIndex].Index == 0 && dgv.CurrentCell.Value == null)
            {

            }
        }


    }
}
