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
    public partial class frm_siire : Form
    {
        TssSystemLibrary tss = new TssSystemLibrary();
        DataTable dt_m = new DataTable();
        string w_str = "05";
        
        public frm_siire()
        {
            InitializeComponent();
        }


        //取引先コードから取引先名を持ってくるメソッド
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

        //取引先コードから仕入締日を計算するメソッド
        private DateTime get_siire_simebi(DateTime in_siire_date)
        {
            DateTime out_siire_simebi = new DateTime();  //戻り値用
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_torihikisaki_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text + "'");
            
            string str_day = dt_work.Rows[0][13].ToString(); //締日の日付


            if (str_day == "99")
            {
                DateTime dt1 = dtp_siire_date.Value;
                DateTime dt2 = new DateTime(dt1.Year, dt1.Month, DateTime.DaysInMonth(dt1.Year, dt1.Month));
                out_siire_simebi = dt2;
            }

            else
            {
                DateTime dt1 = dtp_siire_date.Value;
                DateTime dt2 = new DateTime(dt1.Year, dt1.Month, DateTime.DaysInMonth(dt1.Year, dt1.Month));

                out_siire_simebi = dt2;
                //int d1 = int.Parse(dt1.Day.ToString());
                //int d2 = int.Parse(str_day);
                
                //if( d1 >= d2)
                //{
                //    DateTime dt2 = new DateTime(dt1.Year, dt1.Month, DateTime.DaysInMonth(dt1.Year, dt1.Month));
                //}
                //if( d1 < d2)

                //DateTime dt2 = new DateTime(dt1.Day);

                
                //out_torihikisaki_name = dt_work.Rows[0]["torihikisaki_name"].ToString();
            }
            return out_siire_simebi;
        }


        //SEQを持ってくるメソッド
        private void SEQ()
        {
            DataTable dt_work = new DataTable();
            double w_seq;
            w_seq = tss.GetSeq(w_str);
            if (w_seq == 0)
            {
                MessageBox.Show("連番マスタに異常があります。処理を中止します。");
                this.Close();
            }
            tb_siire_no.Text = (w_seq).ToString("0000000000");
        }

        private void frm_siire_Load(object sender, EventArgs e)
        {
            SEQ();
        }

        private void tb_torihikisaki_cd_Validating(object sender, CancelEventArgs e)
        {
            if (tb_torihikisaki_cd.Text == "")
            {
                tb_torihikisaki_name.Text = "";
                return;
            }

            bool bl = true; //戻り値
            DataTable dt_work = new DataTable();
            dt_work = tss.OracleSelect("select * from tss_torihikisaki_m where torihikisaki_cd  = '" + tb_torihikisaki_cd.Text + "'");
            if (dt_work.Rows.Count <= 0)
            {
                //無し
                MessageBox.Show("入力された取引先コードが存在しません。取引先マスタに登録してください。");
                tb_torihikisaki_cd.Focus();

            }
            else
            {
                //既存データ有
                tb_torihikisaki_name.Text = get_torihikisaki_name(tb_torihikisaki_cd.Text);
                dgv_siire.Focus();
            }
            
        }

        private void dgv_siire_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;


            //部品コードが入力されたならば、部品名と仕入単価を部品マスターから取得して表示
            if (dgv.Columns[e.ColumnIndex].Index == 0 && dgv.CurrentCell.Value == null)
            {
                return;
            }

            if (dgv.Columns[e.ColumnIndex].Index == 0 && dgv.CurrentCell.Value.ToString() != null)
            {
                int i = e.RowIndex;

                DataTable dtTmp = (DataTable)dgv_siire.DataSource;

                //部品コードをキーに、部品名、仕入単価を引っ張ってくる

                DataTable dt_work = new DataTable();
                DataTable dt_work2 = new DataTable();
                int j = dt_work.Rows.Count;
                int j2 = dt_work2.Rows.Count;
                
                dt_work = tss.OracleSelect("select * from tss_buhin_m where buhin_cd = '" + dgv.CurrentCell.Value.ToString() + "'");
                dt_work2 = tss.OracleSelect("select * from tss_torihikisaki_m where torihikisaki_cd = '" + tb_torihikisaki_cd.Text.ToString() + "'");

                if (dt_work.Rows.Count <= 0)
                {
                    MessageBox.Show("この部品コードは登録されていません。部品登録してください。");
                    dgv.Rows[i].Cells[1].Value = "";
                    dgv_siire.Focus();
                    dgv_siire.CurrentCell = dgv_siire[0, i];
                }
                else
                {
                    dgv.Rows[i].Cells[1].Value = dt_work.Rows[j][1].ToString();
                    dgv.Rows[i].Cells[3].Value = dt_work.Rows[j][8].ToString();


                    string seikyu_simebi = dt_work2.Rows[j2][13].ToString();
                    string kaisyu_tuki = dt_work2.Rows[j2][14].ToString();
                    string kaisyu_hi = dt_work2.Rows[j2][15].ToString();

                    string siharai_simebi = dt_work2.Rows[j2][16].ToString();
                    string siharai_tuki = dt_work2.Rows[j2][17].ToString();
                    string siharai_hi = dt_work2.Rows[j2][18].ToString();

                    string hasu_kbn = dt_work2.Rows[j2][22].ToString();
                    string hasu_syori_tani = dt_work2.Rows[j2][23].ToString();


                    DateTime dt1 = dtp_siire_date.Value;
                    DateTime dt2 = new DateTime(dt1.Year, dt1.Month,DateTime.DaysInMonth(dt1.Year, dt1.Month));
                    DateTime dt3 = dt2.AddMonths(int.Parse(kaisyu_tuki));

                    MessageBox.Show("仕入締日は" + dt2.ToShortDateString() + "です。");
                    MessageBox.Show("支払日は" + dt3.ToShortDateString() + "です。");


                    //DateTime dt1 = dtp_siire_date.Value;

                    //DateTime dt2 = dt1.AddMonths(int.Parse(kaisyu_tuki));

                    //DateTime dt3 = new DateTime(dt2.Year, dt2.Month, 1);

                    MessageBox.Show(seikyu_simebi);
                    MessageBox.Show(kaisyu_tuki);





                        
                }

                return;
            }
            
            if (dgv.Columns[e.ColumnIndex].Index == 0 && dgv.CurrentCell.Value.ToString() == null && dgv.CurrentCell.Value.ToString() == "")
            {
                return;
            }
            
            ////仕入数量が入力されたならば、仕入単価と数量を掛け算して仕入金額に表示（取引先マスタの端数処理も組み込む）
            //if (dgv.Columns[e.ColumnIndex].Index == 2 && dgv.CurrentCell.Value == null)
            //{
            //    return;
            //}

            //if (dgv.Columns[e.ColumnIndex].Index == 2 && dgv.CurrentCell.Value.ToString() != null)
            //{
            //    int i = e.RowIndex;

            //    DataTable dtTmp = (DataTable)dgv_siire.DataSource;

            //    //部品コードをキーに、部品名、仕入単価を引っ張ってくる

            //    DataTable dt_work = new DataTable();
            //    //DataTable dt_work2 = new DataTable();
            //    int j = dt_work.Rows.Count;
            //    //int j2 = dt_work2.Rows.Count;
                
            //    dt_work = tss.OracleSelect("select * from tss_buhin_m where buhin_cd = '" + dgv.CurrentCell.Value.ToString() + "'");
            //    //dt_work2 = tss.OracleSelect("select * from tss_buhin_m where buhin_cd = '" + dgv.CurrentCell.Value.ToString() + "'");

            //    if (dt_work.Rows.Count <= 0)
            //    {
            //        MessageBox.Show("この部品コードは登録されていません。部品登録してください。");
            //        dgv.Rows[i].Cells[1].Value = "";
            //        dgv_siire.Focus();
            //        dgv_siire.CurrentCell = dgv_siire[0, i];
            //    }
            //    else
            //    {
            //        dgv.Rows[i].Cells[1].Value = dt_work.Rows[j][1].ToString();
            //        dgv.Rows[i].Cells[3].Value = dt_work.Rows[j][8].ToString();
            //    }

            //    return;
            //}

            if (dgv.Columns[e.ColumnIndex].Index == 2 && dgv.CurrentCell.Value.ToString() == null && dgv.CurrentCell.Value.ToString() == "")
            {
                return;

            }


        }

    }
}
