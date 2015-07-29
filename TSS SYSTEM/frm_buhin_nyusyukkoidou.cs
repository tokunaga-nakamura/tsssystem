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
            //モードによってフォームの表示内容を変える
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

            //SEQ連番マスタから連番を取得して、+1した値を表示させる
            DataTable dt_work = new DataTable();

            dt_work = tss.OracleSelect("select * from TSS_SEQ_M where seq_m_cd = '06' ");

            if (dt_work.Rows.Count != 0)
            {
                int i = int.Parse(dt_work.Rows[0][1].ToString());
                tb_seq.Text = (i + 1).ToString();
            }
            else
            {
                tb_seq.Text = "0";
            }


            dgv_nyusyukkoidou.Rows[0].Cells[0].Value = "001";

            
            
           

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


        
        //取引先コード入力時の処理
        private void tb_torihikisaki_cd_Validating(object sender, CancelEventArgs e)
        {
            //空白の場合はOKとする
            if (tb_torihikisaki_cd.Text != "")
            {
                if (chk_torihikisaki_cd() != true)
                {
                    MessageBox.Show("取引先コードに異常があります。");
                    e.Cancel = true;
                }
                else
                {
                    tb_torihikisaki_name.Text = get_torihikisaki_name(tb_torihikisaki_cd.Text);
                }
            }
        }

        private bool chk_torihikisaki_cd()
        {
            bool bl = true; //戻り値
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_torihikisaki_m where torihikisaki_cd  = '" + tb_torihikisaki_cd.Text + "'");
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



        //部品コード入力時の処理
        private void tb_buhin_cd_Validating(object sender, CancelEventArgs e)
        {
            //空白の場合はOKとする
            if (tb_buhin_cd.Text != "")
            {
                if (chk_buhin_cd() != true)
                {
                    MessageBox.Show("部品コードに異常があります。");
                    e.Cancel = true;
                }
                else
                {
                    tb_buhin_name.Text = get_buhin_name(tb_buhin_cd.Text);
                }
            }
        }
        private bool chk_buhin_cd()
        {
            bool bl = true; //戻り値
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_buhin_m where buhin_cd  = '" + tb_buhin_cd.Text + "'");
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

        private string get_buhin_name(string in_buhin_cd)
        {
            string out_buhin_name = "";  //戻り値用
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_buhin_m where buhin_cd = '" + in_buhin_cd + "'");
            if (dt_work.Rows.Count <= 0)
            {
                out_buhin_name = "";
            }
            else
            {
                out_buhin_name = dt_work.Rows[0]["buhin_name"].ToString();
            }
            return out_buhin_name;
        }

        private void tb_zaiko_kbn_DoubleClick(object sender, EventArgs e)
        {
            this.tb_zaiko_kbn.Text = tss.kubun_cd_select("01");
            this.tb_zaiko_kbn_name.Text = tss.kubun_name_select("01", tb_zaiko_kbn.Text);
        }

        private void tb_zaiko_kbn_Validating(object sender, CancelEventArgs e)
        {
            if (tb_zaiko_kbn.Text == "02")
            {
                //tb_zaiko_kbn_name.Text = "ロット";
                tb_juchu_cd1.Enabled = true;
                tb_juchu_cd2.Enabled = true;
            }
            if (tb_zaiko_kbn.Text == "01")
            {
                //tb_zaiko_kbn_name.Text = "フリー";
                tb_juchu_cd1.Enabled = false;
                tb_juchu_cd2.Enabled = false;
            }
        }

        private void btn_touroku_Click(object sender, EventArgs e)
        {
            tss.GetUser();  //ユーザー情報の取得
            int int_insert = 0; //新規レコード数
            int int_update = 0; //更新レコード数
           
            
            if(str_mode == "1")
            {
                MessageBox.Show("1です");
                //DataRow dr in dt_kubun_m.Rows
                 
                 DataTable dtTmp = (DataTable)this.dgv_nyusyukkoidou.DataSource;

                    //レコードの行数分ループして日付を付与

                    int dttmpc = dtTmp.Rows.Count;

                    for (int i = 0; i < dttmpc; i++)
                    {
                        dtTmp.Rows[i][0] = dtp_buhin_syori_date.Value.ToShortDateString();
                        dtTmp.Rows[i][1] = tb_seq.Text.ToString();
                    }    
                    
                    
                    
                    //Insert
                 //bool bl = tss.OracleInsert("INSERT INTO tss_buhinnyusyukko_m (buhin_syori_kbn,buhin_syori_no,seq,buhin_syori_date,buhin_cd,kubun_cd,zaiko_kbn,torihikisaki_cd,juchu_cd1,juchu_cd2,suryou,idousaki_zaiko_kbn,idousaki_torihikisaki_cd,idousaki_juchu_cd1,idousaki_juchu_cd2,denpyou_no,barcode,bikou,create_user_cd,create_datetime) VALUES ('"
                 //                           + "06" + "','" + tb_seq.ToString() + "','" + .ToString() + "','" + dr["bikou"].ToString() + "','" + tss.user_cd + "',SYSDATE)");











                 if (bl != true)
                 {
                     tss.ErrorLogWrite(tss.user_cd, "区分名称マスタ／登録", "登録ボタン押下時のOracleInsert");
                     MessageBox.Show("書込みでエラーが発生しました。" + Environment.NewLine + "処理を中止します。");
                     this.Close();
                 }
                 else
                 {
                     int_insert++;
                 }
            }
   





            

            if (str_mode == "2")
            {
                MessageBox.Show("2です");
            }

            if (str_mode == "3")
            {
                MessageBox.Show("3です");
            }

        }

        //データグリッドビュー改行時、SEQに3桁の連番を付ける
        private void dgv_nyusyukkoidou_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int j = dgv_nyusyukkoidou.NewRowIndex;
            
            if(j <=9)
            {
                dgv_nyusyukkoidou.Rows[j - 1].Cells[0].Value = "00" + j;

            }
            if (j <= 99 &&  j > 9)
            {
                dgv_nyusyukkoidou.Rows[j - 1].Cells[0].Value = "0" + j;

            }
            if (j > 99)
            {
                dgv_nyusyukkoidou.Rows[j - 1].Cells[0].Value = j;

            }

        }



    }
}
