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
    public partial class frm_uriage : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();
        double w_uriage_no;

        public frm_uriage()
        {
            InitializeComponent();
        }

        private void btn_syuuryou_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_hardcopy_Click(object sender, EventArgs e)
        {
            tss.HardCopy();
        }

        private void frm_uriage_Load(object sender, EventArgs e)
        {
            w_uriage_no = tss.GetSeq("05");
            tb_uriage_no.Text = w_uriage_no.ToString("0000000000");
            tb_uriage_no.Focus();
        }

        private void tb_uriage_date_Validating(object sender, CancelEventArgs e)
        {
            if(tss.try_string_to_date(tb_uriage_date.Text.ToString()))
            {
                tb_uriage_date.Text = tss.out_datetime.ToShortDateString();
            }
            else
            {
                MessageBox.Show("売上計上日に異常があります。");
                tb_uriage_date.Focus();
            }
        }

        private void tb_torihikisaki_cd_Validating(object sender, CancelEventArgs e)
        {
            //終了ボタンを考慮して、空白は許容する
            if (tb_torihikisaki_cd.Text != "")
            {
                //既存データの場合は、取引先コードの変更、再読み込みは不可
                if (tb_uriage_no.Text.ToString() == w_uriage_no.ToString("0000000000"))
                {
                    if (chk_torihikisaki_cd() != true)
                    {
                        MessageBox.Show("取引先コードに異常があります。");
                        e.Cancel = true;
                    }
                    else
                    {
                        //取引先名を取得・表示
                        tb_torihikisaki_name.Text = get_torihikisaki_name(tb_torihikisaki_cd.Text);
                    }
                }
            }
        }

        private string get_torihikisaki_name(string in_torihikisaki_cd)
        {
            string out_torihikisaki_name = "";  //戻り値用
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_torihikisaki_m where torihikisaki_cd = '" + in_torihikisaki_cd + "'");
            if (dt_work.Rows.Count <= 0)
            {
                out_torihikisaki_name = "";
            }
            else
            {
                out_torihikisaki_name = dt_work.Rows[0]["torihikisaki_name"].ToString();
            }
            return out_torihikisaki_name;
        }

        private bool chk_torihikisaki_cd()
        {
            bool bl = true; //戻り値
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_torihikisaki_m where torihikisaki_cd  = '" + tb_torihikisaki_cd.Text.ToString() + "'");
            if (dt_work.Rows.Count <= 0)
            {
                //無し
                bl = false;
            }
            else
            {
                //既存データ有
            }
            return bl;
        }

        private void tb_uriage_no_Validating(object sender, CancelEventArgs e)
        {
            //入力された売上番号を"0000000000"形式の文字列に変換
            double w_double;
            if(double.TryParse(tb_uriage_no.Text.ToString(), out w_double))
            {
                tb_uriage_no.Text = w_double.ToString("0000000000");
            }
            else
            {
                MessageBox.Show("売上番号に異常があります。");
                tb_uriage_no.Focus();
            }
            //新規か既存かの判定
            if(tb_uriage_no.Text.ToString() == w_uriage_no.ToString("0000000000"))
            {
                //新規
                //dgvに空のデータを表示するためのダミー抽出
                DataTable w_dt = new DataTable();
                w_dt = tss.OracleSelect("select * from tss_uriage_m where uriage_no = '" + tb_uriage_no.Text.ToString() + "' order by uriage_no asc,seq asc");
                uriage_sinki(w_dt);
            }
            else
            {
                //既存売上の表示
                DataTable w_dt = new DataTable();
                w_dt = tss.OracleSelect("select * from tss_uriage_m where uriage_no = '" + tb_uriage_no.Text.ToString() + "' order by uriage_no asc,seq asc");
                if(w_dt.Rows.Count == 0)
                {
                    MessageBox.Show("データがありません。");
                    tb_uriage_no.Text = w_uriage_no.ToString("0000000000");
                    tb_uriage_no.Focus();
                    return;
                }
                uriage_disp(w_dt);
            }
        }    

        private void uriage_disp(DataTable in_dt)
        {
            //dgvをクリア
            dgv_m.DataSource = null;
            dgv_m.Columns.Clear();
            dgv_m.Rows.Clear();

            //画面の見出し項目を表示
            tb_torihikisaki_cd.Text = in_dt.Rows[0]["torihikisaki_cd"].ToString();
            tb_torihikisaki_name.Text = tss.get_torihikisaki_name(in_dt.Rows[0]["torihikisaki_cd"].ToString());
            tb_uriage_date.Text = DateTime.Parse(in_dt.Rows[0]["uriage_date"].ToString()).ToShortDateString();

            //dgvにデータをバインド
            dgv_m.DataSource = in_dt;

            //dgvの表示設定
            uriage_init();

            //合計を表示
            uriage_goukei_disp();
        }

        private void uriage_sinki(DataTable in_dt)
        {
            //dgvをクリア
            dgv_m.DataSource = null;
            dgv_m.Columns.Clear();
            dgv_m.Rows.Clear();

            //dgvにデータをバインド
            dgv_m.DataSource = in_dt;

            //dgvの表示設定
            uriage_init();

            //合計を表示
            uriage_goukei_disp();

            //seqを表示
            seq_disp();
        }
        private void uriage_init()
        {
            //カラム幅の自動調整（ヘッダーとセルの両方の最長幅に調整する）
            dgv_m.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //セルの高さ変更不可
            dgv_m.AllowUserToResizeRows = false;
            //カラムヘッダーの高さ変更不可
            dgv_m.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //１行のみ選択可能（複数行の選択不可）
            dgv_m.MultiSelect = false;

            //カラムヘッダーの定義
            dgv_m.Columns[0].HeaderText = "売上No";
            dgv_m.Columns[1].HeaderText = "SEQ";
            dgv_m.Columns[2].HeaderText = "取引先コード";
            dgv_m.Columns[3].HeaderText = "売上計上日";
            dgv_m.Columns[4].HeaderText = "受注コード1";
            dgv_m.Columns[5].HeaderText = "受注コード2";
            dgv_m.Columns[6].HeaderText = "製品コード";
            dgv_m.Columns[7].HeaderText = "製品名";
            dgv_m.Columns[8].HeaderText = "売上数";
            dgv_m.Columns[9].HeaderText = "販売単価";
            dgv_m.Columns[10].HeaderText = "売上金額";
            dgv_m.Columns[11].HeaderText = "請求番号";
            dgv_m.Columns[12].HeaderText = "売上締日";
            dgv_m.Columns[13].HeaderText = "削除フラグ";
            dgv_m.Columns[14].HeaderText = "備考";
            dgv_m.Columns[15].HeaderText = "作成者コード";
            dgv_m.Columns[16].HeaderText = "作成日時";
            dgv_m.Columns[17].HeaderText = "更新者コード";
            dgv_m.Columns[18].HeaderText = "更新日時";

            //セルの値表示位置の設定
            dgv_m.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;     //seq
            dgv_m.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;     //売上数
            dgv_m.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;     //販売単価
            dgv_m.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;    //売上金額

            //指定列を非表示にする
            dgv_m.Columns[0].Visible = false;   //売上No
            dgv_m.Columns[2].Visible = false;   //取引先コード
            dgv_m.Columns[3].Visible = false;   //売上計上日
            dgv_m.Columns[11].Visible = false;  //請求番号
            dgv_m.Columns[12].Visible = false;  //売上締日
            dgv_m.Columns[13].Visible = false;  //削除フラグ
            dgv_m.Columns[15].Visible = false;  //作成者コード
            dgv_m.Columns[16].Visible = false;  //作成日時
            dgv_m.Columns[17].Visible = false;  //更新者コード
            dgv_m.Columns[18].Visible = false;  //更新日時
        }


        private void uriage_goukei_disp()
        {
            double w_dou;
            double w_uriage_goukei = 0;
            for (int i = 0; i < dgv_m.Rows.Count - 1;i++)
            {
                if (double.TryParse(dgv_m.Rows[i].Cells["uriage_kingaku"].Value.ToString(), out w_dou))
                {
                    w_uriage_goukei = w_uriage_goukei + w_dou;
                }
            }
            tb_uriage_goukei.Text = w_uriage_goukei.ToString("#,0");
        }

        private void seq_disp()
        {
            for(int i = 0;i<dgv_m.Rows.Count;i++)
            {
                dgv_m.Rows[i].Cells[1].Value = i + 1;
            }
        }
    
    }
}
