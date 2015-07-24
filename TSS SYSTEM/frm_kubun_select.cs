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
    /// <summary>
    /// <para>区分マスタの選択画面です。</para>
    /// <para>プロパティ str_kubun_meisyou_cd 抽出用の区分名称コード</para>
    /// <para>プロパティ str_kubun_cd 戻り値用の区分コード（エラー・キャンセル時は""）</para>
    /// <para>プロパティ bl_kubun_sentaku 通常選択時:true、エラー・キャンセル時:false</para>
    /// </summary>

    public partial class frm_kubun_select : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();
        DataTable dt_kubun_m = new DataTable();


        //親画面から参照できるプロパティを作成
        public string fld_kubun_meisyou_cd;    //選択する区分名称コード
        public string fld_kubun_cd;            //選択された区分コード
        public bool fld_kubun_sentaku;         //区分選択フラグ 選択:true エラーまたはキャンセル:false

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
            form_close_true();
        }

        private void frm_kubun_select_Load(object sender, EventArgs e)
        {
            //行ヘッダーを非表示にする
            dgv_kubun_m.RowHeadersVisible = false;
            //カラム幅の自動調整（ヘッダーとセルの両方の最長幅に調整する）
            dgv_kubun_m.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //セルの高さ変更不可
            dgv_kubun_m.AllowUserToResizeRows = false;
            //カラムヘッダーの高さ変更不可
            dgv_kubun_m.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //削除不可にする（コードからは削除可）
            dgv_kubun_m.AllowUserToDeleteRows = false;
            //１行のみ選択可能（複数行の選択不可）
            dgv_kubun_m.MultiSelect = false;
            //セルを選択すると行全体が選択されるようにする
            dgv_kubun_m.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //DataGridView1にユーザーが新しい行を追加できないようにする
            dgv_kubun_m.AllowUserToAddRows = false;


            //画面に引数の区分名称コード表示
            this.tb_kubun_meisyou_cd.Text = str_kubun_meisyou_cd;
            //画面に区分名称表示
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select kubun_meisyou_cd,kubun_name from tss_kubun_meisyou_m where kubun_meisyou_cd = '" + tb_kubun_meisyou_cd.Text + "'");
            if(dt_work == null)
            {
                MessageBox.Show("該当する区分名称マスタがありません。");
                form_close_false();
            }
            this.tb_kubun_meisyou_name.Text = dt_work.Rows[0]["KUBUN_NAME"].ToString() ;
            //引数を基にデータを抽出して表示
            dt_kubun_m = tss.OracleSelect("select kubun_cd,kubun_name from tss_kubun_m where kubun_meisyou_cd = '" + tb_kubun_meisyou_cd.Text + "' order by kubun_cd asc");
            dgv_kubun_m.DataSource = dt_kubun_m;
            //DataGridViewのカラムヘッダーテキストを変更する
            dgv_kubun_m.Columns[0].HeaderText = "区分コード";
            dgv_kubun_m.Columns[1].HeaderText = "区分名";

            if(dt_kubun_m == null)
            {
                MessageBox.Show("該当する区分マスタがありません。");
                form_close_false();
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            form_close_false();
        }

        private void dgv_kubun_m_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            form_close_true();
        }

        //エラー及びキャンセル時の終了処理
        private void form_close_false()
        {
            str_kubun_cd = "";
            bl_sentaku = false;
            this.Close();
        }

        //選択時の終了処理
        private void form_close_true()
        {
            str_kubun_cd = dgv_kubun_m.CurrentRow.Cells[0].Value.ToString();
            bl_sentaku = false;
            this.Close();
        }
    }
}
