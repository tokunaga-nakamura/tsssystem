using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Oracle.DataAccess.Client;

namespace TSS_SYSTEM
{
    public partial class frm_table_maintenance : Form
    {
        OracleDataAdapter da;
        DataTable dt = new DataTable();
        OracleCommandBuilder cb;
        DataSet das;

        public frm_table_maintenance()
        {
            InitializeComponent();
        }

        private void cb_table_name_DropDown(object sender, EventArgs e)
        {
            //テーブル名を取得してコンボボックスのアイテムに追加する
            TssSystemLibrary tss = new TssSystemLibrary();
            DataTable dt2 = new DataTable();
            dt2 = tss.OracleSelect("SELECT TABLE_NAME FROM USER_TABLES order by table_name asc");
            if (dt2 == null)
            {
                MessageBox.Show("テーブルが取得できません。", "エラー");
                return;
            }
            cb_table_name.Items.Clear();

            foreach (DataRow tablename in dt2.Rows)
            {
                cb_table_name.Items.Add(tablename[0]);
            }
        }

        private void btn_hyouji_Click(object sender, EventArgs e)
        {
            try
            {
                dt.Rows.Clear();
                dt.Columns.Clear();
                dt.Clear();
                TssSystemLibrary tssdb = new TssSystemLibrary();
                string connStr = tssdb.GetConnectionString();
                OracleConnection conn = new OracleConnection(connStr);
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                string sql = "select * from " + cb_table_name.Text;
                if (tb_sql.Text.Length >= 1)
                {
                    sql = sql + " " + tb_sql.Text;
                }
                cmd.CommandText = sql;
                da = new OracleDataAdapter(cmd);
                cb = new OracleCommandBuilder(da);
                das = new DataSet();
                da.Fill(dt);
                dgv_table.DataSource = dt;
            }
            catch
            {
                dgv_table.DataSource = null;
            }
        }

        private void btn_touroku_Click(object sender, EventArgs e)
        {
            if (dgv_table.DataSource != null)
            {
                try
                {
                    da.Update(dt);
                    MessageBox.Show("更新しました");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "更新失敗");
                }
            }
        }

        private void btn_csv_Click(object sender, EventArgs e)
        {
            //CSV出力
            if (dgv_table.Rows.Count <= 0)
            {
                return;
            }
            //保存ダイアログ
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "ファイルを保存";  // ダイアログのタイトルを指定
            sfd.InitialDirectory = @"c:\"; // 初期フォルダを指定
            sfd.FileName = string.Empty;   // 初期ファイル名
            sfd.CheckFileExists = false;   // ファイルが存在するかどうかチェックする
            sfd.AddExtension = true;       // 拡張子が入力されなければ自動で付与するか否か
            sfd.OverwritePrompt = true;    // ファイルが既に存在する場合警告するか否か
            // ファイルの種類、拡張子のフィルターを設定
            sfd.Filter =
                "すべてのファイル(*.*)|*.*|" +
                "CSVファイル(*.CSV)|*.csv";
            // 初期状態で選択されているフィルターは何番目か
            // インデックスは１からはじまるので注意
            sfd.FilterIndex = 2;
            // ダイアログを表示
            if (sfd.ShowDialog() == DialogResult.OK)    //←このif文は、ダイアログでOKが押されたら・・・って事。
            {
                TssSystemLibrary tssa= new TssSystemLibrary();

                if (tssa.DataTableCSV(dt, sfd.FileName, "\"", true))
                {
                    MessageBox.Show("保存しました。");
                }
                else
                {
                    MessageBox.Show("保存できませんでした。", "エラー");
                }
            }
            return;
        }

        private void btn_syuuryou_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_hardcopy_Click(object sender, EventArgs e)
        {

        }
    }
}
