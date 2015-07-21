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
    public partial class frm_kubun_select : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();
        DataTable dt_kubun_m = new DataTable();


        //親画面から参照できるプロパティを作成
        private string fld_kubun_meisyou_cd;    //選択する区分名称コード
        private string fld_kubun_cd;            //選択された区分コード
        private bool fld_kubun_sentaku;         //区分選択フラグ 選択:true エラーまたはキャンセル:false

        public string str_kubun_meisyou_cd
        {
            get
            {
                return fld_kubun_meisyou_cd;
            }
            set
            {
                fld_kubun_meisyou_cd = value;
            }
        }
        public string str_kubun_cd
        {
            get
            {
                return fld_kubun_cd;
            }
            set
            {
                fld_kubun_cd = value;
            }
        }
        public bool bl_sentaku
        {
            get
            {
                return fld_kubun_sentaku;
            }
            set
            {
                fld_kubun_sentaku = value;
            }
        }

        public frm_kubun_select()
        {
            InitializeComponent();
        }

        private void btn_sentaku_Click(object sender, EventArgs e)
        {
            str_kubun_cd = dgv_kubun_m.CurrentRow.Cells[0].ToString();

        }

        private void frm_kubun_select_Load(object sender, EventArgs e)
        {
            //１行のみ選択可能（複数行の選択不可）
            dgv_kubun_m.MultiSelect = false;
            //セルを選択すると行全体が選択されるようにする
            dgv_kubun_m.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //画面に引数の区分名称コード表示
            this.tb_kubun_meisyou_cd.Text = str_kubun_meisyou_cd;
            //画面に区分名称表示
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select kubun_meisyou_cd,kubun_name from tss_kubun_meisyou_m where kubun_meisyou_cd = '" + tb_kubun_meisyou_cd.Text + "'");
            if(dt_work == null)
            {
                MessageBox.Show("該当する区分名称マスタがありません。");
                this.Close();
            }
            this.tb_kubun_meisyou_name.Text = dt_work.Rows[0]["KUBUN_NAME"].ToString() ;
            //引数を基にデータを抽出して表示
            dt_kubun_m = tss.OracleSelect("select kubun_cd,kubun_name from tss_kubun_m where kubun_meisyou_cd = '" + tb_kubun_meisyou_cd.Text + "' order by kubun_cd asc");
            dgv_kubun_m.DataSource = dt_kubun_m;
            if(dt_kubun_m == null)
            {
                MessageBox.Show("該当する区分マスタがありません。");
                this.Close();
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            bl_sentaku = false;
            this.Close();
        }
    }
}
