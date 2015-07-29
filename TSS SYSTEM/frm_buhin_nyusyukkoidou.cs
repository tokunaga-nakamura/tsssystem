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
    /// <para>プロパティ str_mode 1:入庫モード　 2:出庫モード　3:移動モード</para>
    /// </summary>
    
    
    public partial class frm_buhin_nyusyukkoidou : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();
        DataTable dt_m = new DataTable();

        //親画面から参照できるプロパティを作成
        public string fld_mode; 　 //画面モード
        public string fld_cd;     //選択された部品コード
        
        public string str_mode
        {
            get
            {
                return fld_mode;
            }
            set
            {
                fld_mode = value;
            }
        }

        public string str_cd
        {
            get
            {
                return fld_cd;
            }
            set
            {
                fld_cd = value;
            }
        }

        public frm_buhin_nyusyukkoidou()
        {
            InitializeComponent();
        }

        private void frm_buhin_nyusyukkoidou_Load(object sender, EventArgs e)
        {
            switch (str_mode)
            {
                case "1":
                    //入庫モード
                    mode1();
                    break;

                case "2":
                    //出庫モード
                    mode2();
                    break;
                
                case "3":
                    //移動モード
                    mode3();
                    break;
                
                default:
                    MessageBox.Show("画面モードのプロパティに異常があります。処理を中止します。");
                    //form_close_false();
                    break;
            }

        }

        private void mode1()
        {
            label3.Text = "入庫処理";
            textBox10.Visible = false;
            textBox13.Visible = false;
            textBox15.Visible = false;
            textBox17.Visible = false;
            tb_idousaki_zaiko_kbn.Visible = false;
            tb_idousaki_torihikisaki_cd.Visible = false;
            tb_idousaki_juchu_cd1.Visible = false;
            tb_idousaki_juchu_cd2.Visible = false;
            tb_idousaki_zaiko_kbn_name.Visible = false;
            tb_idousaki_torihikisaki_name.Visible = false;

        }

        private void mode2()
        {
            label3.Text = "出庫処理";
            textBox10.Visible = false;
            textBox13.Visible = false;
            textBox15.Visible = false;
            textBox17.Visible = false;
            tb_idousaki_zaiko_kbn.Visible = false;
            tb_idousaki_torihikisaki_cd.Visible = false;
            tb_idousaki_juchu_cd1.Visible = false;
            tb_idousaki_juchu_cd2.Visible = false;
            tb_idousaki_zaiko_kbn_name.Visible = false;
            tb_idousaki_torihikisaki_name.Visible = false;
           
        }

        private void mode3()
        {
            label3.Text = "移動処理";
            textBox10.Visible = true;
            textBox13.Visible = true;
            textBox15.Visible = true;
            textBox17.Visible = true;
            tb_idousaki_zaiko_kbn.Visible = true;
            tb_idousaki_torihikisaki_cd.Visible = true;
            tb_idousaki_juchu_cd1.Visible = true;
            tb_idousaki_juchu_cd2.Visible = true;
            tb_idousaki_zaiko_kbn_name.Visible = true;
            tb_idousaki_torihikisaki_name.Visible = true;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btn_syuuryou_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tb_torihikisaki_cd_Leave(object sender, EventArgs e)
        {

        }
    }
}
