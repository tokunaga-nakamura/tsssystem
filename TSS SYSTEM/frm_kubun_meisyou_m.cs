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
    public partial class frm_kubun_meisyou_m : Form
    {
        DataTable dt = new DataTable();
        TssSystemLibrary tss = new TssSystemLibrary();


        public frm_kubun_meisyou_m()
        {
            InitializeComponent();
        }

        private void btn_hardcopy_Click(object sender, EventArgs e)
        {
            //Altキー＋Print Screenキーの送信
            SendKeys.SendWait("%{PRTSC}");
        }

        private void frm_kubun_meisyou_m_Load(object sender, EventArgs e)
        {
            status_disp();
            kubun_meisyou_m_disp();
        }

        private void btn_touroku_Click(object sender, EventArgs e)
        {
            //区分名称コードのチェック
            if(kubun_meisyou_cd_check() != true)
            {
                tb_kubun_meisyou_cd.Focus();
            }
            //区分名称のチェック
            else if (tb_kubun_meisyou.Text == null || tb_kubun_meisyou.Text.Length == 0)
            {
                MessageBox.Show("名称を入力してください。");
                tb_kubun_meisyou.Focus();
            }
            //書込み
            else
            {
                tss.GetUser();
                bool bl_tss;
                bl_tss = tss.OracleInsert("INSERT INTO tss_kubun_meisyou_m (kubun_meisyou_cd,kubun_name,bikou,create_user_cd) VALUES ('" + tb_kubun_meisyou_cd.Text + "','" + tb_kubun_meisyou.Text + "','" + tb_bikou.Text + "','" + tss.user_cd + "')");
                if (bl_tss != true)
                {
                    tss.ErrorLogWrite(tss.UserID, "区分名称マスタ／登録", "登録ボタン押下時のOracleInsert");
                    MessageBox.Show("書込みでエラーが発生しました。処理を中止します。");
                    this.Close();
                }
                kubun_meisyou_m_disp();
                gamen_clear();
                tb_kubun_meisyou_cd.Focus();
            }
        }

        private bool kubun_meisyou_cd_check()
        {
            bool bl = true; //戻り値

            //入力された文字列を00形式にする
            int i;
            if (int.TryParse(tb_kubun_meisyou_cd.Text, out i))
            {
                //変換出来たら、iにその数値が入る
                tb_kubun_meisyou_cd.Text = i.ToString("00");
                //使用できるコードかチェック
                DataTable dt_work = new DataTable();
                dt_work = tss.OracleSelect("select * from TSS_KUBUN_MEISYOU_M where kubun_meisyou_cd = '" + tb_kubun_meisyou_cd.Text + "'");
                if (dt_work.Rows.Count != 0)
                {
                    MessageBox.Show("使用できないコードです。");
                    bl = false;
                }
            }
            else
            {
                MessageBox.Show("区分名称コードは数字のみです。");
                bl = false;
            }
            return bl;
        }

        private void status_disp()
        {
            TssSystemLibrary tss = new TssSystemLibrary();
            tss.GetSystemSetting();
            tss.GetUser();
            ss_status.Items.Add(tss.system_name);
            ss_status.Items.Add(tss.system_version);
            ss_status.Items.Add(tss.user_name);
            ss_status.Items.Add(tss.kengen1 + tss.kengen2 + tss.kengen3 + tss.kengen4 + tss.kengen5 + tss.kengen6);
        }

        private void kubun_meisyou_m_disp()
        {
            dt = tss.OracleSelect("select * from TSS_KUBUN_MEISYOU_M order by kubun_meisyou_cd asc");
            dgv_kubun.DataSource = dt;
        }

        private void btn_syuuryou_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void gamen_clear()
        {
            tb_kubun_meisyou_cd.Text = "";
            tb_kubun_meisyou.Text = "";
            tb_bikou.Text = "";
        }

        private void tb_kubun_meisyou_cd_Validating(object sender, CancelEventArgs e)
        {
            if (kubun_meisyou_cd_check() != true)
            {
                tb_kubun_meisyou_cd.Focus();
            }

        }
    }
}
