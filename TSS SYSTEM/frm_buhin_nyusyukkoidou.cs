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

        string w_str = "";
        


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

        public string in_cd
        {
            get
            {
                return in_cd;
            }
            set
            {
                in_cd = value;
            }
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
            double w_seq;
            w_seq = tss.GetSeq(w_str);
            if(w_seq == 0)
            {
                MessageBox.Show("連番マスタに異常があります。処理を中止します。");
                this.Close();
            }
            tb_seq.Text = (w_seq).ToString("0000000000");
             
            dgv_nyusyukkoidou.Rows[0].Cells[0].Value = "001";

            //データグリッドビューの部品名は編集不可
            dgv_nyusyukkoidou.Columns[2].ReadOnly = true;

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
            w_str = "01";

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
            w_str = "02";
           
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
            w_str = "03";

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
            bool bl = true; //戻り値
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_torihikisaki_m where torihikisaki_cd  = '" + tb_torihikisaki_cd.Text + "'");
            if (dt_work.Rows.Count <= 0)
            {
                //無し
                bl = false;
                MessageBox.Show("入力された取引先コードが存在しません。取引先マスタに登録してください。");
            }
            else
            {
                //既存データ有
                tb_torihikisaki_name.Text = get_torihikisaki_name(tb_torihikisaki_cd.Text);
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

       //登録ボタンが押された時の処理//////////////////////////////////////////////////////////////////////////////////////
        private void btn_touroku_Click(object sender, EventArgs e)
        {

            DataTable dt_work = new DataTable();

            //登録前に全ての項目をチェック
            //伝票番号
            if (chk_denpyou_no() == false)
            {
                MessageBox.Show("伝票番号の値が異常です");
                //tb_torihikisaki_cd.Focus();
                return;
            }
            //取引先コード
            if (chk_torihikisaki_cd() == false)
            {
                MessageBox.Show("取引先コードの値が異常です");
                //tb_torihikisaki_cd.Focus();
                return;
            }

            //データグリッドビューの中を1行ずつループしてチェック
            int dgvrc = dgv_nyusyukkoidou.Rows.Count;

            tss.GetUser();  //ユーザー情報の取得

            //テキストボックスとデータグリッドビューの入力内容チェック
            for (int i = 0; i < dgvrc - 1; i++)
            {
                if (dgv_nyusyukkoidou.Rows[i].Cells[1].Value == null)
                {
                    MessageBox.Show("部品コードを入力してください");
                    return;
                }

                if (dgv_nyusyukkoidou.Rows[i].Cells[3].Value == null)
                {
                    MessageBox.Show("在庫区分を入力してください");
                    return;
                }

                if (dgv_nyusyukkoidou.Rows[i].Cells[3].Value.ToString() == "02" && dgv_nyusyukkoidou.Rows[0].Cells[4].Value == null)
                {
                    MessageBox.Show("受注コード1を入力してください");
                    return;
                }
                if (dgv_nyusyukkoidou.Rows[i].Cells[6].Value == null)
                {
                    MessageBox.Show("数量を入力してください");
                    return;
                }
                if (dgv_nyusyukkoidou.Rows[i].Cells[3].Value.ToString() == "01" && dgv_nyusyukkoidou.Rows[i].Cells[4].Value != null && dgv_nyusyukkoidou.Rows[i].Cells[5].Value != null)
                {
                    MessageBox.Show("在庫区分01の時は、受注コード1、2に何も入力しないでください。");
                    return;
                }
                if (dgv_nyusyukkoidou.Rows[i].Cells[3].Value.ToString() == "01" && dgv_nyusyukkoidou.Rows[i].Cells[4].Value == null || dgv_nyusyukkoidou.Rows[i].Cells[5].Value == null)
                {
                    dgv_nyusyukkoidou.Rows[i].Cells[4].Value = 0;
                    dgv_nyusyukkoidou.Rows[i].Cells[5].Value = 0;
                }
            }

            if (str_mode == "1")　//入庫モード
            {
  
                //レコードの行数分ループしてインサート
                int dgvrc2= dgv_nyusyukkoidou.Rows.Count;

                for (int i = 0; i < dgvrc - 1; i++)
                {
                    bool bl6 = tss.OracleInsert("INSERT INTO tss_buhin_nyusyukko_m (buhin_syori_kbn,buhin_syori_no,seq,buhin_syori_date,buhin_cd,zaiko_kbn,torihikisaki_cd,juchu_cd1,juchu_cd2,suryou,idousaki_zaiko_kbn,idousaki_torihikisaki_cd,idousaki_juchu_cd1,idousaki_juchu_cd2,denpyou_no,barcode,bikou,create_user_cd,create_datetime) VALUES ('"
                                        + "01" + "','"
                                        + tb_seq.Text.ToString() + "','"
                                        + dgv_nyusyukkoidou.Rows[i].Cells[0].Value.ToString() + "','"
                                        + dtp_buhin_syori_date.Value.ToShortDateString() + "','"
                                        + dgv_nyusyukkoidou.Rows[i].Cells[1].Value.ToString() + "','"
                                        + dgv_nyusyukkoidou.Rows[i].Cells[3].Value.ToString() + "','"
                                        + tb_torihikisaki_cd.Text.ToString() + "','"
                                        + dgv_nyusyukkoidou.Rows[i].Cells[4].Value.ToString() + "','"
                                        + dgv_nyusyukkoidou.Rows[i].Cells[5].Value.ToString() + "','"
                                        + dgv_nyusyukkoidou.Rows[i].Cells[6].Value.ToString() + "','"
                                        + tb_idousaki_zaiko_kbn.Text.ToString() + "','"
                                        + tb_idousaki_torihikisaki_cd.Text.ToString() + "','"
                                        + tb_idousaki_juchu_cd1.Text.ToString() + "','"
                                        + tb_idousaki_juchu_cd2.Text.ToString() + "','"
                                        + tb_denpyou_no.Text.ToString() + "','"
                                        + "" + "','"
                                        + "" + "','"
                                        + tss.user_cd + "',SYSDATE)");

                    if (bl6 != true)
                    {
                        tss.ErrorLogWrite(tss.user_cd, "入出庫移動／登録", "登録ボタン押下時のOracleInsert");
                        MessageBox.Show("登録でエラーが発生しました。" + Environment.NewLine + "処理を中止します。");
                        this.Close();
                    }
                }
                


                //連番マスタの更新
                bool bl2 = tss.OracleUpdate("UPDATE TSS_SEQ_M SET SEQ = '" + tb_seq.Text.ToString() + "',UPDATE_DATETIME = SYSDATE WHERE SEQ_M_CD = '01'");


                //部品在庫マスタの更新
                //既存の区分があるかチェック
                int j = dgv_nyusyukkoidou.Rows.Count;
                DataTable dt_work5 = new DataTable();

                for (int i = 0; i < j - 1; i++)
                {
                    dt_work5 = tss.OracleSelect("select * from tss_buhin_zaiko_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'and buhin_cd = '" + dgv_nyusyukkoidou.Rows[i].Cells[1].Value.ToString() + "'and zaiko_kbn = '" + dgv_nyusyukkoidou.Rows[i].Cells[3].Value.ToString() + "' and juchu_cd1 = '" + dgv_nyusyukkoidou.Rows[i].Cells[4].Value.ToString() + "'and juchu_cd2 = '" + dgv_nyusyukkoidou.Rows[i].Cells[5].Value.ToString() + "'");


                    if (dt_work5.Rows.Count == 0)
                    {
                        
                        bool bl3 = tss.OracleInsert("insert into tss_buhin_zaiko_m (buhin_cd, zaiko_kbn,torihikisaki_cd, juchu_cd1, juchu_cd2, zaiko_su,create_user_cd,create_datetime) values ('"
                                                  + dgv_nyusyukkoidou.Rows[i].Cells[1].Value.ToString() + "','"
                                                  + dgv_nyusyukkoidou.Rows[i].Cells[3].Value.ToString() + "','"
                                                  + tb_torihikisaki_cd.Text.ToString() + "','"
                                                  + dgv_nyusyukkoidou.Rows[i].Cells[4].Value.ToString() + "','"
                                                  + dgv_nyusyukkoidou.Rows[i].Cells[5].Value.ToString() + "','"
                                                  + dgv_nyusyukkoidou.Rows[i].Cells[6].Value.ToString() + "','"
                                                  + tss.user_cd + "',SYSDATE)");
                    }

                    if (dt_work5.Rows.Count != 0)
                    {
                        int zaikosu1 = int.Parse(dt_work5.Rows[0][5].ToString());
                        int zaikosu2 = int.Parse(dgv_nyusyukkoidou.Rows[i].Cells[6].Value.ToString());

                        int zaikosu3 = zaikosu1 + zaikosu2;

                        bool bl5 = tss.OracleUpdate("UPDATE TSS_BUHIN_ZAIKO_M SET ZAIKO_SU = '" + zaikosu3 + "',UPDATE_DATETIME = SYSDATE WHERE buhin_cd = '" + dgv_nyusyukkoidou.Rows[i].Cells[1].Value.ToString() + "' and juchu_cd1 = '" + dgv_nyusyukkoidou.Rows[i].Cells[4].Value.ToString() + "' and juchu_cd2 = '" + dgv_nyusyukkoidou.Rows[i].Cells[5].Value.ToString() + "'");

                    }


                }
                MessageBox.Show("入庫処理されました。");
                this.Close();



                if (str_mode == "2")
                {
                    //レコードの行数分ループしてインサート


                }


                    MessageBox.Show("出庫処理されました。");
                    this.Close();
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


        //伝票番号チェック用
        private bool chk_denpyou_no()
        {
            bool bl = true; //戻り値用

            if (tss.StringByte(tb_denpyou_no.Text) > 16)
            {
                bl = false;
            }
            return bl;
        }
        
        //取引先コードチェック用
        private bool chk_torihikisaki_cd()
        {
            bool bl = true; //戻り値用

            if (tb_torihikisaki_cd.Text == null || tb_torihikisaki_cd.Text.Length > 6 || tb_torihikisaki_cd.Text.Length < 6)
            {
                bl = false;
            }
            return bl;
        }


        //データグリッドビューのセルの値が変わった時のイベント
        private void dgv_nyusyukkoidou_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            //部品コードが入力されたならば、部品名を部品マスターから取得して表示
            if (dgv.Columns[e.ColumnIndex].Index == 1 && dgv.CurrentCell.Value.ToString() != null)
            {
                int i = e.RowIndex;
                

                DataTable dtTmp = (DataTable)dgv_nyusyukkoidou.DataSource;

                //部品コードをキーに、部品名を引っ張ってくる

                DataTable dt_work = new DataTable();
                int j = dt_work.Rows.Count;
                dt_work = tss.OracleSelect("select * from tss_buhin_m where buhin_cd = '" + dgv.CurrentCell.Value.ToString() + "'");
                if (dt_work.Rows.Count <= 0)
                {
                    MessageBox.Show("この部品コードは登録されていません。部品登録してください。");
                    dgv.Rows[i].Cells[2].Value = "";
                }
                else
                {
                    dgv.Rows[i].Cells[2].Value = dt_work.Rows[j][1].ToString();
                }
                //
                return;
            }
        }
 

    }
}
