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
    public partial class frm_seihin_kousei_m : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();
        DataTable dt_m = new DataTable();

        //string w_str = "";
        
        
        public frm_seihin_kousei_m()
        {
            InitializeComponent();
        }

        private void frm_seihin_kousei_m_Load(object sender, EventArgs e)
        {
            

               // dgv_seihin_kousei.Enabled = false;
         
        }

        private string get_seihin_name(string in_cd)
        {
            string out_name = "";  //戻り値用
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_seihin_m where seihin_cd = '" + in_cd + "'");
            if (dt_work.Rows.Count <= 0)
            {
                out_name = "";
            }
            else
            {
                out_name = dt_work.Rows[0]["seihin_name"].ToString();
            }
            return out_name;
        }

        private bool chk_seihin_cd()
        {
            bool bl = true; //戻り値
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_seihin_m where seihin_cd  = '" + tb_seihin_cd.Text.ToString() + "'");
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

        private void tb_seihin_cd_DoubleClick(object sender, EventArgs e)
        {
            //選択画面へ
            string w_cd;
            w_cd = tss.search_seihin("2", "");
            if (w_cd != "")
            {
                tb_seihin_cd.Text = w_cd;
                tb_seihin_name.Text = get_seihin_name(tb_seihin_cd.Text);
                //tb_juchu_su.Focus();
            }
        }

        private void tb_seihin_cd_Validating(object sender, CancelEventArgs e)
        {
            //空白の場合はOKとする
            if (tb_seihin_cd.Text != "")
            {
                if (chk_seihin_cd() != true)
                {
                    MessageBox.Show("入力されている製品コードは存在しません。");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    tb_seihin_name.Text = get_seihin_name(tb_seihin_cd.Text);

                    
                }
            }

            //string out_seihin_kousei_name = "";  //戻り値用
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select seihin_kousei_no,seihin_kousei_name from tss_seihin_kousei_name_m where seihin_cd = '" + tb_seihin_cd.Text.ToString() + "'");
            if (dt_work.Rows.Count <= 0)
            {
                MessageBox.Show("製品構成名称に登録がありません。新規に登録します。");
                tb_seihin_kousei_no.Text = "01";
                tb_seihin_kousei_name.Text = "初回登録";
            }
            else
            {
                dgv_seihin_kousei_name.DataSource = dt_work;
                dgv_seihin_kousei_name.Columns[0].HeaderText = "部品構成番号";
                dgv_seihin_kousei_name.Columns[1].HeaderText = "部品構成名称";

                tb_seihin_kousei_no.Clear();
                tb_seihin_kousei_name.Clear();

                //out_seihin_kousei_name = dt_work.Rows[0]["seihin_kousei_name"].ToString();
            }
            //return out_seihin_kousei_name;




        }

        private string get_seihin_kousei_name(string in_seihin_cd, string in_seihin_kousei_no)
        {
            string out_seihin_kousei_name = "";  //戻り値用
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_seihin_kousei_name_m where seihin_cd = '" + in_seihin_cd + "' and seihin_kousei_no = '" + in_seihin_kousei_no + "'");
            if (dt_work.Rows.Count <= 0)
            {
                out_seihin_kousei_name = "";
            }
            else
            {
                out_seihin_kousei_name = dt_work.Rows[0]["seihin_kousei_name"].ToString();
            }
            return out_seihin_kousei_name;
        }

        private bool chk_seihin_kousei_no()
        {
            bool bl = true; //戻り値
            //空白のまま登録を許可する（製品の登録時点でまだ部品構成を作っていない場合を考慮）
            if (tb_seihin_kousei_no.Text.Length != 0)
            {
                DataTable dt_work = new DataTable();
                dt_work = tss.OracleSelect("select * from tss_seihin_kousei_name_m where seihin_cd  = '" + tb_seihin_cd.Text.ToString() + "' and seihin_kousei_no = '" + tb_seihin_kousei_no.Text.ToString() + "'");
                if (dt_work.Rows.Count <= 0)
                {
                    //無し
                    bl = false;
                }
                else
                {
                    //既存データ有
                }
            }
            return bl;
        }


        private void dgv_seihin_kousei_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            //部品コードが入力されたならば、部品名を部品マスターから取得して表示
            if (dgv.Columns[e.ColumnIndex].Index == 1 && dgv.CurrentCell.Value.ToString() != null)
            {
                int i = e.RowIndex;

                DataTable dtTmp = (DataTable)dgv_seihin_kousei.DataSource;

                //部品コードをキーに、部品名を引っ張ってくる

                DataTable dt_work = new DataTable();
                int j = dt_work.Rows.Count;
                dt_work = tss.OracleSelect("select * from tss_buhin_m where buhin_cd = '" + dgv.CurrentCell.Value.ToString() + "'");
                if (dt_work.Rows.Count <= 0)
                {
                    MessageBox.Show("この部品コードは登録されていません。部品登録してください。");
                    dgv.Rows[i].Cells[2].Value = "";
                    dgv_seihin_kousei.Focus();
                    dgv_seihin_kousei.CurrentCell = dgv_seihin_kousei[i, 2];
                }
                else
                {
                    dgv.Rows[i].Cells[2].Value = dt_work.Rows[j][1].ToString();
                }

                return;
            }
        }

        private void btn_sentaku_Click(object sender, EventArgs e)
        {
            int i = dgv_seihin_kousei_name.CurrentCell.RowIndex;

            tb_seihin_kousei_no.Text = dgv_seihin_kousei_name.Rows[i].Cells[0].Value.ToString();
            tb_seihin_kousei_name.Text = dgv_seihin_kousei_name.Rows[i].Cells[1].Value.ToString();
        }

        private void tb_seihin_kousei_no_Validating(object sender, CancelEventArgs e)
        {
            if (tb_seihin_kousei_no.Text.ToString() != "")
            {
                dgv_seihin_kousei.Enabled = true;
            }


            





        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    DataTable dt_work = new DataTable();
        //    dt_work = tss.OracleSelect("select buhin_level,t.BUHIN_CD,s1.BUHIN_NAME,SIYOU_SU,t.GOKAN_BUHIN_CD,s2.BUHIN_NAME 互換部品名,t.bikou from TSS_SEIHIN_KOUSEI_M t LEFT OUTER JOIN TSS_BUHIN_M s1 ON t.BUHIN_CD = s1.BUHIN_CD LEFT OUTER JOIN TSS_BUHIN_M s2 ON t.GOKAN_BUHIN_CD = s2.BUHIN_CD WHERE seihin_cd = '" + tb_seihin_cd.Text.ToString() + "' and seihin_kousei_no = '" + tb_seihin_kousei_no.Text.ToString() + "'");
            
        //    //dt_work = tss.OracleSelect("select buhin_level,t.BUHIN_CD,s1.BUHIN_NAME,SIYOU_SU,t.OYA_BUHIN_CD,s2.BUHIN_NAME 親部品名,t.GOKAN_BUHIN_CD,s3.BUHIN_NAME 互換部品名,t.bikou from TSS_SEIHIN_KOUSEI_M t LEFT OUTER JOIN TSS_BUHIN_M s1 ON t.BUHIN_CD = s1.BUHIN_CD LEFT OUTER JOIN TSS_BUHIN_M s2 ON t.OYA_BUHIN_CD = s2.BUHIN_CD LEFT OUTER JOIN TSS_BUHIN_M s3 ON t.GOKAN_BUHIN_CD = s3.BUHIN_CD WHERE seihin_cd = '" + tb_seihin_cd.Text.ToString() + "' and seihin_kousei_no = '" + tb_seihin_kousei_no.Text.ToString() + "'");
            
        //    if (dt_work.Rows.Count <= 0)
        //    {
        //        MessageBox.Show("製品構成に登録がありません。新規に登録してください。");
        //        dgv_seihin_kousei.DataSource = dt_work;
        //        dgv_seihin_kousei.Columns[0].HeaderText = "部品レベル";
        //        dgv_seihin_kousei.Columns[1].HeaderText = "部品コード";
        //        dgv_seihin_kousei.Columns[2].HeaderText = "部品名";
        //        dgv_seihin_kousei.Columns[3].HeaderText = "使用数";
        //        dgv_seihin_kousei.Columns[4].HeaderText = "互換部品コード";
        //        dgv_seihin_kousei.Columns[5].HeaderText = "互換部品名";
        //        dgv_seihin_kousei.Columns[6].HeaderText = "備考";
        //        //tb_seihin_kousei_no.Text = "01";
        //        //tb_seihin_kousei_name.Text = "初回登録";
        //    }
        //    else
        //    {
        //        dgv_seihin_kousei.DataSource = dt_work;

        //        dgv_seihin_kousei.Columns[0].HeaderText = "部品レベル";
        //        dgv_seihin_kousei.Columns[1].HeaderText = "部品コード";
        //        dgv_seihin_kousei.Columns[2].HeaderText = "部品名";
        //        dgv_seihin_kousei.Columns[3].HeaderText = "使用数";
        //        //dgv_seihin_kousei.Columns[4].HeaderText = "親部品コード";
        //        //dgv_seihin_kousei.Columns[5].HeaderText = "親部品名";
        //        dgv_seihin_kousei.Columns[4].HeaderText = "互換部品コード";
        //        dgv_seihin_kousei.Columns[5].HeaderText = "互換部品名";
        //        dgv_seihin_kousei.Columns[6].HeaderText = "備考";

                

        //        //tb_seihin_kousei_no.Clear();
        //        //tb_seihin_kousei_name.Clear();

        //        //out_seihin_kousei_name = dt_work.Rows[0]["seihin_kousei_name"].ToString();
        //    }
        //}

        private void tb_seihin_kousei_no_TextChanged(object sender, EventArgs e)
        {
            tb_seihin_kousei_name.Clear();
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select buhin_level,t.BUHIN_CD,s1.BUHIN_NAME,SIYOU_SU,t.GOKAN_BUHIN_CD,s2.BUHIN_NAME 互換部品名,t.bikou from TSS_SEIHIN_KOUSEI_M t LEFT OUTER JOIN TSS_BUHIN_M s1 ON t.BUHIN_CD = s1.BUHIN_CD LEFT OUTER JOIN TSS_BUHIN_M s2 ON t.GOKAN_BUHIN_CD = s2.BUHIN_CD WHERE seihin_cd = '" + tb_seihin_cd.Text.ToString() + "' and seihin_kousei_no = '" + tb_seihin_kousei_no.Text.ToString() + "'");

            //dt_work = tss.OracleSelect("select buhin_level,t.BUHIN_CD,s1.BUHIN_NAME,SIYOU_SU,t.OYA_BUHIN_CD,s2.BUHIN_NAME 親部品名,t.GOKAN_BUHIN_CD,s3.BUHIN_NAME 互換部品名,t.bikou from TSS_SEIHIN_KOUSEI_M t LEFT OUTER JOIN TSS_BUHIN_M s1 ON t.BUHIN_CD = s1.BUHIN_CD LEFT OUTER JOIN TSS_BUHIN_M s2 ON t.OYA_BUHIN_CD = s2.BUHIN_CD LEFT OUTER JOIN TSS_BUHIN_M s3 ON t.GOKAN_BUHIN_CD = s3.BUHIN_CD WHERE seihin_cd = '" + tb_seihin_cd.Text.ToString() + "' and seihin_kousei_no = '" + tb_seihin_kousei_no.Text.ToString() + "'");

            if (dt_work.Rows.Count <= 0)
            {
                //MessageBox.Show("製品構成に登録がありません。新規に登録してください。");
                dgv_seihin_kousei.DataSource = dt_work;
                dgv_seihin_kousei.Columns[0].HeaderText = "部品レベル";
                dgv_seihin_kousei.Columns[1].HeaderText = "部品コード";
                dgv_seihin_kousei.Columns[2].HeaderText = "部品名";
                dgv_seihin_kousei.Columns[3].HeaderText = "使用数";
                dgv_seihin_kousei.Columns[4].HeaderText = "互換部品コード";
                dgv_seihin_kousei.Columns[5].HeaderText = "互換部品名";
                dgv_seihin_kousei.Columns[6].HeaderText = "備考";
                //tb_seihin_kousei_no.Text = "01";
                //tb_seihin_kousei_name.Text = "初回登録";
            }
            else
            {
                dgv_seihin_kousei.DataSource = dt_work;

                dgv_seihin_kousei.Columns[0].HeaderText = "部品レベル";
                dgv_seihin_kousei.Columns[1].HeaderText = "部品コード";
                dgv_seihin_kousei.Columns[2].HeaderText = "部品名";
                dgv_seihin_kousei.Columns[3].HeaderText = "使用数";
                //dgv_seihin_kousei.Columns[4].HeaderText = "親部品コード";
                //dgv_seihin_kousei.Columns[5].HeaderText = "親部品名";
                dgv_seihin_kousei.Columns[4].HeaderText = "互換部品コード";
                dgv_seihin_kousei.Columns[5].HeaderText = "互換部品名";
                dgv_seihin_kousei.Columns[6].HeaderText = "備考";



                //tb_seihin_kousei_no.Clear();
                //tb_seihin_kousei_name.Clear();

                //out_seihin_kousei_name = dt_work.Rows[0]["seihin_kousei_name"].ToString();
            }
        }

        private void btn_touroku_Click(object sender, EventArgs e)
        {
            tss.GetUser();
            int rc = dgv_seihin_kousei.Rows.Count;
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from TSS_SEIHIN_KOUSEI_M WHERE seihin_cd = '" + tb_seihin_cd.Text.ToString() + "' and seihin_kousei_no = '" + tb_seihin_kousei_no.Text.ToString() + "'");
            
            //if(dt_work.Rows.Count >= 0)
            //{
                
            //}
            //dt_work.Rows.Clear();

            for (int i = 0; i < rc - 1; i++)
            {
     
                //dt_work.Rows.Add();

                dt_work.Rows[i][0] = tb_seihin_cd.Text.ToString();
                dt_work.Rows[i][1] = tb_seihin_kousei_no.Text.ToString();
                dt_work.Rows[i][2] = i+1;//SEQ
                dt_work.Rows[i][3] = dgv_seihin_kousei.Rows[i].Cells[0].Value.ToString();//部品レベル
                dt_work.Rows[i][4] = dgv_seihin_kousei.Rows[i].Cells[1].Value.ToString();//部品コード


                if (dgv_seihin_kousei.Rows[i].Cells[0].Value.ToString() == "1")
                {
                    dt_work.Rows[i][5] = 999;//親行番号
                    dt_work.Rows[i][6] = "";//親部品コード
                }
                
                if (int.Parse(dgv_seihin_kousei.Rows[i].Cells[0].Value.ToString()) > 1)
                {
                    for (int j = 1; j < rc - 1; j++)
                    {
                        int a = int.Parse(dgv_seihin_kousei.Rows[i].Cells[0].Value.ToString()) - int.Parse(dgv_seihin_kousei.Rows[i - j].Cells[0].Value.ToString());

                        if (a == 0)
                        {
                            dt_work.Rows[i][5] = dt_work.Rows[i-1][5];//親行番号
                            dt_work.Rows[i][6] = dt_work.Rows[i-1][6];//親部品コード
                            break;
                        }
                        
                        if (a == 1)
                        {
                            dt_work.Rows[i][5] = dt_work.Rows[i - 1][2];//親行番号
                            dt_work.Rows[i][6] = dt_work.Rows[i - 1][4];//親部品コード
                            break;
                        }
                     }
                 
                }
                
                dt_work.Rows[i][7] = dgv_seihin_kousei.Rows[i].Cells[3].Value.ToString();//使用数
                dt_work.Rows[i][8] = dgv_seihin_kousei.Rows[i].Cells[4].Value.ToString();//使用数
                dt_work.Rows[i][9] = dgv_seihin_kousei.Rows[i].Cells[6].Value.ToString();//備考

                if (dt_work.Rows[i][10].ToString() == "")
                {
                    dt_work.Rows[i][10] = tss.user_cd;//クリエイトユーザーコード
                }

                if (dt_work.Rows[i][11].ToString() == "")
                {
                    dt_work.Rows[i][11] = DateTime.Today;//クリエイトユーザーコード
                }

            }

            
            //tss.OracleDelete("delete * from TSS_SEIHIN_KOUSEI_M WHERE seihin_cd = '" + tb_seihin_cd.Text.ToString() + "' and seihin_kousei_no = '" + tb_seihin_kousei_no.Text.ToString() + "'");




        }

    }
}
