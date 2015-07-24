using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;     //app.config用（参照設定にも追加）
using System.Windows.Forms;     //messagebox用（参照設定にも追加）
using Oracle.DataAccess.Client; //（参照設定にも追加）
using System.Data;              //OracleのUPDATE文の実行の際にIsolationLevelを指定するのに必要らしい
using System.IO;                //StreamWriter


namespace TSS_SYSTEM
{
    #region TssSystemLibrary
    /// <summary>
    /// tssシステム用のデータアクセス関連のライブラリ詰め合わせ
    /// tssシステム以外の事は考えていないので他で使用する場合は注意！
    /// </summary>
    class TssSystemLibrary
    {
        #region TssSystemLibrary クラス
        //フィールドの定義
        string fld_DataSource;
        string fld_UserID;
        string fld_Password;
        string fld_ConnectionString;

        string fld_system_cd;
        string fld_system_name;
        string fld_system_version;
        int fld_chat_update_interval;
        string fld_share_directory;

        string fld_user_cd;
        string fld_user_name;
        string fld_user_name2;
        string fld_busyo_cd;
        string fld_kengen1;
        string fld_kengen2;
        string fld_kengen3;
        string fld_kengen4;
        string fld_kengen5;
        string fld_kengen6;


        public TssSystemLibrary()
        {
            //コンストラクタ
            fld_DataSource = null;
            fld_UserID = null;
            fld_Password = null;
            fld_ConnectionString = null;

            fld_system_cd = null;
            fld_system_name = null;
            fld_system_version = null;
            fld_chat_update_interval = 0;
            fld_share_directory = null;

            fld_user_cd = null;
            fld_user_name = null;
            fld_user_name2 = null;
            fld_busyo_cd = null;
            fld_kengen1 = null;
            fld_kengen2 = null;
            fld_kengen3 = null;
            fld_kengen4 = null;
            fld_kengen5 = null;
            fld_kengen6 = null;

        }
        public string DataSource { get { return fld_DataSource; } }
        public string UserID { get { return fld_UserID; } }
        public string Password { get { return fld_Password; } }
        public string ConnectionString { get { return fld_ConnectionString; } }

        public string system_cd { get { return fld_system_cd; } }
        public string system_name { get { return fld_system_name; } }
        public string system_version { get { return fld_system_version; } }
        public int chat_update_interval { get { return fld_chat_update_interval; } }
        public string share_directory { get { return fld_share_directory; } }

        public string user_cd { get { return fld_user_cd; } }
        public string user_name { get { return fld_user_name; } }
        public string user_name2 { get { return fld_user_name2; } }
        public string busyo_cd { get { return fld_busyo_cd; } }
        public string kengen1 { get { return fld_kengen1; } }
        public string kengen2 { get { return fld_kengen2; } }
        public string kengen3 { get { return fld_kengen3; } }
        public string kengen4 { get { return fld_kengen4; } }
        public string kengen5 { get { return fld_kengen5; } }
        public string kengen6 { get { return fld_kengen6; } }
        #endregion

        #region GetConnectionString メソッド
        /// <summary>
        /// App.Configから情報を取得し、暗号・マスク処理を行い、DB接続に必要なConnectionStringを生成し文字列を返す
        /// 戻り値 エラー:null 正常:ConnectionString文字列を作成して返す
        /// </summary>
        public string GetConnectionString()
        {
            /// <summary>
            /// 2014.08.11現在
            /// ID、Password、接続文字列は以下のように暗号化している
            /// ***Strに実際に必要な文字列＋ダミーの文字列を入れる（ダミーの文字列は何文字でも良い）
            /// ***Mskに実際に必要な文字列と同じ長さの適当な文字を入れる
            /// ***Mskの文字数分を***Strの先頭から切り出し、実際の文字列とする
            /// </summary>

            //app.configから必要な情報を取得
            string DataSourceStr = ConfigurationManager.AppSettings["DataSourceStr"];   //データソース名文字列の取得
            string DataSourceMsk = ConfigurationManager.AppSettings["DataSourceMsk"];   //データソース名用のマスク（隠蔽文字列）の取得
            string UserIDStr = ConfigurationManager.AppSettings["UserIDStr"];   //ユーザー名文字列を取得
            string UserIDMsk = ConfigurationManager.AppSettings["UserIDMsk"];   //ユーザー名用のマスク（隠蔽文字列）の取得
            string PasswordStr = ConfigurationManager.AppSettings["PasswordStr"];   //パスワード文字列の取得
            string PasswordMsk = ConfigurationManager.AppSettings["PasswordMsk"];   //パスワード文字列用のマスク（隠蔽文字列）の取得

            if (DataSourceStr.Length < DataSourceMsk.Length)
            {
                MessageBox.Show("設定ファイル:DataSourceを処理できません。");
                return null;
            }
            else
            {
                fld_DataSource = DataSourceStr.Substring(0, DataSourceMsk.Length);
            }
            if (UserIDStr.Length < UserIDMsk.Length)
            {
                MessageBox.Show("設定ファイル:UserIDを処理できません。");
                return null;
            }
            else
            {
                fld_UserID = UserIDStr.Substring(0, UserIDMsk.Length);
            }
            if (PasswordStr.Length < PasswordMsk.Length)
            {
                MessageBox.Show("設定ファイル:Passwordを処理できません。");
                return null;
            }
            else
            {
                fld_Password = PasswordStr.Substring(0, PasswordMsk.Length);
            }
            //戻り値
            fld_ConnectionString = "Data Source=" + DataSource + ";User Id=" + UserID + ";Password=" + Password;
            return ConnectionString;
        }
        #endregion

        #region GetSystemSetting メソッド
        /// <summary>
        /// テーブル TSS_SYSTEM のシステム情報を読み込み、フィールドに格納し、プロパティとして参照可能にする
        /// 引数：無し　戻り値：無し
        /// </summary>
        public void GetSystemSetting()
        {
            DataTable dt = new DataTable();
            dt = OracleSelect("SELECT * FROM TSS_SYSTEM_M where system_cd = '0101'");

            fld_system_cd = dt.Rows[0]["system_cd"].ToString();
            fld_system_name = dt.Rows[0]["system_name"].ToString();
            fld_system_version = dt.Rows[0]["system_version"].ToString();
            //fld_chat_update_interval = (int)(dt.Rows[0]["chat_update_interval"]);
            //fld_share_directory = dt.Rows[0]["share_directory"].ToString();
        }
        #endregion


        #region GetUser メソッド
        /// <summary>
        /// テーブル TSS_USER_M を読み込み、フィールドに格納し、プロパティとして参照可能にする
        /// 引数：無し　戻り値：無し
        /// </summary>
        public void GetUser()
        {
            string TempPath = ConfigurationManager.AppSettings["TempPath"];   //テンポラリフォルダのパスの取得
            string usercd;
            using (StreamReader sr = new StreamReader(TempPath + "user.txt"))
            {
                usercd = sr.ReadToEnd();
            }
            if (usercd != "notlogin") //ユーザー名にnotloginという文字列が入っていたら終了する
            {
                DataTable dt = new DataTable();
                dt = OracleSelect("SELECT * FROM TSS_USER_M where user_cd = '" + usercd + "'");

                fld_user_cd = dt.Rows[0]["user_cd"].ToString();
                fld_user_name = dt.Rows[0]["user_name"].ToString();
                fld_user_name2 = dt.Rows[0]["user_name2"].ToString();
                fld_busyo_cd = dt.Rows[0]["busyo_cd"].ToString();
                fld_kengen1 = dt.Rows[0]["kengen1"].ToString();
                fld_kengen2 = dt.Rows[0]["kengen2"].ToString();
                fld_kengen3 = dt.Rows[0]["kengen3"].ToString();
                fld_kengen4 = dt.Rows[0]["kengen4"].ToString();
                fld_kengen5 = dt.Rows[0]["kengen5"].ToString();
                fld_kengen6 = dt.Rows[0]["kengen6"].ToString();
            }
        }
        #endregion

        #region OracleUpdate メソッド
        /// <summary>
        /// OracleへUPDATE文を実行します。
        /// 戻り値 boolean型 正常=true 異常=false
        /// </summary>
        /// <param name="sql">実行するSQL文です</param>
        public Boolean OracleUpdate(string sql)
        {
            Boolean rtncode = false;    //戻り値用（初期値false）
            string connStr = GetConnectionString();
            using (OracleCommand cmd = new OracleCommand())
            {
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = connStr;
                //トランザクション開始
                conn.Open();
                OracleTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();
                    //トランザクションをコミットします。
                    transaction.Commit();
                    rtncode = true;
                }
                catch (Exception)
                {
                    //トランザクションをロールバック
                    transaction.Rollback();
                    rtncode = false;
                }
                finally
                {
                    conn.Close();
                }
            }
            return rtncode;
        }
        #endregion

        #region OracleDelete メソッド
        /// <summary>
        /// OracleへDELETE文を実行します。
        /// 戻り値 boolean型 正常=true 異常=false
        /// </summary>
        /// <param name="sql">実行するSQL文です</param>
        public Boolean OracleDelete(string sql)
        {
            Boolean rtncode = false;    //戻り値用（初期値false）
            string connStr = GetConnectionString();
            using (OracleCommand cmd = new OracleCommand())
            {
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = connStr;
                //トランザクション開始
                conn.Open();
                OracleTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();
                    //トランザクションをコミットします。
                    transaction.Commit();
                    rtncode = true;
                }
                catch (Exception)
                {
                    //トランザクションをロールバック
                    transaction.Rollback();
                    rtncode = false;
                }
                finally
                {
                    conn.Close();
                }
            }
            return rtncode;
        }
        #endregion

        #region OracleInsert メソッド
        /// <summary>
        /// OracleへINSERT文を実行します。
        /// 戻り値 boolean型 正常=true 異常=false
        /// </summary>
        /// <param name="sql">実行するSQL文です</param>
        public Boolean OracleInsert(string sql)
        {
            Boolean rtncode = false;    //戻り値用（初期値false）
            string connStr = GetConnectionString();
            using (OracleCommand cmd = new OracleCommand())
            {
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = connStr;
                //トランザクション開始
                conn.Open();
                OracleTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();
                    //トランザクションをコミットします。
                    transaction.Commit();
                    rtncode = true;
                }
                catch (Exception)
                {
                    //トランザクションをロールバック
                    transaction.Rollback();
                    rtncode = false;
                }
                finally
                {
                    conn.Close();
                }
            }
            return rtncode;
        }
        #endregion

        #region OracleSelect メソッド
        /// <summary>
        /// OracleへSELECT文を実行します。
        /// 戻り値 DataTable型 エラー時:null
        /// </summary>
        /// <param name="sql">実行するSQL文です</param>
        public DataTable OracleSelect(string sql)
        {
            DataTable dt = new DataTable(); //戻り値用
            string connStr = GetConnectionString();
            using (OracleCommand cmd = new OracleCommand())
            {
                try
                {
                    OracleConnection conn = new OracleConnection();
                    conn.ConnectionString = connStr;
                    //問い合わせ開始
                    //conn.Open();
                    OracleDataAdapter da = new OracleDataAdapter();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    conn.ConnectionString = connStr;
                    da.SelectCommand = cmd;
                    cmd.CommandText = sql;
                    da.Fill(dt);
                    conn.Close();
                }
                catch (Exception)
                {
                    dt = null;
                }
            }
            return dt;
        }
        #endregion

        #region MessageLogWrite メソッド
        /// <summary>
        /// 受け取った文字列をテーブル TSS_MESSAGE_LOG_F に書き込む
        /// 引数：送信先ユーザーコード、発生処理名、メッセージ内容、送信元ユーザーコード　戻り値：bool型
        /// </summary>
        public bool MessageLogWrite(string user_cd_from, string syori_name, string naiyou, string user_cd_to)
        {
            bool bl = new bool();
            bl = OracleInsert("insert into tss_message_log_f(message_datetime,user_cd,message_syori_name,message_log_naiyou,create_user_cd,create_datetime) values (sysdate,'" + user_cd_from + "','" + syori_name + "','" + naiyou + "','" + user_cd_to + "',sysdate)");
            return bl;
        }
        #endregion


        #region ErrorLogWrite メソッド
        /// <summary>
        /// 受け取った文字列をテーブル TSS_ERROR_LOG_F に書き込む
        /// 引数：送信先ユーザーコード、発生処理名、メッセージ内容　戻り値：bool型
        /// </summary>
        public bool ErrorLogWrite(string user_cd, string syori_name, string naiyou)
        {
            bool bl = new bool();
            bl = OracleInsert("insert into tss_error_log_f(error_datetime,user_cd,error_syori_name,error_log_naiyou,create_user_cd,create_datetime) values (sysdate,'" + user_cd + "','" + syori_name + "','" + naiyou + "','" + user_cd + "',sysdate)");
            return bl;
        }
        #endregion




        #region DataTableCSV メソッド
        /// <summary>
        /// DataTableをCSVファイルに出力します。
        /// <param name="dt">出力するDataTable名</param>
        /// <param name="csvPath">CSVファイルのパス</param>
        /// <param name="interstring">各データを囲む文字</param>
        /// <param name="writeHeader">ヘッダを書き込む時はtrue</param>
        /// <returns>正常:true 失敗:false</returns>
        /// </summary>
        public Boolean DataTableCSV(DataTable dt, string csvPath, string interstring, Boolean writeHeader)
        {
            //CSVファイルに書き込むときに使うEncoding
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("Shift_JIS");
            //書き込むファイルを開く
            StreamWriter sr = new StreamWriter(csvPath, false, enc);

            int columncnt = dt.Columns.Count;
            int lastcolumncnt = columncnt - 1;
            try
            {
                //ヘッダを書き込む
                if (writeHeader)
                {
                    for (int i = 0; i < columncnt; i++)
                    {
                        //ヘッダの取得
                        string field = dt.Columns[i].Caption;
                        //データを囲む処理
                        field = interstring + field + interstring;
                        //フィールドを書き込む
                        sr.Write(field);
                        //カンマを書き込む
                        if (lastcolumncnt > i)
                        {
                            sr.Write(',');
                        }
                    }
                    //改行する
                    sr.Write("\r\n");
                }
                //レコードを書き込む
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < columncnt; i++)
                    {
                        //フィールドの取得
                        string field = row[i].ToString();
                        //データを囲む処理
                        field = interstring + field + interstring;
                        //フィールドを書き込む
                        sr.Write(field);
                        //カンマを書き込む
                        if (lastcolumncnt > i)
                        {
                            sr.Write(',');
                        }
                    }
                    //改行する
                    sr.Write("\r\n");
                }
            }
            catch (System.Exception)
            {
                sr.Close();
                return false;
            }
            //閉じる
            sr.Close();
            return true;
        }
        #endregion




        #region　StringByte メソッド
        /// <summary>
        /// 半角 1 バイト、全角 2 バイトとして、指定された文字列のバイト数を返します。</summary>
        /// <param name="str">
        /// バイト数取得の対象となる文字列。</param>
        /// <returns>
        /// 半角 1 バイト、全角 2 バイトでカウントされたバイト数。</returns>
        public int StringByte(string str)
        {
            return System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(str);
        }
        #endregion



        #region　HardCopy メソッド
        /// <summary>
        /// 呼ばれた時点でのアクティブなウィンドゥのハードコピーをクリップボードに送ります。
        /// 正確にはAlt+PrtScが押された事をOSに送信します。
        /// </summary>
        /// <returns>
        /// 戻り値はありません。
        /// </returns>
        public void HardCopy()
        {
            //Altキー＋Print Screenキーの送信
            SendKeys.SendWait("%{PRTSC}");
        }
        #endregion


        //区分コード選択画面の呼び出し
        public string kubun_cd_select(string in_kubun_cd)
        {
            //マウスのX座標を取得する
            int x = System.Windows.Forms.Cursor.Position.X;
            //マウスのY座標を取得する
            int y = System.Windows.Forms.Cursor.Position.Y;

            string out_kubun_cd = "";   //戻り値用
            frm_kubun_select frm_ks = new frm_kubun_select();

            //フォームをマウスの位置に表示する
            frm_ks.Left = x;
            frm_ks.Top = y;
            frm_ks.StartPosition = FormStartPosition.Manual;

            //子画面のプロパティに値をセットする
            frm_ks.str_kubun_meisyou_cd = in_kubun_cd;
            frm_ks.ShowDialog();
            //子画面から値を取得する
            out_kubun_cd = frm_ks.str_kubun_cd;
            frm_ks.Dispose();
            return out_kubun_cd;
        }

        //区分コードから区分名を取得
        public string kubun_name_select(string in_kubun_meisyou_cd, string in_kubun_cd)
        {
            string out_kubun_name = "";   //戻り値用
            //区分名を取得する
            DataTable dt_work = OracleSelect("select kubun_name from tss_kubun_m where kubun_meisyou_cd = '" + in_kubun_meisyou_cd + "' and kubun_cd = '" + in_kubun_cd + "'");
            if (dt_work.Rows.Count <= 0)
            {
                out_kubun_name = "";
            }
            else
            {
                out_kubun_name = dt_work.Rows[0]["kubun_name"].ToString();
            }
            return out_kubun_name;
        }


        //区分コード選択画面（DataTable版）の呼び出し
        public string kubun_cd_select_dt(string in_kubun_name,DataTable in_dt_kubun)
        {
            //マウスのX座標を取得する
            int x = System.Windows.Forms.Cursor.Position.X;
            //マウスのY座標を取得する
            int y = System.Windows.Forms.Cursor.Position.Y;

            string out_kubun_cd = "";   //戻り値用
            frm_kubun_select_dt frm_ks_dt = new frm_kubun_select_dt();

            //フォームをマウスの位置に表示する
            frm_ks_dt.Left = x;
            frm_ks_dt.Top = y;
            frm_ks_dt.StartPosition = FormStartPosition.Manual;

            //子画面のプロパティに値をセットする
            frm_ks_dt.ppt_str_kubun_name = in_kubun_name;
            frm_ks_dt.ppt_dt_kubun = in_dt_kubun;
            frm_ks_dt.ShowDialog();
            //子画面から値を取得する
            out_kubun_cd = frm_ks_dt.ppt_str_kubun_cd;
            frm_ks_dt.Dispose();
            return out_kubun_cd;
        }







    }
    #endregion
}
