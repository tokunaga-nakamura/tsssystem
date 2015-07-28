using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Configuration;     //app.config用
using System.IO;                //テキストファイル読み込み用
using Oracle.DataAccess.Client; //Oracle用



namespace TSS_SYSTEM
{
    public partial class frm_menu : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();


        public frm_menu()
        {
            InitializeComponent();
        }

        private void menu_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            frm_login frm_login = new frm_login();
            frm_login.ShowDialog(this);
            frm_login.Dispose();
            //ここから先のコードが実行されるということは、ログイン成功ということ
            string TempPath = ConfigurationManager.AppSettings["TempPath"];   //テンポラリフォルダのパスの取得
            string username;
            using (StreamReader sr = new StreamReader(TempPath + "user.txt"))
            {
                username = sr.ReadToEnd();
            }
            if (username == "notlogin") //ユーザー名にnotloginという文字列が入っていたら終了する
            {
                Application.Exit();
            }
        }

        private void frm_menu_Activated(object sender, EventArgs e)
        {
            this.Opacity = 1;
            status_disp();
        }

        private void btn_hardcopy_Click(object sender, EventArgs e)
        {
            //Altキー＋Print Screenキーの送信
            SendKeys.SendWait("%{PRTSC}");
        }

        private void btn_syuuryou_Click(object sender, EventArgs e)
        {
            string TempPath = ConfigurationManager.AppSettings["TempPath"];   //テンポラリフォルダのパスの取得
            //ログアウト情報更新
            string usercd;
            using (StreamReader sr = new StreamReader(TempPath + "user.txt"))
            {
                usercd = sr.ReadToEnd();
            }
            TssSystemLibrary tsslib = new TssSystemLibrary();
            string sql = "UPDATE tss_user_m SET login_flg = '0',logout_datetime = sysdate WHERE user_cd = '" + usercd + "'";
            tsslib.OracleUpdate(sql);

            Application.Exit();
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            //ユーザーコードの取得
            string TempPath = ConfigurationManager.AppSettings["TempPath"];   //テンポラリフォルダのパスの取得
            //まずログアウト情報更新
            string usercd;
            using (StreamReader sr = new StreamReader(TempPath + "user.txt"))
            {
                usercd = sr.ReadToEnd();
            }
            TssSystemLibrary tsslib = new TssSystemLibrary();
            string sql = "UPDATE tss_user_m SET login_flg = '0',logout_datetime = sysdate WHERE user_cd = '" + usercd + "'";
            tsslib.OracleUpdate(sql);
            //ログイン画面へ
            this.Opacity = 0;
            frm_login frm_login = new frm_login();
            frm_login.ShowDialog(this);
            frm_login.Dispose();
            //ここから先のコードが実行されるということは、ログイン成功ということ
            //ログインユーザーIDの取得・表示
            using (StreamReader sr = new StreamReader(TempPath + "user.txt"))
            {
                usercd = sr.ReadToEnd();
            }
            if (usercd == "notlogin") //ユーザー名にnotloginという文字列が入っていたら終了する
            {
                Application.Exit();
            }
        }
        private void status_disp()
        {
            TssSystemLibrary tss = new TssSystemLibrary();
            tss.GetSystemSetting();
            tss.GetUser();
            ss_status.Items.Clear();    //追加する前にクリアする
            ss_status.Items.Add(tss.system_name);
            ss_status.Items.Add(tss.system_version);
            ss_status.Items.Add(tss.user_name);
            ss_status.Items.Add(tss.kengen1+tss.kengen2+tss.kengen3+tss.kengen4+tss.kengen5+tss.kengen6);
        }



        private void btn_mst_table_Click(object sender, EventArgs e)
        {
            frm_table_maintenance frm_mm = new frm_table_maintenance();
            frm_mm.ShowDialog(this);
            frm_mm.Dispose();

        }

        private void btn_kubun_meisyou_m_Click(object sender, EventArgs e)
        {
            frm_kubun_meisyou_m frm_kmm = new frm_kubun_meisyou_m();
            frm_kmm.ShowDialog(this);
            frm_kmm.Dispose();
        }

        private void btn_kubun_m_Click(object sender, EventArgs e)
        {
            frm_kubun_m frm_kmm = new frm_kubun_m();
            frm_kmm.ShowDialog(this);
            frm_kmm.Dispose();
        }

        private void btn_juchuu_nyuuryoku_Click(object sender, EventArgs e)
        {
            frm_juchuu_nyuuryoku frm_jn = new frm_juchuu_nyuuryoku();
            frm_jn.ShowDialog(this);
            frm_jn.Dispose();
        }

        private void btn_seihin_m_Click(object sender, EventArgs e)
        {
            frm_seihin_m frm_sm = new frm_seihin_m();
            frm_sm.ShowDialog(this);
            frm_sm.Dispose();
        }

        private void btn_torihikisaki_m_Click(object sender, EventArgs e)
        {
            frm_torihikisaki_m frm_tm = new frm_torihikisaki_m();
            frm_tm.ShowDialog(this);
            frm_tm.Dispose();
        }

        private void btn_buhin_m_Click(object sender, EventArgs e)
        {
            frm_buhin_m frm_bm = new frm_buhin_m();
            frm_bm.ShowDialog(this);
            frm_bm.Dispose();
        }

        private void btn_bank_m_Click(object sender, EventArgs e)
        {
            frm_bank_m frm_bam = new frm_bank_m();
            frm_bam.ShowDialog(this);
            frm_bam.Dispose();
        }

        private void btn_buhin_kensaku_Click(object sender, EventArgs e)
        {
            //検索画面へ
            tss.search_buhin("1", "");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frm_buhin_nyusyukkoidou frm_bnsi = new frm_buhin_nyusyukkoidou();
            frm_bnsi.ShowDialog(this);
            frm_bnsi.Dispose();
        }
    }
}
