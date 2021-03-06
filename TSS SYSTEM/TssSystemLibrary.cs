﻿using System;
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
        string fld_CsvPath;

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
            fld_CsvPath = null;

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
        public string CsvPath { get { return fld_CsvPath; } }

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
                    GetUser();
                    ErrorLogWrite(user_cd, "OracleUpdate", sql.Replace("'", "###"));
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
                    GetUser();
                    ErrorLogWrite(user_cd, "OracleDelete", sql.Replace("'", "#"));
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
                    GetUser();
                    ErrorLogWrite(user_cd, "OracleInsert", sql.Replace("'", "#"));

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
                    GetUser();
                    ErrorLogWrite(user_cd, "OracleSelect",sql.Replace("'","#"));
                    MessageBox.Show("データベースの処理中にエラーが発生しました。");
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
        public bool MessageLogWrite(string user_cd_from, string user_cd_to,string syori_name, string naiyou)
        {
            bool bl = new bool();
            bl = OracleInsert("insert into tss_message_log_f(message_datetime,user_cd_from,user_cd_to,message_syori_name,message_log_naiyou,create_user_cd,create_datetime) values (to_char(sysdate,'yyyy/mm/dd hh24:mi:ss'),'" + user_cd_from + "','" + user_cd_to + "','" + syori_name + "','" + naiyou + "','" + user_cd_to + "',sysdate)");
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
            bl = OracleInsert("insert into tss_error_log_f(error_datetime,user_cd,error_syori_name,error_naiyou,create_user_cd,create_datetime) values (to_char(sysdate,'yyyy/mm/dd hh24:mi:ss'),'" + user_cd + "','" + syori_name + "','" + naiyou + "','" + user_cd + "',sysdate)");
            return bl;
        }
        #endregion

        #region DataTableCSV メソッド
        /// <summary>
        /// DataTableをCSVファイルに出力します。
        /// <param name="dt">出力するDataTable名</param>
        /// <param name="SaveFileDialog">ファイルダイアログを使用する時はtrue</param>
        /// <param name="csvPath">CSVファイルのフルパス（ファイル名まで含める）※SaveFileDialogがtrueの場合はパスを除いたファイル名</param>
        /// <param name="interstring">各データを囲む文字</param>
        /// <param name="writeHeader">ヘッダを書き込む時はtrue</param>
        /// <returns>正常:true 失敗:false</returns>
        /// </summary>
        public Boolean DataTableCSV(DataTable in_dt,bool in_SaveFileDialog, string in_csvPath, string in_interstring, Boolean in_writeHeader)
        {
            //保存するファイルパスとファイル名を決める
            string w_str_filename;
            if(in_SaveFileDialog)
            {
                //SaveFileDialogクラスのインスタンスを作成
                SaveFileDialog sfd = new SaveFileDialog();
                //はじめのファイル名を指定する
                sfd.FileName = in_csvPath;
                //はじめに表示されるフォルダを指定する
                sfd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                //[ファイルの種類]に表示される選択肢を指定する
                sfd.Filter = "CSVファイル(*.csv)|*.csv|すべてのファイル(*.*)|*.*";
                //タイトルを設定する
                sfd.Title = "保存先のファイルを選択してください";
                //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
                sfd.RestoreDirectory = true;
                //既に存在するファイル名を指定したとき警告する（デフォルトでTrueなので指定する必要はない）
                sfd.OverwritePrompt = true;
                //存在しないパスが指定されたとき警告を表示する（デフォルトでTrueなので指定する必要はない）
                sfd.CheckPathExists = true;

                //ダイアログを表示する
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    //OKボタンがクリックされたとき選択されたファイル名を表示する
                    w_str_filename = sfd.FileName;
                }
                else
                {
                    //OKボタンでない場合はキャンセルとみなし終了する
                    return false;
                }
            }
            else
            {
                w_str_filename = in_csvPath;
            }

            //CSVファイルに書き込むときに使うEncoding
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("Shift_JIS");
            //書き込むファイルを開く
            StreamWriter sr = new StreamWriter(w_str_filename, false, enc);

            int columncnt = in_dt.Columns.Count;
            int lastcolumncnt = columncnt - 1;
            try
            {
                //ヘッダを書き込む
                if (in_writeHeader)
                {
                    for (int i = 0; i < columncnt; i++)
                    {
                        //ヘッダの取得
                        string field = in_dt.Columns[i].Caption;
                        //データを囲む処理
                        field = in_interstring + field + in_interstring;
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
                foreach (DataRow row in in_dt.Rows)
                {
                    for (int i = 0; i < columncnt; i++)
                    {
                        //フィールドの取得
                        string field = row[i].ToString();
                        //データを囲む処理
                        field = in_interstring + field + in_interstring;
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

        #region StringByte メソッド
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

        #region HardCopy メソッド
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

        #region 区分コード選択画面
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
        #endregion

        #region 区分コード選択画面（初期選択機能付き）
        //区分コード選択画面の呼び出し 初期値あり版
        public string kubun_cd_select(string in_kubun_cd,string in_initial_cd)
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
            frm_ks.str_initial_cd = in_initial_cd;
            frm_ks.ShowDialog();
            //子画面から値を取得する
            out_kubun_cd = frm_ks.str_kubun_cd;
            frm_ks.Dispose();
            return out_kubun_cd;
        }
        #endregion

        #region 区分名取得
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
        #endregion

        #region 区分選択画面（DataTable版）
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
        #endregion

        #region 区分選択画面（DataTable版＋初期選択機能付き）
        //区分コード選択画面（DataTable版）の呼び出し　初期値あり版
        public string kubun_cd_select_dt(string in_kubun_name, DataTable in_dt_kubun,string in_initial_cd)
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
            frm_ks_dt.ppt_str_initial_cd = in_initial_cd;
            frm_ks_dt.ShowDialog();
            //子画面から値を取得する
            out_kubun_cd = frm_ks_dt.ppt_str_kubun_cd;
            frm_ks_dt.Dispose();
            return out_kubun_cd;
        }
        #endregion

        #region 製品構成番号選択画面
        //製品構成番号選択画面（DataTable版）の呼び出し
        public string seihin_kousei_select_dt(string in_seihin_cd, DataTable in_dt_seihin_kousei_name)
        {
            //マウスのX座標を取得する
            int x = System.Windows.Forms.Cursor.Position.X;
            //マウスのY座標を取得する
            int y = System.Windows.Forms.Cursor.Position.Y;

            string out_seihin_cd = "";   //戻り値用
            string out_seihin_kousei_no = "";   //戻り値用
            frm_seihin_kousei_select frm_sks = new frm_seihin_kousei_select();

            //フォームをマウスの位置に表示する
            frm_sks.Left = x;
            frm_sks.Top = y;
            frm_sks.StartPosition = FormStartPosition.Manual;

            //子画面のプロパティに値をセットする
            frm_sks.ppt_str_seihin_cd = in_seihin_cd;
            frm_sks.ppt_dt_seihin_kousei_name = in_dt_seihin_kousei_name;
            frm_sks.ShowDialog();
            //子画面から値を取得する
            out_seihin_cd = frm_sks.ppt_str_seihin_cd + "                ";
            out_seihin_cd = out_seihin_cd.Substring(0, 16);
            out_seihin_kousei_no = frm_sks.ppt_str_seihin_kousei_no + "  ";
            out_seihin_kousei_no = out_seihin_kousei_no.Substring(0, 2);
            frm_sks.Dispose();
            return out_seihin_cd + out_seihin_kousei_no;
        }
        #endregion

        #region 製品構成番号選択画面（製品コードの受け渡しなし）
        //製品構成番号選択画面（製品コードの受け渡しなし）の呼び出し
        public string seihin_kousei_select_dt2(string in_seihin_cd, DataTable in_dt_seihin_kousei_name)
        {
            //マウスのX座標を取得する
            int x = System.Windows.Forms.Cursor.Position.X;
            //マウスのY座標を取得する
            int y = System.Windows.Forms.Cursor.Position.Y;

            string out_seihin_cd = "";   //戻り値用
            string out_seihin_kousei_no = "";   //戻り値用
            frm_seihin_kousei_select frm_sks = new frm_seihin_kousei_select();

            //フォームをマウスの位置に表示する
            frm_sks.Left = x;
            frm_sks.Top = y;
            frm_sks.StartPosition = FormStartPosition.Manual;

            //子画面のプロパティに値をセットする
            frm_sks.ppt_str_seihin_cd = "";
            //frm_sks.ppt_dt_seihin_kousei_name = "";
            frm_sks.ShowDialog();
            //子画面から値を取得する
            out_seihin_cd = frm_sks.ppt_str_seihin_cd + "                ";
            out_seihin_cd = out_seihin_cd.Substring(0, 16);
            out_seihin_kousei_no = frm_sks.ppt_str_seihin_kousei_no + "  ";
            out_seihin_kousei_no = out_seihin_kousei_no.Substring(0, 2);
            frm_sks.Dispose();
            return out_seihin_cd + out_seihin_kousei_no;
        }
        #endregion

        #region 部品検索画面
        //部品検索画面の呼び出し
        public string search_buhin(string in_mode, string in_cd)
        {
            //マウスのX座標を取得する
            //int x = System.Windows.Forms.Cursor.Position.X;
            //マウスのY座標を取得する
            //int y = System.Windows.Forms.Cursor.Position.Y;

            string out_cd = "";   //戻り値用
            frm_search_buhin frm_sb = new frm_search_buhin();

            //フォームをマウスの位置に表示する
            //frm_sb.Left = x;
            //frm_sb.Top = y;
            //frm_sb.StartPosition = FormStartPosition.Manual;

            //子画面のプロパティに値をセットする
            frm_sb.str_mode = in_mode;
            frm_sb.str_name = in_cd;
            frm_sb.ShowDialog();
            //子画面から値を取得する
            out_cd = frm_sb.str_cd;
            frm_sb.Dispose();
            return out_cd;
        }
        #endregion

        #region 取引先検索画面
        //取引先検索画面の呼び出し
        public string search_torihikisaki(string in_mode, string in_cd)
        {
            //マウスのX座標を取得する
            //int x = System.Windows.Forms.Cursor.Position.X;
            //マウスのY座標を取得する
            //int y = System.Windows.Forms.Cursor.Position.Y;

            string out_cd = "";   //戻り値用
            frm_search_torihikisaki frm_sb = new frm_search_torihikisaki();

            //フォームをマウスの位置に表示する
            //frm_sb.Left = x;
            //frm_sb.Top = y;
            //frm_sb.StartPosition = FormStartPosition.Manual;

            //子画面のプロパティに値をセットする
            frm_sb.str_mode = in_mode;
            frm_sb.str_name = in_cd;
            frm_sb.ShowDialog();
            //子画面から値を取得する
            out_cd = frm_sb.str_cd;
            frm_sb.Dispose();
            return out_cd;
        }
        #endregion

        #region 受注検索画面
        //受注検索画面の呼び出し
        public string search_juchu(string in_mode, string in_torihikisaki_cd,string in_juchu_cd1,string in_juchu_cd2,string in_seihin_cd)
        {
            //マウスのX座標を取得する
            //int x = System.Windows.Forms.Cursor.Position.X;
            //マウスのY座標を取得する
            //int y = System.Windows.Forms.Cursor.Position.Y;

            string out_cd = "";   //戻り値用
            frm_search_juchu frm_sb = new frm_search_juchu();

            //フォームをマウスの位置に表示する
            //frm_sb.Left = x;
            //frm_sb.Top = y;
            //frm_sb.StartPosition = FormStartPosition.Manual;

            //子画面のプロパティに値をセットする
            frm_sb.str_mode = in_mode;
            frm_sb.str_name = in_torihikisaki_cd;

            frm_sb.in_torihikisaki_cd = in_torihikisaki_cd;
            frm_sb.in_juchu_cd1 = in_juchu_cd1;
            frm_sb.in_juchu_cd2 = in_juchu_cd2;
            frm_sb.in_seihin_cd = in_seihin_cd;

            frm_sb.ShowDialog();
            //子画面から値を取得する
            //※受注検索画面の戻り値は、受注Noの３つの項目を１つの文字列にして返す
            out_cd = frm_sb.str_torihikisaki_cd + frm_sb.str_juchu_cd1 + frm_sb.str_juchu_cd2 ;
            frm_sb.Dispose();
            return out_cd;
        }
        #endregion

        #region 製品検索画面
        public string search_seihin(string in_mode, string in_cd)
        {
            //マウスのX座標を取得する
            //int x = System.Windows.Forms.Cursor.Position.X;
            //マウスのY座標を取得する
            //int y = System.Windows.Forms.Cursor.Position.Y;

            string out_cd = "";   //戻り値用
            frm_search_seihin frm_sb = new frm_search_seihin();

            //フォームをマウスの位置に表示する
            //frm_sb.Left = x;
            //frm_sb.Top = y;
            //frm_sb.StartPosition = FormStartPosition.Manual;

            //子画面のプロパティに値をセットする
            frm_sb.str_mode = in_mode;
            frm_sb.str_name = in_cd;
            frm_sb.ShowDialog();
            //子画面から値を取得する
            out_cd = frm_sb.str_cd;
            frm_sb.Dispose();
            return out_cd;
        }
        #endregion

        #region 受注コード２選択画面
        public string select_juchu_cd(DataTable in_dt)
        {
            //マウスのX座標を取得する
            //int x = System.Windows.Forms.Cursor.Position.X;
            //マウスのY座標を取得する
            //int y = System.Windows.Forms.Cursor.Position.Y;

            string out_cd = "";   //戻り値用
            frm_select_juchu frm_sb = new frm_select_juchu();

            //フォームをマウスの位置に表示する
            //frm_sb.Left = x;
            //frm_sb.Top = y;
            //frm_sb.StartPosition = FormStartPosition.Manual;

            //子画面のプロパティに値をセットする
            frm_sb.ppt_str_name = "下記リストから受注を選択してください。";
            frm_sb.ppt_dt_m = in_dt;
            frm_sb.ShowDialog();
            //子画面から値を取得する
            out_cd = frm_sb.ppt_str_juchu_cd2;
            frm_sb.Dispose();
            return out_cd;
        }
        #endregion

        #region GetCsvPath
        /// <summary>
        /// TSSシステムのデフォルトのCSV出力先パスを取得する。</summary>
        /// <returns>
        /// string scv出力先のパスを返します。</returns>
        public string GetCsvPath()
        {
            //app.configから必要な情報を取得
            string CsvPath = ConfigurationManager.AppSettings["CsvPath"];   //データソース名文字列の取得

            if (CsvPath.Length == 0)
            {
                MessageBox.Show("設定ファイル:CsvPathを処理できません。");
                return null;
            }
            else
            {
                fld_CsvPath = CsvPath;
            }
            //戻り値
            fld_ConnectionString = "Data Source=" + DataSource + ";User Id=" + UserID + ";Password=" + Password;
            return CsvPath;
        }
        #endregion

        #region GetSeq メソッド
        /// <summary>
        /// 連番マスタから必要な連番を取得し、取得後連番を＋１する。</summary>
        /// <param name="string in_cd">
        /// 取得する連番の連番コード</param>
        /// <returns>
        /// double out_seq 取得した連番。エラー時は0を返します。</returns>
        public double GetSeq(string in_cd)
        {
            GetUser();
            double out_seq = 0;
            DataTable w_dt = new DataTable();
            w_dt = OracleSelect("select * from tss_seq_m where seq_m_cd = '" + in_cd + "'");
            if(w_dt.Rows.Count == 0)
            {
                out_seq = 0;
            }
            else
            {
                //seqは保存されている値が使用される値
                out_seq = Convert.ToDouble(w_dt.Rows[0]["seq"]);
                string sql = "update tss_seq_m set seq = '" + (out_seq+1).ToString() + "',UPDATE_USER_CD = '" + user_cd + "',UPDATE_DATETIME = SYSDATE WHERE seq_m_cd = '" + in_cd + "'";
                //取得後、＋１して書き込む
                if(OracleUpdate(sql))
                {
                    //書込み成功
                }
                else
                {
                    ErrorLogWrite(user_cd, "GetSeq内のOracleUpdate",sql.Replace("'","#"));
                    MessageBox.Show("データベースの処理中にエラーが発生しました。");
                    out_seq = 0;
                }
            }
            return out_seq;
        }
        #endregion

        #region 部品入出庫呼び出し
        //部品入出庫画面の呼び出し
        public string buhin_nyusyukkoidou(string in_mode)
        {
            string out_cd = "";   //戻り値用
            frm_buhin_nyusyukkoidou frm_bnsi = new frm_buhin_nyusyukkoidou();

            //子画面のプロパティに値をセットする
            frm_bnsi.str_mode = in_mode;
            frm_bnsi.ShowDialog();
            frm_bnsi.Dispose();

            //子画面から値を取得する
            out_cd = frm_bnsi.str_cd;
            return out_cd;
        }
        #endregion

        public string out_str_seihin_kousei_no { get; set; }

        #region try_string_to_date メソッド
        /// <summary>
        /// 文字列を受け取りdate型に変換できるか（適切な日付か）を調べ、bool型を返す</summary>
        /// <param name="in_str">
        /// 変換前の日付文字列</param>
        /// <returns>
        /// bool true:変換可能 false:変換不能
        /// 変換可能時にプロパティ out_datetime に変換後の値を格納</returns>
        public DateTime out_datetime;
        public bool try_string_to_date(string in_str)
        {
            bool bl;    //戻り値用
            bl = true;
            string w_str = in_str;
            //7文字以下はNG
            if(StringByte(in_str) < 8)
            {
                bl = false;
                return bl;
            }
            //スラッシュがある場合はそのまま使用し、なければスラッシュを加える
            if(in_str.IndexOf("/") == -1)
            {
                w_str = in_str.Substring(0, 4) + "/" + in_str.Substring(4, 2) + "/" + in_str.Substring(6);
            }
            if (DateTime.TryParse(w_str, out out_datetime))
            {
                bl = true;
            }
            else
            {
                bl = false;
            }
            return bl;
        }
        #endregion

        #region get_torihikisaki_name メソッド
        /// <summary>
        /// 取引先コードを受け取り取引先名を返す</summary>
        /// <param name="in_cd">
        /// 取引先名を取得する取引先コード</param>
        /// <returns>
        /// string 取引先名
        /// エラー等、取得できない場合はnull</returns>
        public string get_torihikisaki_name(string in_cd)
        {
            string out_str = null;  //戻り値用
            DataTable w_dt = new DataTable();
            w_dt = OracleSelect("select * from tss_torihikisaki_m where torihikisaki_cd = '" + in_cd + "'");
            if (w_dt.Rows.Count == 0)
            {
                out_str = null;
            }
            else
            {
                out_str = w_dt.Rows[0]["torihikisaki_name"].ToString();
            }
            return out_str;
        }
        #endregion

        #region get_seihin_name メソッド
        /// <summary>
        /// "製品コードを受け取り製品名を返す</summary>
        /// <param name="in_cd">
        /// 製品名を取得する製品コード</param>
        /// <returns>
        /// string 製品名
        /// エラー等、取得できない場合はnull</returns>
        public string get_seihin_name(string in_cd)
        {
            string out_str = null;  //戻り値用
            DataTable w_dt = new DataTable();
            w_dt = OracleSelect("select * from tss_seihin_m where seihin_cd = '" + in_cd + "'");
            if (w_dt.Rows.Count == 0)
            {
                out_str = null;
            }
            else
            {
                out_str = w_dt.Rows[0]["seihin_name"].ToString();
            }
            return out_str;
        }
        #endregion

        #region get_seihin_tanka メソッド
        /// <summary>
        /// 製品コードを受け取り販売単価を返す</summary>
        /// <param name="in_cd">
        /// 販売単価を取得する製品コード</param>
        /// <returns>
        /// double 販売単価
        /// エラー等、取得できない場合は-1</returns>
        public double get_seihin_tanka(string in_cd)
        {
            double out_dou = -1;  //戻り値用
            DataTable w_dt = new DataTable();
            double w_dou;
            w_dt = OracleSelect("select * from tss_seihin_m where seihin_cd = '" + in_cd + "'");
            if (w_dt.Rows.Count == 0)
            {
                out_dou = -1;
            }
            else
            {
                if (double.TryParse(w_dt.Rows[0]["hanbai_tanka"].ToString(), out w_dou))
                {
                    out_dou = w_dou;
                }
                else
                {
                    out_dou = -1;
                }
            }
            return out_dou;
        }
        #endregion

        #region get_juchu_to_seihin_cd メソッド
        /// <summary>
        /// 受注番号を受け取り製品コードを返す</summary>
        /// <param name="in_torihikisaki_cd">
        /// 製品コードを取得する受注番号の取引先コード</param>
        /// <param name="in_juchu_cd1">
        /// 製品コードを取得する受注番号の受注コード1</param>
        /// <param name="in_juchu_cd2">
        /// 製品コードを取得する受注番号の受注コード2</param>
        /// <returns>
        /// string 製品コード
        /// エラー等、取得できない場合はnull</returns>
        public string get_juchu_to_seihin_cd(string in_torihikisaki_cd,string in_juchu_cd1,string in_juchu_cd2)
        {
            string out_str = null;  //戻り値用
            DataTable w_dt = new DataTable();
            w_dt = OracleSelect("select * from tss_juchu_m where torihikisaki_cd = '" + in_torihikisaki_cd + "' and juchu_cd1 = '" + in_juchu_cd1 + "' and juchu_cd2 = '" + in_juchu_cd2 + "'");
            if (w_dt.Rows.Count == 0)
            {
                out_str = null;
            }
            else
            {
                out_str = w_dt.Rows[0]["seihin_cd"].ToString();
            }
            return out_str;
        }
        #endregion

        #region zaiko_proc メソッド
        /// <summary>
        /// 部品コード、在庫区分、受注番号の3項目、在庫の加減数を受け取り、在庫の加減算を行い書き込む
        /// ロット在庫の消し込みの場合、ロット在庫が足りない場合、不足分はフリー在庫で処理する
        /// ロット在庫の消し込みの場合、受注マスタの売上完了フラグが立っている場合、ロット在庫の残りはフリー在庫に移動する
        /// レコードがない場合は作成する</summary>
        /// <param name="string in_buhin_cd">
        /// 在庫の加減算を行う部品コード</param>
        /// <param name="string in_zaiko_kbn">
        /// 加減算を行う在庫区分</param>
        /// <param name="string in_torihikisaki_cd">
        /// ロット在庫の取引先コード</param>
        /// <param name="string in_juchu_cd1">
        /// ロット在庫の受注コード1</param>
        /// <param name="string in_juchu_cd2">
        /// ロット在庫の受注コード2</param>
        /// <param name="double in_su">
        /// 加減算数</param>
        /// <param name="double in_gyou">
        /// 履歴行</param>
        /// <param name="string in_bikou">
        /// 備考に書き込む伝票番号等</param>
        /// <returns>
        /// bool true:正常終了 false:異常終了
        /// エラー等、取得できない場合はnull</returns>
        public int ppt_gyou;   //履歴に書き込んだ行番号
        public bool zaiko_proc(string in_buhin_cd,string in_zaiko_kbn, string in_torihikisaki_cd, string in_juchu_cd1, string in_juchu_cd2,double in_su,double in_rireki_no,int in_gyou,string in_bikou)
        {
            bool bl = true;  //戻り値用
            string w_sql;

            DataTable w_dt = new DataTable();
            DataTable w_dt2 = new DataTable();  //フリー在庫以外の在庫がマイナスになる場合用
            double w_zaiko_su;
            bool w_rireki_bl;   //履歴書込み用
            string w_rireki_kbn;
            if(in_su >= 0)
            {
                w_rireki_kbn = "02";    //入庫（売上の取消）
            }
            else
            {
                w_rireki_kbn = "01";    //出庫（通常消込）
            }
            ppt_gyou = in_gyou;

            //消し込む在庫レコードをw_dtに入れる
            w_dt = OracleSelect("select * from tss_buhin_zaiko_m where buhin_cd = '" + in_buhin_cd + "' and zaiko_kbn = '" + in_zaiko_kbn + "' and torihikisaki_cd = '" + in_torihikisaki_cd + "' and juchu_cd1 = '" + in_juchu_cd1 + "' and juchu_cd2 = '" + in_juchu_cd2 + "'");
            if (w_dt.Rows.Count == 0)
            {

                //在庫レコードが無い場合
                //フリー在庫でなかった場合は、フリー在庫を使用する
                if(in_zaiko_kbn != "01")
                {
                    w_dt = OracleSelect("select * from tss_buhin_zaiko_m where buhin_cd = '" + in_buhin_cd + "' and zaiko_kbn = '" + "01" + "' and torihikisaki_cd = '" + in_torihikisaki_cd + "' and juchu_cd1 = '9999999999999999' and juchu_cd2 = '9999999999999999'");
                    if(w_dt.Rows.Count == 0)
                    {
                        GetUser();
                        ErrorLogWrite(user_cd, "tss.zeiko_procでフリー在庫レコードが無い", "zaiko_proc(" + in_buhin_cd + "," + in_zaiko_kbn + "," + in_torihikisaki_cd + "," + in_juchu_cd1 + "," + in_juchu_cd2 + "," + in_su + ")");
                        bl = false;
                        return bl;
                    }
                }
                else
                {
                    GetUser();
                    ErrorLogWrite(user_cd, "tss.zeiko_procでフリー在庫レコードが無い", "zaiko_proc(" + in_buhin_cd + "," + in_zaiko_kbn + "," + in_torihikisaki_cd + "," + in_juchu_cd1 + "," + in_juchu_cd2 + "," + in_su + ")");
                    bl = false;
                    return bl;
                }
            }
            double.TryParse(w_dt.Rows[0]["zaiko_su"].ToString(), out w_zaiko_su);
            w_zaiko_su = w_zaiko_su - in_su;

            if (w_dt.Rows[0]["zaiko_kbn"].ToString() != "01" && w_zaiko_su < 0)
            {
                //フリー在庫以外で、在庫数がマイナスになる場合は、マイナス分はフリー在庫で処理する。
                //まず、フリー在庫でない在庫の書き込み（０にする）
                GetUser();
                w_sql = "UPDATE tss_buhin_zaiko_m SET zaiko_su = '0',update_user_cd = '" + user_cd + "',update_datetime = sysdate where buhin_cd = '" + in_buhin_cd + "' and zaiko_kbn = '" + w_dt.Rows[0]["zaiko_kbn"].ToString() + "' and torihikisaki_cd = '" + in_torihikisaki_cd + "' and juchu_cd1 = '" + w_dt.Rows[0]["juchu_cd1"].ToString() + "' and juchu_cd2 = '" + w_dt.Rows[0]["juchu_cd2"].ToString() + "'";
                if(OracleUpdate(w_sql) == false)
                {
                    GetUser();
                    ErrorLogWrite(user_cd, "OracleUpdate", w_sql.Replace("'", "#"));
                    MessageBox.Show("データベースの処理中にエラーが発生しました。zaiko_proc 01");
                    bl = false;
                }
                //在庫履歴へ書き込み
                w_rireki_bl = OracleInsert("insert into tss_buhin_nyusyukko_m (buhin_syori_kbn,buhin_syori_no,seq,buhin_syori_date,buhin_cd,zaiko_kbn,torihikisaki_cd,juchu_cd1,juchu_cd2,suryou,bikou,create_user_cd,create_datetime) values ('" + w_rireki_kbn + "','" + in_rireki_no.ToString("0") + "','" + ppt_gyou.ToString() + "',sysdate,'" + in_buhin_cd + "','" + w_dt.Rows[0]["zaiko_kbn"].ToString() + "','" + in_torihikisaki_cd + "','" + w_dt.Rows[0]["juchu_cd1"].ToString() + "','" + w_dt.Rows[0]["juchu_cd2"].ToString() + "','" + "0.00" + "','売上番号" + in_bikou + "分の消し込み','" + user_cd + "',sysdate)");
                ppt_gyou++;
               
                //次に残りの在庫数をフリー在庫で処理する
                w_dt2 = OracleSelect("select * from tss_buhin_zaiko_m where buhin_cd = '" + in_buhin_cd + "' and zaiko_kbn = '" + "01" + "' and torihikisaki_cd = '" + in_torihikisaki_cd + "' and juchu_cd1 = '9999999999999999' and juchu_cd2 = '9999999999999999'");
                if (w_dt2.Rows.Count == 0)
                {
                    GetUser();
                    ErrorLogWrite(user_cd, "tss.zeiko_procでフリー在庫レコードが無い", "zaiko_proc(" + in_buhin_cd + "," + in_zaiko_kbn + "," + in_torihikisaki_cd + "," + in_juchu_cd1 + "," + in_juchu_cd2 + "," + in_su + ")");
                    bl = false;
                    return bl;
                }

                //フリー在庫に残りの在庫数を更新
                w_sql = "UPDATE tss_buhin_zaiko_m SET zaiko_su = '" + w_zaiko_su.ToString("0.00") + "',update_user_cd = '" + user_cd + "',update_datetime = sysdate WHERE buhin_cd = '" + in_buhin_cd + "' and zaiko_kbn = '01' and torihikisaki_cd = '" + in_torihikisaki_cd + "' and juchu_cd1 = '9999999999999999' and juchu_cd2 = '9999999999999999'";
                if(OracleUpdate(w_sql) == false)
                {
                    GetUser();
                    ErrorLogWrite(user_cd, "OracleUpdate", w_sql.Replace("'", "#"));
                    MessageBox.Show("データベースの処理中にエラーが発生しました。zaiko_proc 02");
                    bl = false;
                }
                //在庫履歴へ書き込み
                w_rireki_bl = OracleInsert("insert into tss_buhin_nyusyukko_m (buhin_syori_kbn,buhin_syori_no,seq,buhin_syori_date,buhin_cd,zaiko_kbn,torihikisaki_cd,juchu_cd1,juchu_cd2,suryou,bikou,create_user_cd,create_datetime) values ('" + w_rireki_kbn + "','" + in_rireki_no.ToString("0") + "','" + ppt_gyou.ToString() + "',sysdate,'" + in_buhin_cd + "','" + "01" + "','" + in_torihikisaki_cd + "','" + "9999999999999999" + "','" + "9999999999999999" + "','" + in_su.ToString() + "','売上番号" + in_bikou + "分のロット在庫不足分の消し込み','" + user_cd + "',sysdate)");
                ppt_gyou++;
            }
            else
            {
                //通常に在庫の更新
                GetUser();
                w_sql = "UPDATE tss_buhin_zaiko_m SET zaiko_su = '" + w_zaiko_su.ToString("0.00") + "',update_user_cd = '" + user_cd + "',update_datetime = sysdate where buhin_cd = '" + in_buhin_cd + "' and zaiko_kbn = '" + w_dt.Rows[0]["zaiko_kbn"].ToString() + "' and torihikisaki_cd = '" + w_dt.Rows[0]["torihikisaki_cd"].ToString() + "' and juchu_cd1 = '" + w_dt.Rows[0]["juchu_cd1"].ToString() + "' and juchu_cd2 = '" + w_dt.Rows[0]["juchu_cd2"].ToString() + "'";
                if(OracleUpdate(w_sql) == false)
                {
                    GetUser();
                    ErrorLogWrite(user_cd, "OracleUpdate", w_sql.Replace("'", "#"));
                    MessageBox.Show("データベースの処理中にエラーが発生しました。zaiko_proc 03");
                    bl = false;
                }
                //在庫履歴へ書き込み
                w_rireki_bl = OracleInsert("insert into tss_buhin_nyusyukko_m (buhin_syori_kbn,buhin_syori_no,seq,buhin_syori_date,buhin_cd,zaiko_kbn,torihikisaki_cd,juchu_cd1,juchu_cd2,suryou,bikou,create_user_cd,create_datetime) values ('" + w_rireki_kbn + "','" + in_rireki_no.ToString("0") + "','" + ppt_gyou.ToString() + "',sysdate,'" + in_buhin_cd + "','" + w_dt.Rows[0]["zaiko_kbn"].ToString() + "','" + in_torihikisaki_cd + "','" + w_dt.Rows[0]["juchu_cd1"].ToString() + "','" + w_dt.Rows[0]["juchu_cd2"].ToString() + "','" + in_su.ToString("0.00") + "','売上番号" + in_bikou + "分の消し込み','" + user_cd + "',sysdate)");
                ppt_gyou++;
            }
            return bl;
        }
        #endregion


    }
    #endregion
}
