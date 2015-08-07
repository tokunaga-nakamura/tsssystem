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
    public partial class frm_eigyou_calender : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();
        DataTable w_dt_calender = new DataTable();

        public frm_eigyou_calender()
        {
            InitializeComponent();
        }

        private void btn_hardcopy_Click(object sender, EventArgs e)
        {
            tss.HardCopy();
        }

        private void btn_syuuryou_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_eigyou_calender_Load(object sender, EventArgs e)
        {
            //表示の初期値設定
            nud_year.Value = DateTime.Today.Year;
            nud_month.Value = DateTime.Today.Month;
            //表示
            calender_disp();
        }


        private void calender_disp()
        {
            get_calender();
            if(w_dt_calender.Rows.Count == 0)
            {
                //新規にレコードを作成
                calender_create();
                get_calender();
                if(w_dt_calender.Rows.Count == 0)
                {
                    MessageBox.Show("営業カレンダーの作成で例外エラーが発生しました。処理を中止します。");
                    this.Close();
                }
            }
            dgv_calender.DataSource = w_dt_calender;

            //セルの高さ変更不可
            dgv_calender.AllowUserToResizeRows = false;

            //カラムヘッダーの高さ変更不可
            dgv_calender.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            //削除不可にする（コードからは削除可）
            dgv_calender.AllowUserToDeleteRows = false;

            //行ヘッダーを非表示にする
            dgv_calender.RowHeadersVisible = false;

            //DataGridView1にユーザーが新しい行を追加できないようにする（最下行を非表示にする）
            dgv_calender.AllowUserToAddRows = false;

            //列の文字数制限（TextBoxのMaxLengthと同じ動作になる）
            ((DataGridViewTextBoxColumn)dgv_calender.Columns[3]).MaxInputLength = 1;    //祝祭日名称
            ((DataGridViewTextBoxColumn)dgv_calender.Columns[4]).MaxInputLength = 1;    //営業区分
            ((DataGridViewTextBoxColumn)dgv_calender.Columns[7]).MaxInputLength = 1;    //内容

            //指定列を非表示にする
            dgv_calender.Columns[8].Visible = false;
            dgv_calender.Columns[9].Visible = false;
            dgv_calender.Columns[10].Visible = false;
            dgv_calender.Columns[11].Visible = false;
        }


        private void calender_create()
        {
            tss.GetUser();
            int w_int_days = DateTime.DaysInMonth((int)nud_year.Value,(int)nud_month.Value);
            for(int i = 1;i <= w_int_days;i++)
            {
                if(tss.OracleInsert("insert into tss_calender_f (year,month,day,create_user_cd,create_datetime) value ('" + nud_year.Value.ToString() + "','" + nud_month.Value.ToString("00") + "','" + i.ToString("00") + "','" + tss.user_cd + "',sysdate") == false)
                {
                    MessageBox.Show("営業カレンダーの作成でエラーが発生しました。処理を中止します。");
                    this.Close();
                }
            }
        }

        private void get_calender()
        {
            w_dt_calender = tss.OracleSelect("select * from tss_calender_f where year = '" + nud_year.Value.ToString() + "' and month = '" + nud_month.Value.ToString("00") + "'");
        }

    }
}
